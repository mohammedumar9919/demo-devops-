using System.Text.Json;
using AuthDemo.Api.Models;
using Microsoft.Extensions.Options;

namespace AuthDemo.Api.Services;

public sealed class UserStore : IUserStore
{
    public sealed class UserStoreOptions
    {
        public string JsonPath { get; set; } = "App_Data/users.json";
    }

    private readonly string _path;
    private readonly SemaphoreSlim _lock = new(1, 1);

    // cached users
    private UsersFile? _cache;

    public UserStore(IOptions<UserStoreOptions> options, IWebHostEnvironment env)
    {
        var configured = options.Value.JsonPath ?? "App_Data/users.json";
        _path = Path.IsPathRooted(configured)
            ? configured
            : Path.Combine(env.ContentRootPath, configured);
    }

    public async Task<UserRecord?> FindByUsernameAsync(string username, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;

        var usersFile = await LoadAsync(ct);

        return usersFile.Users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<UsersFile> LoadAsync(CancellationToken ct)
    {
        if (_cache is not null) return _cache;

        await _lock.WaitAsync(ct);
        try
        {
            if (_cache is not null) return _cache;

            if (!File.Exists(_path))
                throw new FileNotFoundException($"users.json not found at: {_path}");

            await using var fs = File.OpenRead(_path);

            var data = await JsonSerializer.DeserializeAsync<UsersFile>(
                fs,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                ct
            );

            _cache = data ?? new UsersFile();
            return _cache;
        }
        finally
        {
            _lock.Release();
        }
    }
}
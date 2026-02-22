using System.Security.Claims;
using AuthDemo.Api.Models;

namespace AuthDemo.Api.Services;

public sealed class AuthService : IAuthService
{
    private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        "admin",
        "user"
    };

    private readonly IUserStore _store;
    private readonly JwtTokenService _jwt;

    public AuthService(IUserStore store, JwtTokenService jwt)
    {
        _store = store;
        _jwt = jwt;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var user = await _store.FindByUsernameAsync(request.Username, ct);
        if (user is null) return null;

        if (!AllowedRoles.Contains(user.Role)) return null;

        var ok = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!ok) return null;

        var token = _jwt.CreateToken(user);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        };
    }

    public MeResponse BuildMe(ClaimsPrincipal user)
    {
        var username =
            user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
            ?? user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? user.Identity?.Name
            ?? "";

        var role =
            user.Claims.FirstOrDefault(c => c.Type == "role")?.Value
            ?? user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
            ?? "";

        return new MeResponse { Username = username, Role = role };
    }   
}
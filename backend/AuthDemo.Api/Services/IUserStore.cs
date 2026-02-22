using AuthDemo.Api.Models;

namespace AuthDemo.Api.Services;

public interface IUserStore
{
    Task<UserRecord?> FindByUsernameAsync(string username, CancellationToken ct);
}
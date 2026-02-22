using AuthDemo.Api.Models;
using System.Security.Claims;

namespace AuthDemo.Api.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct);
    MeResponse BuildMe(ClaimsPrincipal user);
}
using AuthDemo.Api.Models;
using AuthDemo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Api.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    // Spec: POST /auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var result = await _auth.LoginAsync(req, ct);

        // Spec failure shape:
        // { "message": "Invalid username or password" }
        if (result is null)
            return Unauthorized(new { message = "Invalid username or password" });

        // Spec success shape:
        // { "token": "...", "username": "admin", "role": "admin" }
        return Ok(new
        {
            token = result.Token,
            username = result.Username,
            role = result.Role
        });
    }

    // Spec: GET /auth/me (requires auth)
    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var me = _auth.BuildMe(User);

        if (string.IsNullOrWhiteSpace(me.Username))
            return Unauthorized(new { message = "Unauthorized" });

        return Ok(new
        {
            username = me.Username,
            role = me.Role
        });
    }
}
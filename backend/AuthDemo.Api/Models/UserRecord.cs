namespace AuthDemo.Api.Models;

public sealed class UserRecord
{
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "";
}
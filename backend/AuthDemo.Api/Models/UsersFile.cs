namespace AuthDemo.Api.Models;

public sealed class UsersFile
{
    public List<UserRecord> Users { get; set; } = new();
}
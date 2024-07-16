namespace aspnet.webapi.Entities;

public class User
{
    public string Email { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string[] Roles { get; set; } = null!;
    public string Username { get; set; } = string.Empty;
}

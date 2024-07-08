namespace aspnet.webapi.Models;

public class UserResponse
{
    public UserResponseProps User { get; set; } = null!;
}

public class UserResponseProps
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Bio { get; set; } = null!;
    public string? Image { get; set; } = null!;
}

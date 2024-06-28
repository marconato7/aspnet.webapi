using System.ComponentModel.DataAnnotations;

namespace aspnet.webapi.Entities;

public class User
{
    public string Email { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string[] Roles { get; set; } = null!;
    public string Username { get; set; } = string.Empty;
}

public class RegistrationRequest
{
    [Required]
    public string Username { get; init; } = string.Empty;
    [Required]
    public string Email { get; init; } = string.Empty;
    [Required]
    public string Password { get; init; } = string.Empty;
}

public class RegistrationResponse
{
    [Required]
    public string Username { get; init; } = string.Empty;
    [Required]
    public string Email { get; init; } = string.Empty;
    [Required]
    public string Password { get; init; } = string.Empty;
}

public class AuthenticationRequest
{
    [Required]
    public string Username { get; init; } = string.Empty;
    [Required]
    public string Email { get; init; } = string.Empty;
    [Required]
    public string Password { get; init; } = string.Empty;
}

public class AuthenticationResponse
{
    [Required]
    public string Username { get; init; } = string.Empty;
    [Required]
    public string Email { get; init; } = string.Empty;
    [Required]
    public string Password { get; init; } = string.Empty;
}

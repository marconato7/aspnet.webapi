using System.ComponentModel.DataAnnotations;

namespace aspnet.webapi.Models;

public class AuthenticationRequest
{
    [Required] public AuthenticationRequestProps User { get; set; } = null!;
}

public class AuthenticationRequestProps
{
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}

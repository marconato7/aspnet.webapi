using System.ComponentModel.DataAnnotations;

namespace aspnet.webapi.Models;

public class RegistrationRequest
{
    [Required] public RegistrationRequestProps User { get; set; } = null!;
}

public class RegistrationRequestProps
{
    [Required] public string Username { get; set; } = null!;
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}

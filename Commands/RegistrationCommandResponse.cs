using System.ComponentModel.DataAnnotations;
using aspnet.webapi.Entities;

namespace aspnet.webapi.Commands;

public class RegistrationCommandResponse
{
    [Required] public UserEntity User { get; set; } = null!;
    [Required] public string Token { get; set; } = null!;
}

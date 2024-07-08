using System.ComponentModel.DataAnnotations;
using MediatR;

namespace aspnet.webapi.Commands;

public class RegistrationCommand : IRequest<RegistrationCommandResponse?>
{
    [Required] public string Username { get; set; } = null!;
    [Required] [EmailAddress] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}

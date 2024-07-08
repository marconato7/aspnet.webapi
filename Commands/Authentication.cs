using aspnet.webapi.Data;
using aspnet.webapi.Entities;
using aspnet.webapi.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace aspnet.webapi.Commands;

public class AuthenticationCommandHandler(TokenService tokenService, ApplicationDbContext context) : IRequestHandler<AuthenticationCommand, AuthenticationDto?>
{
    private readonly TokenService _tokenService = tokenService;
    private readonly ApplicationDbContext _context = context;

    public async Task<AuthenticationDto?> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Set<UserEntity>().AsNoTracking().SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user is null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return null;
        }

        var token = _tokenService.Generate(user);

        var authenticationDto = new AuthenticationDto
        {
            User = user,
            Token = token,
        };

        return authenticationDto;
    }
}

public class AuthenticationCommand : IRequest<AuthenticationDto?>
{
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}

public class AuthenticationDto
{
    [Required] public UserEntity User { get; set; } = null!;
    [Required] public string Token { get; set; } = null!;
}

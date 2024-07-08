using aspnet.webapi.Data;
using aspnet.webapi.Entities;
using aspnet.webapi.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace aspnet.webapi.Commands;

public class RegistrationCommandHandler(ILogger<RegistrationCommandHandler> logger, TokenService tokenService, ApplicationDbContext context) : IRequestHandler<RegistrationCommand, RegistrationCommandResponse?>
{
    private readonly ILogger<RegistrationCommandHandler> _logger = logger;
    private readonly TokenService _tokenService = tokenService;
    private readonly ApplicationDbContext _context = context;

    public async Task<RegistrationCommandResponse?> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        var userFromDb = await _context.Set<UserEntity>()
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (userFromDb is null)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);

            var user = UserEntity.Create(request.Email, request.Username, passwordHash, null, null);

            var token = _tokenService.Generate(user);

            _context.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            return new RegistrationCommandResponse
            {
                User = user,
                Token = token,
            };
        }

        return null;
    }
}

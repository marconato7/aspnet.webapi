using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using aspnet.webapi.Entities;
using Microsoft.IdentityModel.Tokens;

namespace aspnet.webapi.Services;

public class TokenService(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public string Generate(User user)
    {
        var stringKeyFromSettings = _configuration.GetSection("JwtSettings:Key").Value!;

        var stringKeyFromSettingsAsByteArray = Encoding.UTF8.GetBytes(stringKeyFromSettings);

        var key = new SymmetricSecurityKey(stringKeyFromSettingsAsByteArray);

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration.GetSection("JwtSettings:Audience").Value!,
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = _configuration.GetSection("JwtSettings:Issuer").Value!,
            SigningCredentials = credentials,
            Subject = GenerateClaims(user),
        };

        var handler = new JwtSecurityTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        var jwt = handler.WriteToken(token);

        return jwt;
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        // claimsIdentity.AddClaim(new Claim("userId", user.Id.ToString()));

        if(user.Roles.IsNullOrEmpty())
        {
            return claimsIdentity;
        }
        
        foreach (var role in user.Roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return claimsIdentity;
    }
}

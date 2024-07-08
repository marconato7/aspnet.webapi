using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using aspnet.webapi.Entities;
using aspnet.webapi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace aspnet.webapi.Services;

public class TokenService(IOptions<JwtOptions> jwtOptions)
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string Generate(UserEntity user)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = signingCredentials,
            Subject = GenerateClaims(user),
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

        var jwt = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        return jwt;
    }

    private ClaimsIdentity GenerateClaims(UserEntity user)
    {
        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, _jwtOptions.Issuer));
        // claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, _jwtOptions.Audience));
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes).ToString()));
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));
        claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

        // if(user.Roles.IsNullOrEmpty())
        // {
        //     return claimsIdentity;
        // }
        
        // foreach (var role in user.Roles)
        // {
        //     claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
        // }

        return claimsIdentity;
    }
}

namespace aspnet.webapi.Models;

public class JwtOptions
{
    public const string Jwt = "Jwt";

    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Key { get; set; } = null!;
    public double ExpiresInMinutes { get; set; }
    // public string Issuer { get; set; }
    // public string Audience { get; set; }
    // public SigningCredentials SigningCredentials { get; set; }
    // public int AccessTokenExpiration { get; set; }
    // public int RefreshTokenExpiration { get; set; }
}

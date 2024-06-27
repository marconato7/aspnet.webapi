using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using aspnet.webapi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace aspnet.webapi.Controllers;

[ApiController]
// [Route("[controller]")]
public class ApplicationController(ILogger<ApplicationController> logger, IConfiguration configuration) : ControllerBase
{
    private readonly static User MyUser = new();

    private readonly ILogger<ApplicationController> _logger = logger;
    private readonly IConfiguration _configuration = configuration;

    // [HttpGet(Name = "all")]
    [HttpGet]
    // public async Task<IEnumerable<Entity>> Get()
    [Route("")]
    public IActionResult Get()
    {
        return Ok("test");
        // await using var ctx = new ApplicationDbContext();
        // await ctx.Database.EnsureDeletedAsync();
        // await ctx.Database.EnsureCreatedAsync();

        // ctx.Set<Entity>().Add(new() { Name = "FooEntity" });

        // await ctx.SaveChangesAsync();

        // var entities = await ctx.Set<Entity>().Where(e => e.Name.StartsWith('F')).ToListAsync();

        // return Enumerable.Range(1, 5).Select(index => new Entity
        // {
        //     Name = index.ToString(),
        // })
        // .ToArray();
    }

    // [HttpGet(Name = "identity-test")]
    [HttpGet]
    [Route("identity-test")]
    [Authorize]
    public IActionResult IdentityTest()
    // public async Task<IEnumerable<Entity>> Get()
    {
        return Ok("test");
        // await using var ctx = new ApplicationDbContext();
        // await ctx.Database.EnsureDeletedAsync();
        // await ctx.Database.EnsureCreatedAsync();

        // ctx.Set<Entity>().Add(new() { Name = "FooEntity" });

        // await ctx.SaveChangesAsync();

        // var entities = await ctx.Set<Entity>().Where(e => e.Name.StartsWith('F')).ToListAsync();

        // return Enumerable.Range(1, 5).Select(index => new Entity
        // {
        //     Name = index.ToString(),
        // })
        // .ToArray();
    }

    // [HttpPost(Name = "identity-test")]
    [HttpPost]
    [Route("registration")]
    [AllowAnonymous]
    public ActionResult<RegistrationResponse> Registration([FromBody] RegistrationRequest request)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);

        MyUser.Email = request.Email;
        MyUser.Username = request.Username;
        MyUser.Password = hash;

        return Ok(MyUser);

        // var tokenHandler = new JwtSecurityTokenHandler();
        // var key = Encoding.UTF8.GetBytes("Key#123");

        // var claims = new List<Claim>
        // {
        //     new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //     new(JwtRegisteredClaimNames.Sub, request.Email),
        //     new(JwtRegisteredClaimNames.Email, request.Email),
        //     new("userId", request.UserId.ToString()),
        // };

        // foreach (var claimPair in request.CustomClaims)
        // {
        //     var jsonElement = (JsonElement) claimPair.Value;
        //     var valueType = jsonElement.ValueKind switch
        //     {
        //         JsonValueKind.True => ClaimValueTypes.Boolean,
        //         JsonValueKind.False => ClaimValueTypes.Boolean,
        //         JsonValueKind.Number => ClaimValueTypes.Boolean,
        //         _ => ClaimValueTypes.String,
        //     };

        //     var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);
        //     claims.Add(claim);
        // }

        // var tokenDescriptor = new SecurityTokenDescriptor
        // {
        //     Subject = new ClaimsIdentity(claims),
        //     Expires = DateTime.UtcNow.AddDays(1),
        //     Issuer = "Issuer",
        //     Audience = "Audience",
        //     SigningCredentials = new SigningCredentials(new SymmetricSecurityKey())
        // };

        // var token = tokenHandler.CreateToken(tokenDescriptor);

        // var jwt = tokenHandler.WriteToken(token);

        // return Ok(jwt);
    }

    // [HttpPost(Name = "identity-test")]
    [HttpPost]
    [Route("authentication")]
    [AllowAnonymous]
    public ActionResult<AuthenticationResponse> Authentication([FromBody] AuthenticationRequest request)
    {
        if (request.Username != MyUser.Username)
        {
            return BadRequest("invalid credentials");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, MyUser.Password))
        {
            return BadRequest("invalid credentials");
        }

        string token = CreateToken(MyUser);

        return Ok(token);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = [ new Claim(ClaimTypes.Name, user.Username) ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Key").Value!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}

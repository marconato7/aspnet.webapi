using aspnet.webapi.Data;
using aspnet.webapi.Entities;
using aspnet.webapi.Models;
using aspnet.webapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace aspnet.webapi.Controllers;

[ApiController]
[Route("api")]
public class ApplicationController(ILogger<ApplicationController> logger, TokenService tokenService, ApplicationDbContext context) : ControllerBase
{
    private readonly static User _user = new();
    private readonly ILogger<ApplicationController> _logger = logger;
    private readonly TokenService _tokenService = tokenService;
    private readonly ApplicationDbContext _context = context;

    // var premiumUser = User.IsInRole("premium");
    // var userName = User.Identity?.Name;
    // var naem = User.Identity?.Name ?? string.Empty;

    // Example request body:
    // {
    // "user":{
    //     "username": "Jacob",
    //     "email": "jake@jake.jake",
    //     "password": "jakejake"
    // }
    // }
    
    // Returns a User
    // Required fields: email, username, password
    [HttpPost]
    [Route("users")]
    [AllowAnonymous]
    public ActionResult<RegistrationResponse> Registration([FromBody] RegistrationRequest request)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(request.Password, 12);

        _user.Email = request.Email;
        _user.Password = hash;
        _user.Username = request.Username;

        return Ok(_user);
    }

    [HttpPost]
    [Route("authentication")]
    [AllowAnonymous]
    public ActionResult<UserResponse> Authentication([FromBody] AuthenticationRequest request)
    {
        if (request.Username != _user.Username)
        {
            return BadRequest("invalid credentials");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, _user.Password))
        {
            return BadRequest("invalid credentials");
        }

        string jwt = _tokenService.Generate(_user);

        return Ok();
    }
}

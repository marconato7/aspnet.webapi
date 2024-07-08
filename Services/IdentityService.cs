// using aspnet.webapi.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Options;

// namespace aspnet.webapi.Services;

// public class IdentityService
// (
//     SignInManager<IdentityUser> signInManager,
//     UserManager<IdentityUser> userManager,
//     TokenService tokenService,
//     IOptions<JwtSettings> jwtSettings
// )
// {
//     private readonly SignInManager<IdentityUser> _signInManager = signInManager;
//     private readonly UserManager<IdentityUser> _userManager = userManager;
//     private readonly TokenService _tokenService = tokenService;
//     private readonly JwtSettings _jwtSettings = jwtSettings.Value;

//     public async Task Registration([FromBody] RegistrationRequest request)
//     {
//         var user = new IdentityUser
//         {
//             Email = request.User.Email,
//             EmailConfirmed = true,
//             UserName = request.User.Email,
//         };

//         var result = await _userManager.CreateAsync(user, request.User.Password);
//         if (result.Succeeded)
//         {
//             await _userManager.SetLockoutEnabledAsync(user, false);
//         }
//     }

//     public async Task Authentication([FromBody] AuthenticationRequest request)
//     {
//         var result = await _signInManager.PasswordSignInAsync(request.User.Email, request.User.Password, false, true);
//         if (result.Succeeded)
//         {
//             var user = await _userManager.FindByEmailAsync(request.User.Email);
//             if (user is not null)
//             {
//                 // await _tokenService.Generate(user);
//             }
//         }
//     }
// }

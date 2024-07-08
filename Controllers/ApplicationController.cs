using System.Security.Claims;
using aspnet.webapi.Commands;
using aspnet.webapi.Data;
using aspnet.webapi.Entities;
using aspnet.webapi.Models;
using aspnet.webapi.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspnet.webapi.Controllers;

[ApiController]
[Route("api")]
public class ApplicationController(IMediator mediator, ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMediator _mediator = mediator;
    // var premiumUser = User.IsInRole("premium");
    // var userName = User.Identity?.Name;
    // var naem = User.Identity?.Name ?? string.Empty;
    // var isAuthenticated = User.Identity?.IsAuthenticated;
    // var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

    [HttpPost]
    [Route("users/login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponse>> Authentication([FromBody] AuthenticationRequest request)
    {
        var command = new AuthenticationCommand
        {
            Email = request.User.Email,
            Password = request.User.Password,
        };

        var response = await _mediator.Send(command);
        if (response is null)
        {
            return BadRequest();
        }

        var userResponse = new UserResponse
        {
            User = new UserResponseProps
            {
                Username = response.User.Username,
                Email = response.User.Email,
                Token = response.Token,
                Bio = response.User.Bio,
                Image = response.User.Image,
            },
        };

        return Ok(userResponse);
    }

    [HttpPost]
    [Route("users")]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponse>> Registration([FromBody] RegistrationRequest request)
    {
        var command = new RegistrationCommand
        {
            Username = request.User.Username,
            Email = request.User.Email,
            Password = request.User.Password,
        };

        var response = await _mediator.Send(command);
        if (response is null)
        {
            return BadRequest();
        }

        var userResponse = new UserResponse
        {
            User = new UserResponseProps
            {
                Username = response.User.Username,
                Email = response.User.Email,
                Token = response.Token,
                Bio = response.User.Bio,
                Image = response.User.Image,
            },
        };

        return Created(nameof(Registration), userResponse);
    }

    [HttpPost]
    [Route("articles")]
    [Authorize]
    public async Task<ActionResult<ArticleResponse>> CreateArticle([FromBody] CreateArticleRequest request)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var command = new CreateArticleCommand
        {
            UserEmail = userEmail!,
            Title = request.Article.Title,
            Description = request.Article.Description,
            Body = request.Article.Body,
            TagList = request.Article.TagList,
        };

        var response = await _mediator.Send(command);
        if (response is null)
        {
            return BadRequest();
        }

        ArticleResponse articleResponse = ToArticleResponse(response);

        return Created(nameof(CreateArticle), articleResponse);
    }

    [HttpGet]
    [Route("users")]
    [AllowAnonymous]
    public async Task<ActionResult> Users(CancellationToken cancellationToken)
    {
        return Ok(await _context.Set<UserEntity>().Include(u => u.Articles).ToListAsync(cancellationToken));
    }

    [HttpGet]
    [Route("articles/{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<ArticleResponse>> GetArticle([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var command = new GetArticleQuery
        {
            Slug = slug,
        };

        var response = await _mediator.Send(command);
        if (response is null)
        {
            return BadRequest();
        }

        ArticleResponse articleResponse = ToArticleResponse(response);

        return Ok(articleResponse);
    }

    [HttpGet]
    [Route("tags")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> GetTags(CancellationToken cancellationToken)
    {
        var query = new GetTagsQuery();

        var response = await _mediator.Send(query, cancellationToken);
        if (response is null)
        {
            return BadRequest();
        }

        var articleResponse = new ListofTags
        {
            Tags = response.Tags.Select(t => t.Name).ToArray(),
        };

        return Ok(articleResponse);
    }

    private static ArticleResponse ToArticleResponse(ArticleDto response)
    {
        return new ArticleResponse
        {
            Article = new ArticleResponseProps
            {
                Slug = response.Article.Slug,
                Title = response.Article.Title,
                Description = response.Article.Description,
                Body = response.Article.Body,
                TagList = response.Article.Tags?.Select(t => t.Name).ToArray(),
            },
        };
    }
}

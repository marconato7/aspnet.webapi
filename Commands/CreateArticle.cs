using System.ComponentModel.DataAnnotations;
using MediatR;
using aspnet.webapi.Data;
using aspnet.webapi.Entities;
using Microsoft.EntityFrameworkCore;
using Slugify;
using aspnet.webapi.Models;

namespace aspnet.webapi.Commands;

public class CreateArticleCommandHandler(ApplicationDbContext context) : IRequestHandler<CreateArticleCommand, ArticleDto?>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ArticleDto?> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Set<UserEntity>().SingleOrDefaultAsync(u => u.Email == request.UserEmail, cancellationToken);
        if (user is null)
        {
            return null;
        }

        var config = new SlugHelperConfiguration
        {
            ForceLowerCase = true,
            CollapseWhiteSpace = true,
            TrimWhitespace = true,
            CollapseDashes = true,
        };

        var helper = new SlugHelper(config);

        var slug = helper.GenerateSlug(request.Title);

        var articleFromDb = await _context.Set<ArticleEntity>().AsNoTracking().SingleOrDefaultAsync(a => a.Slug == slug, cancellationToken);
        if (articleFromDb is not null)
        {
            return null;
        }

        var article = ArticleEntity.Create(request.Title, request.Description, request.Body, request.TagList);

        var articleDto = new ArticleDto
        {
            Article = article,
        };

        _context.Add(article);

        user.Articles.Add(article);

        await _context.SaveChangesAsync(cancellationToken);

        return articleDto;
    }
}

public class CreateArticleCommand : IRequest<ArticleDto?>
{
    [Required] public string UserEmail { get; set; } = null!;
    [Required] public string Title { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public string Body { get; set; } = null!;
    public string[]? TagList { get; set; } = null!;
}

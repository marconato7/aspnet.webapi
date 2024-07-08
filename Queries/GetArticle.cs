using System.ComponentModel.DataAnnotations;
using MediatR;
using aspnet.webapi.Data;
using aspnet.webapi.Entities;
using Microsoft.EntityFrameworkCore;
using aspnet.webapi.Models;

namespace aspnet.webapi.Queries;

public class GetArticleQueryHandler(ApplicationDbContext context) : IRequestHandler<GetArticleQuery, ArticleDto?>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ArticleDto?> Handle(GetArticleQuery request, CancellationToken cancellationToken)
    {
        var articleFromDb = await _context.Set<ArticleEntity>().Include(a => a.Tags).AsNoTracking().SingleOrDefaultAsync(a => a.Slug == request.Slug, cancellationToken);
        if (articleFromDb is null)
        {
            return null;
        }

        var articleDto = new ArticleDto
        {
            Article = articleFromDb,
        };

        return articleDto;
    }
}

public class GetArticleQuery : IRequest<ArticleDto?>
{
    [Required] public string Slug { get; set; } = null!;
}

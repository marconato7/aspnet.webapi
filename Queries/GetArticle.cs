using System.ComponentModel.DataAnnotations;
using MediatR;
using aspnet.webapi.Data;
using aspnet.webapi.Entities;
using Microsoft.EntityFrameworkCore;
using aspnet.webapi.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace aspnet.webapi.Queries;

public class GetArticleQueryHandler(ApplicationDbContext context, IDistributedCache cache) : IRequestHandler<GetArticleQuery, ArticleDto?>
{
    private readonly ApplicationDbContext _context = context;
    private readonly IDistributedCache _cache = cache;

    public async Task<ArticleDto?> Handle(GetArticleQuery request, CancellationToken cancellationToken)
    {
        string key = $"articles-{request.Slug}";

        await _cache.GetOrCreateAsync(key, async (cancellationToken) =>
        {
            var articleFromDb = await _context
                .Set<ArticleEntity>()
                .Include(a => a.Tags)
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Slug == request.Slug, cancellationToken);

            return articleFromDb;
        }, null, cancellationToken);

        var articleFromCache = await _cache.GetStringAsync(key, cancellationToken);

        if (!string.IsNullOrWhiteSpace(articleFromCache))
        {
            var article = JsonSerializer.Deserialize<ArticleDto>(articleFromCache);
            if (article is not null)
            {
                return article;
            }
        }

        var articleFromDb = await _context
            .Set<ArticleEntity>()
            .Include(a => a.Tags)
            .AsNoTracking()
            .SingleOrDefaultAsync(a => a.Slug == request.Slug, cancellationToken);

        if (articleFromDb is null)
        {
            return null;
        }

        var articleDto = new ArticleDto
        {
            Article = articleFromDb,
        };

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(articleDto), token: cancellationToken);

        return articleDto;
    }
}

public class GetArticleQuery : IRequest<ArticleDto?>
{
    [Required] public string Slug { get; set; } = null!;
}

public static class CacheAside
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    private static readonly DistributedCacheEntryOptions Default = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
    };

    public static async Task<T?> GetOrCreateAsync<T>
    (
        this IDistributedCache cache,
        string key,
        Func<CancellationToken, Task<T>> factory,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var cachedValue = await cache.GetStringAsync(key, cancellationToken);

        T? value;
        if (!string.IsNullOrWhiteSpace(cachedValue))
        {
            value = JsonSerializer.Deserialize<T>(cachedValue);
            if (value is not null)
            {
                return value;
            }
        }

        var hasLock = await _semaphore.WaitAsync(500, cancellationToken);
        if (!hasLock)
        {
            return default;
        }

        try
        {
            value = await factory(cancellationToken);

            if (value is null)
            {
                return default;
            }

            await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options ?? Default, cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }

        return value;
    }
}

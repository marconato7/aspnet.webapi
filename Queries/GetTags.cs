using System.ComponentModel.DataAnnotations;
using MediatR;
using aspnet.webapi.Data;
using aspnet.webapi.Entities;
using Microsoft.EntityFrameworkCore;

namespace aspnet.webapi.Queries;

public class GetTagsQueryHandler(ApplicationDbContext context) : IRequestHandler<GetTagsQuery, GetTagsDto?>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<GetTagsDto?> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var tagsFromDb = await _context
            .Set<TagEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (tagsFromDb is null)
        {
            return null;
        }

        var getTagsDto = new GetTagsDto
        {
            Tags = tagsFromDb,
        };

        return getTagsDto;
    }
}

public class GetTagsQuery : IRequest<GetTagsDto?>;

public class GetTagsDto
{
    [Required] public List<TagEntity> Tags { get; set; } = null!;
}

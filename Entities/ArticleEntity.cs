using Slugify;

namespace aspnet.webapi.Entities;

public class ArticleEntity
{
    public Guid Id { get; private set; }
    public string Slug { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Body { get; private set; } = null!;
    public List<TagEntity>? Tags { get; private set; } = [];
    public UserEntity Author { get; private set; } = null!;
    // public CommentEntity[] Comments { get; private set; } = null!;
    // "createdAt": "2016-02-18T03:22:56.637Z",
    // "updatedAt": "2016-02-18T03:48:35.824Z",
    // "favorited": false,
    // "favoritesCount": 0,
    // "author": {
    //   "username": "jake",
    //   "bio": "I work at statefarm",
    //   "image": "https://i.stack.imgur.com/xHWG8.jpg",
    //   "following": false

    public static ArticleEntity Create(string title, string description, string body, string[]? tagList)
    {
        return new(title, description, body, tagList);
    }

    protected ArticleEntity() {}

    private ArticleEntity(string title, string description, string body, string[]? tagList)
    {
        List<TagEntity> tags = [];

        if (tagList is not null)
        {
            foreach (var tag in tagList)
            {
                tags.Add(TagEntity.Create(tag));
            }
        }

        var config = new SlugHelperConfiguration
        {
            ForceLowerCase = true,
            CollapseWhiteSpace = true,
            TrimWhitespace = true,
            CollapseDashes = true,
        };

        var helper = new SlugHelper(config);

        var slug = helper.GenerateSlug(title);

        Id = Guid.NewGuid();
        Title = title;
        Slug = slug;
        Description = description;
        Body = body;
        Tags = tags;
    }
}

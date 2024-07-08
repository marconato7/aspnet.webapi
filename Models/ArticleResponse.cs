namespace aspnet.webapi.Models;

public class ArticleResponse
{
    public ArticleResponseProps Article { get; set; } = null!;
}

public class ArticleResponseProps
{
    public string Slug { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string[]? TagList { get; set; } = null!;
    // public string? createdAt { get; set; } = null!;
    // public string? updatedAt { get; set; } = null!;
    // public string? favorited { get; set; } = null!;
    // public string? favoritesCount { get; set; } = null!;
    // public string? author { get; set; } = null!;
}

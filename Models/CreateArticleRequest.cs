using System.ComponentModel.DataAnnotations;

namespace aspnet.webapi.Models;

public class CreateArticleRequest
{
    [Required] public CreateArticleRequestProps Article { get; set; } = null!;
}

public class CreateArticleRequestProps
{
    [Required] public string Title { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public string Body { get; set; } = null!;
    public string[]? TagList { get; set; } = null!;
}

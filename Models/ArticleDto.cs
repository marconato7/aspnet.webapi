using System.ComponentModel.DataAnnotations;
using aspnet.webapi.Entities;

namespace aspnet.webapi.Models;

public class ArticleDto
{
    [Required] public ArticleEntity Article { get; set; } = null!;
}

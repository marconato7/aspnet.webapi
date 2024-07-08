using System.ComponentModel.DataAnnotations;

namespace aspnet.webapi.Models;

public class ListofTags
{
    [Required] public string[] Tags { get; set; } = null!;
}

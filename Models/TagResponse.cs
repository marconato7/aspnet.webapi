using System.ComponentModel.DataAnnotations;

namespace aspnet.webapi.Models;

public class TagResponse
{
    [Required] public TagResponseProps Tag { get; set; } = null!;
}

public class TagResponseProps
{
    [Required] public string Name { get; set; } = null!;
}

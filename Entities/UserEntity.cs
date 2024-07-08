namespace aspnet.webapi.Entities;

public class UserEntity
{
    public Ulid Id { get; private set; }
    public string Email { get; private set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Bio { get; set; } = null!;
    public string? Image { get; set; } = null!;
    public List<ArticleEntity> Articles { get; set; } = [];

    public static UserEntity Create(string email, string username, string password, string? bio, string? image)
    {
        return new(email, username, password, bio, image);
    }

    private UserEntity(string email, string username, string password, string? bio, string? image)
    {
        Id = Ulid.NewUlid();
        Email = email;
        Username = username;
        Password = password;
        Bio = bio;
        Image = image;
    }
}

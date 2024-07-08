namespace aspnet.webapi.Entities;

public class CommentEntity
{
    // "createdAt": "2016-02-18T03:22:56.637Z",
    // "updatedAt": "2016-02-18T03:22:56.637Z",
    // "author": {
    //   "username": "jake",
    //   "bio": "I work at statefarm",
    //   "image": "https://i.stack.imgur.com/xHWG8.jpg",
    //   "following": false
    // }
//   }

    public Guid Id { get; private set; }
    public string Body { get; private set; } = null!;
    public UserEntity Author { get; set; } = null!;

    public static CommentEntity Create(string body, string username, string password, string? bio, string? image)
    {
        return new(body);
    }

    private CommentEntity(string body)
    {
        Id = Guid.NewGuid();
        Body = body;
    }
}

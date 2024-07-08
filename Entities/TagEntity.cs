namespace aspnet.webapi.Entities;

public class TagEntity
{
    public Ulid Id { get; private set; }
    public string Name { get; private set; }

    public static TagEntity Create(string name)
    {
        return new(name);
    }

    private TagEntity(string name)
    {
        Id = Ulid.NewUlid();
        Name = name;
    }
}

using Microsoft.EntityFrameworkCore;
using aspnet.webapi.Entities;

namespace aspnet.webapi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users{ get; set; }
    public DbSet<ArticleEntity> Articles { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<TagEntity> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseInMemoryDatabase("aspnet.webapi.database");
        // => optionsBuilder.UseNpgsql(@"Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase");

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        // modelBuilder.Entity<User>().HasKey(u => u.Id);
        // modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
    }
}

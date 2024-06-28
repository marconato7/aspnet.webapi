using Microsoft.EntityFrameworkCore;
using aspnet.webapi.Entities;

namespace aspnet.webapi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseInMemoryDatabase("aspnet.webapi.database");
        // => optionsBuilder.UseNpgsql(@"Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase");

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
        // modelBuilder.Entity<User>().HasKey(u => u.Id);
        // modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
    // }
}

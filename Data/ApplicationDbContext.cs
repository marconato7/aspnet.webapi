using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using aspnet.webapi.Entities;

namespace aspnet.webapi.Data;

// public class ApplicationDbContext : DbContext
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Entity> Entities { get; set; }
    // public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseInMemoryDatabase("aspnet.webapi.database");
        // => optionsBuilder.UseNpgsql(@"Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase");

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
        // modelBuilder.Entity<User>().HasKey(u => u.Id);
        // modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
    // }
}

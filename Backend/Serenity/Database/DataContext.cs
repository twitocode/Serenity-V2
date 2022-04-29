using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Serenity.Database.Entities;

namespace Serenity.Database;

public class DataContext : IdentityDbContext<User>
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Friendship> Friendships { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Post>()
            .Property(b => b.Id)
            .HasDefaultValue(new Guid().ToString());

        builder.Entity<Comment>()
            .Property(b => b.Id)
            .HasDefaultValue(new Guid().ToString());

        builder.Entity<Friendship>()
            .Property(b => b.Id)
            .HasDefaultValue(new Guid().ToString());

        builder.Entity<Comment>()
            .Property(b => b.CreationTime)
            .HasDefaultValue(SystemClock.Instance.GetCurrentInstant());

        builder.Entity<Post>()
            .Property(b => b.CreationTime)
            .HasDefaultValue(SystemClock.Instance.GetCurrentInstant());
    }
}

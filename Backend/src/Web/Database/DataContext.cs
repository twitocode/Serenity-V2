using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serenity.Database.Entities;

namespace Serenity.Database;

public class DataContext : IdentityDbContext<User>
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Post>().Property(e => e.Id).HasConversion(id => id.ToString(), id => new Guid(id));
        builder.Entity<Comment>().Property(e => e.Id).HasConversion(id => id.ToString(), id => new Guid(id));
        builder.Entity<Comment>().Property(e => e.PostId).HasConversion(id => id.ToString(), id => new Guid(id));
    }
}

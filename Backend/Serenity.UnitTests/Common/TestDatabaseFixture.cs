using Microsoft.EntityFrameworkCore;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Tests.Common;

public class TestDatabaseFixture
{
    private const string ConnectionString = @"Host=localhost;Database=serenity_tests;Username=postgres;Password=pg_notTWILIGHT_341;Port=5432";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    // context.AddRange(
                    //     new Post { Name = "Blog1", Url = "http://blog1.com" },
                    //     new Post { Name = "Blog2", Url = "http://blog2.com" });

                    context.SaveChanges();
                }

                _databaseInitialized = true;
            }
        }
    }

    public DataContext CreateContext()
        => new DataContext(
            new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql(ConnectionString)
                .Options);
}
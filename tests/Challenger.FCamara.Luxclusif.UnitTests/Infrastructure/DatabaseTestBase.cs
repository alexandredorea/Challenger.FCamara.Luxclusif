using Challenger.FCamara.Luxclusif.Infrastructure.Persistences.Database;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.UnitTests.Infrastructure;

public abstract class DatabaseTestBase : IDisposable
{
    protected readonly ApplicationDbContext Context;
    private readonly string _databaseName;

    protected DatabaseTestBase()
    {
        // Gera um nome único para cada instância de teste
        _databaseName = Guid.NewGuid().ToString();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: _databaseName)
            .EnableSensitiveDataLogging()
            .Options;

        Context = new ApplicationDbContext(options);

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
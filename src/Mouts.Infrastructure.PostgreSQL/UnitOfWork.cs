using Mouts.Application.Interfaces;
using Mouts.Infrastructure.PostgreSQL.Context;


namespace Mouts.Infrastructure.PostgreSQL;
public class UnitOfWork : IUnitOfWork
{
    private readonly MoutsDbContext _db;

    public UnitOfWork(MoutsDbContext db)
    {
        _db = db;
    }

    public Task CommitAsync()
    {
        return _db.SaveChangesAsync();
    }
}

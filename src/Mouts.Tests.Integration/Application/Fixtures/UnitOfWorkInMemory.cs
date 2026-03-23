using Microsoft.EntityFrameworkCore;
using Mouts.Application.Interfaces;

namespace Mouts.Tests.Integration.Application.Fixtures;
public class UnitOfWorkInMemory : IUnitOfWork
{
    private readonly DbContext _context;
    public UnitOfWorkInMemory(DbContext context) => _context = context;
    public Task CommitAsync() => _context.SaveChangesAsync();
}

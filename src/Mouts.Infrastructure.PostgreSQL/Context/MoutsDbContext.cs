using Microsoft.EntityFrameworkCore;
using Mouts.Domain.Enums;
using Mouts.Domain.Entities;

namespace Mouts.Infrastructure.PostgreSQL.Context;

public class MoutsDbContext:DbContext
{
    public MoutsDbContext(DbContextOptions<MoutsDbContext> options)
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MoutsDbContext).Assembly);

        modelBuilder.HasPostgresEnum<SaleStatus>();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Sale> Sales { get; set; }
}

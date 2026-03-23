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

        modelBuilder.HasPostgresEnum<OrderStatus>();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Order> Orders { get; set; }
}

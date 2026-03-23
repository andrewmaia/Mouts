using Mouts.Application.Repositories;
using Mouts.Domain.Entities;
using Mouts.Domain.Enums;
using Mouts.Infrastructure.PostgreSQL.Context;

namespace Mouts.Infrastructure.PostgreSQL.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly MoutsDbContext _db;

    public OrderRepository(MoutsDbContext db)
    {
        _db = db;
    }

    public void Add(Order order)
    {
        _db.Orders.Add(order);
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        var entity = await _db.Orders.FindAsync(id);
        return entity;
    }

    public IEnumerable<Order> GetOpenOrders()
    {
        return _db.Orders
            .Where(o => o.Status == OrderStatus.Open)
            .ToList();
    }
}

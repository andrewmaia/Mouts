using Microsoft.EntityFrameworkCore;
using Mouts.Application.Repositories;
using Mouts.Domain.Entities;
using Mouts.Domain.Enums;

namespace Mouts.Tests.Integration.Application.Repositories;

public class OrderRepositoryInMemory : IOrderRepository
{
    private readonly DbSet<Order> _orders;

    public OrderRepositoryInMemory(DbContext context)
    {
        _orders = context.Set<Order>();
    }

    public void Add(Order order)
    {
        _orders.Add(order);
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        var entity = await _orders.FindAsync(id);
        return entity;
    }

    public IEnumerable<Order> GetOpenOrders()
    {
        return _orders
            .Where(o => o.Status == OrderStatus.Open)
            .ToList();
    }
}
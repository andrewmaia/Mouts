using Mouts.Domain.Entities;

namespace Mouts.Application.Repositories;

public interface IOrderRepository
{

    void Add(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    IEnumerable<Order> GetOpenOrders();
}

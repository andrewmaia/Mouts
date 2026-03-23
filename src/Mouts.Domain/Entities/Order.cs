using Mouts.Domain.DomainEvents;
using Mouts.Domain.DomainEvents.Common;
using Mouts.Domain.Enums;

namespace Mouts.Domain.Entities;

public class Order: AggregateRoot
{
    public Order(OrderStatus status, decimal totalAmount)
    {
        Id = Guid.NewGuid();
        Status = status;
        TotalAmount = totalAmount;
    }

    public Order(Guid id, OrderStatus status,  decimal totalAmount)
    {
        Id =id;
        Status = status;
        TotalAmount = totalAmount;
    }
    public Guid Id { get; private set; }
    public OrderStatus Status { get; private set; }

    public decimal TotalAmount { get; private set; }

    public void Pay()
    {
        if (Status != OrderStatus.Open)
            throw new InvalidOperationException("Invalid Order");

        Status = OrderStatus.Paid;

        AddDomainEvent(new OrderPaidDomainEvent(Id));
    }
}

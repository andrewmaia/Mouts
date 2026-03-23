using Mouts.Domain.DomainEvents.Common;
using Mouts.Domain.Entities;

namespace Mouts.Domain.DomainEvents;
public class OrderPaidDomainEvent: IDomainEvent
{
    public Guid OrderId { get; }
    public OrderPaidDomainEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}

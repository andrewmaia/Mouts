using Mouts.Domain.DomainEvents.Common;

namespace Mouts.Domain.DomainEvents;

public class ItemCancelledDomainEvent(Guid saleId, Guid itemId) : IDomainEvent
{
    public Guid SaleId { get; } = saleId;
    public Guid ItemId { get; } = itemId;
}

using Mouts.Domain.DomainEvents.Common;

namespace Mouts.Domain.DomainEvents;

public class SaleCreatedDomainEvent(Guid saleId) : IDomainEvent
{
    public Guid SaleId { get; } = saleId;
}

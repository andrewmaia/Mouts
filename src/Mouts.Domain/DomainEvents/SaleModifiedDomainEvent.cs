using Mouts.Domain.DomainEvents.Common;

namespace Mouts.Domain.DomainEvents;

public class SaleModifiedDomainEvent(Guid saleId) : IDomainEvent
{
    public Guid SaleId { get; } = saleId;
}

using Mouts.Domain.DomainEvents.Common;

namespace Mouts.Domain.DomainEvents;

public class SaleCancelledDomainEvent(Guid saleId) : IDomainEvent
{
    public Guid SaleId { get; } = saleId;
}

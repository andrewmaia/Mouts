using Mouts.Application.Common.Events;
using Mouts.Domain.DomainEvents;

namespace Mouts.Application.EventHandlers;
public class IssueInvoiceOnOrderPaidHandler : IDomainEventHandler<OrderPaidDomainEvent>
{
    public Task Handle(OrderPaidDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Issuing invoice after payment confirmation for Order Id: " + domainEvent.OrderId);
        return Task.CompletedTask;
    }
}

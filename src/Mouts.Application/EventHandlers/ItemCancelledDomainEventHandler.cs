using Microsoft.Extensions.Logging;
using Mouts.Application.Common.Events;
using Mouts.Domain.DomainEvents;

namespace Mouts.Application.EventHandlers;

public class ItemCancelledDomainEventHandler : IDomainEventHandler<ItemCancelledDomainEvent>
{
    private readonly ILogger<ItemCancelledDomainEventHandler> _logger;

    public ItemCancelledDomainEventHandler(ILogger<ItemCancelledDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ItemCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Sale item cancelled. SaleId: {SaleId}. ItemId: {ItemId}",
            domainEvent.SaleId,
            domainEvent.ItemId);

        return Task.CompletedTask;
    }
}

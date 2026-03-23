using Microsoft.Extensions.Logging;
using Mouts.Application.Common.Events;
using Mouts.Domain.DomainEvents;

namespace Mouts.Application.EventHandlers;

public class SaleCreatedDomainEventHandler : IDomainEventHandler<SaleCreatedDomainEvent>
{
    private readonly ILogger<SaleCreatedDomainEventHandler> _logger;

    public SaleCreatedDomainEventHandler(ILogger<SaleCreatedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale created. SaleId: {SaleId}", domainEvent.SaleId);
        return Task.CompletedTask;
    }
}

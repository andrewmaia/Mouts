using Microsoft.Extensions.Logging;
using Mouts.Application.Common.Events;
using Mouts.Domain.DomainEvents;

namespace Mouts.Application.EventHandlers;

public class SaleModifiedDomainEventHandler : IDomainEventHandler<SaleModifiedDomainEvent>
{
    private readonly ILogger<SaleModifiedDomainEventHandler> _logger;

    public SaleModifiedDomainEventHandler(ILogger<SaleModifiedDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleModifiedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale modified. SaleId: {SaleId}", domainEvent.SaleId);
        return Task.CompletedTask;
    }
}

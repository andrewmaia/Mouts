using Microsoft.Extensions.Logging;
using Mouts.Application.Common.Events;
using Mouts.Domain.DomainEvents;

namespace Mouts.Application.EventHandlers;

public class SaleCancelledDomainEventHandler : IDomainEventHandler<SaleCancelledDomainEvent>
{
    private readonly ILogger<SaleCancelledDomainEventHandler> _logger;

    public SaleCancelledDomainEventHandler(ILogger<SaleCancelledDomainEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale cancelled. SaleId: {SaleId}", domainEvent.SaleId);
        return Task.CompletedTask;
    }
}

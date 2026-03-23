using Mouts.Domain.DomainEvents.Common;

namespace Mouts.Application.Common.Events;
public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}
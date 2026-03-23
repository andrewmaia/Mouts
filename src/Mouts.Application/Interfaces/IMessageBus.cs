namespace Mouts.Application.Interfaces;

public interface IMessageBus
{
    Task SendAsync<T>(string queueName,T message,CancellationToken ct = default);
}

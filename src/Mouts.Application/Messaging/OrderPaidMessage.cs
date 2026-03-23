namespace Mouts.Application.Messaging;

public sealed class OrderPaidMessage
{
    public Guid OrderId { get; init; }
}
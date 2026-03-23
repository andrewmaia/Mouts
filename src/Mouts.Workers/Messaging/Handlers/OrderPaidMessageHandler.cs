using Mouts.Application.Execution;
using Mouts.Application.Messaging;
using Mouts.Application.UseCases.ProcessOrderPaid;


namespace Mouts.Workers.Messaging.Handlers;
public sealed class OrderPaidMessageHandler
{
    private readonly RequestExecutor _executor;

    public OrderPaidMessageHandler(RequestExecutor executor)
    {
        _executor = executor;
    }

    public async Task HandleAsync(OrderPaidMessage message, CancellationToken ct)
    {
        await _executor.ExecuteAsync<ProcessOrderPaidRequest, ProcessOrderPaidResponse>(new ProcessOrderPaidRequest (message.OrderId));
    }
}
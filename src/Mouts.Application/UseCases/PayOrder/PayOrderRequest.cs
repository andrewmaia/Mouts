namespace Mouts.Application.UseCases.PayOrder;
public class PayOrderRequest
{
    public Guid OrderId { get; }

    public PayOrderRequest(Guid orderId)
    {
        OrderId = orderId;
    }
}

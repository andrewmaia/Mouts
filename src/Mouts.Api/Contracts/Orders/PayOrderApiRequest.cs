namespace Mouts.Api.Contracts.Orders;

/// <summary>
/// Represents the request data to pay an order.
/// </summary>
public class PayOrderApiRequest
{
    /// <summary>
    /// The identifier of the order to be paid.
    /// </summary>
    public Guid OrderId { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="PayOrderApiRequest"/> with the specified order ID.
    /// </summary>
    /// <param name="orderId">The ID of the order to pay. Cannot be empty.</param>
    public PayOrderApiRequest(Guid orderId)
    {
        OrderId = orderId;
    }
}


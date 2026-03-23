namespace Mouts.Api.Contracts.Orders;

/// <summary>
/// Data for creating a new order.
/// </summary>
public class CreateOrderApiRequest
{
    /// <summary>
    /// Total amount of the order.
    /// </summary>
    public decimal TotalAmount { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="CreateOrderApiRequest"/> with the specified total amount.
    /// </summary>
    /// <param name="totalAmount">The total amount of the order. Must be greater than zero.</param>
    public CreateOrderApiRequest(decimal totalAmount)
    {
        TotalAmount = totalAmount;
    }
}

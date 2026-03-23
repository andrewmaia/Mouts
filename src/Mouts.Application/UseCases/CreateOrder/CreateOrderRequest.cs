namespace Mouts.Application.UseCases.CreateOrder;
public  class CreateOrderRequest
{
    public decimal TotalAmount { get; }

    public CreateOrderRequest(decimal totalAmount)
    {
        TotalAmount = totalAmount;
    }
}
using Mouts.Application.Common;

namespace Mouts.Application.UseCases.CreateOrder;
public class CreateOrderResponse : ResultResponse
{
    public Guid? OrderId { get; set; }
}

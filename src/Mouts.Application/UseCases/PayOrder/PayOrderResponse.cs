using Mouts.Application.Common;

namespace Mouts.Application.UseCases.PayOrder;
public class PayOrderResponse: ResultResponse
{
    public Guid OrderId { get; set; }

}
 
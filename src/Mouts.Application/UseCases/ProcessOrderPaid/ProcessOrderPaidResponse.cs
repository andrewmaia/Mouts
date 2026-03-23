using Mouts.Application.Common;

namespace Mouts.Application.UseCases.ProcessOrderPaid;
public class ProcessOrderPaidResponse : ResultResponse
{
    public Guid OrderId { get; set; }

}

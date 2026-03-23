using Mouts.Application.Common;
using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.CancelSale;

public class CancelSaleResponse : ResultResponse
{
    public Guid? SaleId { get; set; }
    public SaleOutput? Sale { get; set; }
}

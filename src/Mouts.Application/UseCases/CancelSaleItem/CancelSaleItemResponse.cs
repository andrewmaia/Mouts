using Mouts.Application.Common;
using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.CancelSaleItem;

public class CancelSaleItemResponse : ResultResponse
{
    public Guid? SaleId { get; set; }
    public Guid? ItemId { get; set; }
    public SaleOutput? Sale { get; set; }
}

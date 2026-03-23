using Mouts.Application.Common;
using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.UpdateSale;

public class UpdateSaleResponse : ResultResponse
{
    public Guid? SaleId { get; set; }
    public SaleOutput? Sale { get; set; }
}

using Mouts.Application.Common;
using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.CreateSale;

public class CreateSaleResponse : ResultResponse
{
    public Guid? SaleId { get; set; }
    public SaleOutput? Sale { get; set; }
}

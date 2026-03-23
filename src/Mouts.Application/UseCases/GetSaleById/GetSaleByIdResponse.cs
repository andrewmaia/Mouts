using Mouts.Application.Common;
using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.GetSaleById;

public class GetSaleByIdResponse : ResultResponse
{
    public SaleOutput? Sale { get; set; }
}

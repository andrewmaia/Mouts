using Mouts.Application.Common;
using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.GetSales;

public class GetSalesResponse : ResultResponse
{
    public IReadOnlyCollection<SaleOutput> Sales { get; set; } = [];
    public int Page { get; set; }
    public int Size { get; set; }
    public int TotalCount { get; set; }
}

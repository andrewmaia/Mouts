using Mouts.Application.UseCases.Common;

namespace Mouts.Application.UseCases.CreateSale;

public class CreateSaleRequest
{
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public IReadOnlyCollection<SaleItemInput> Items { get; set; } = [];
}

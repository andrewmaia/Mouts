using Mouts.Domain.Enums;

namespace Mouts.Application.UseCases.Common;

public class SaleOutput
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public SaleStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public IReadOnlyCollection<SaleItemOutput> Items { get; set; } = [];
}

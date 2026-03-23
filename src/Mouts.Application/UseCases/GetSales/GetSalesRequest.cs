using Mouts.Domain.Enums;

namespace Mouts.Application.UseCases.GetSales;

public class GetSalesRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string? Order { get; set; }
    public string? SaleNumber { get; set; }
    public string? CustomerName { get; set; }
    public string? BranchName { get; set; }
    public SaleStatus? Status { get; set; }
    public DateTime? SaleDateMin { get; set; }
    public DateTime? SaleDateMax { get; set; }
    public decimal? TotalAmountMin { get; set; }
    public decimal? TotalAmountMax { get; set; }
}

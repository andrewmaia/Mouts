using Mouts.Domain.Entities;

namespace Mouts.Application.UseCases.Common;

public static class SaleOutputMapper
{
    public static SaleOutput Map(Sale sale)
    {
        return new SaleOutput
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            Status = sale.Status,
            TotalAmount = sale.TotalAmount,
            Items = sale.Items.Select(item => new SaleItemOutput
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount,
                TotalAmount = item.TotalAmount,
                IsCancelled = item.IsCancelled
            }).ToList()
        };
    }
}

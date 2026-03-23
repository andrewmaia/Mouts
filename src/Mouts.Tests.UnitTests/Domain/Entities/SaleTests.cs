using Mouts.Domain.Entities;
using Mouts.Domain.Enums;
using Mouts.Domain.Exceptions;

namespace Mouts.Tests.UnitTests.Domain.Entities;

public class SaleTests
{
    [Fact]
    public void Create_ShouldApplyNoDiscount_WhenQuantityIsLessThanFour()
    {
        var sale = CreateSale(quantity: 3, unitPrice: 10m);
        var item = sale.Items.Single();

        Assert.Equal(0m, item.Discount);
        Assert.Equal(30m, item.TotalAmount);
        Assert.Equal(30m, sale.TotalAmount);
    }

    [Fact]
    public void Create_ShouldApplyTenPercentDiscount_WhenQuantityIsBetweenFourAndNine()
    {
        var sale = CreateSale(quantity: 4, unitPrice: 10m);
        var item = sale.Items.Single();

        Assert.Equal(4m, item.Discount);
        Assert.Equal(36m, item.TotalAmount);
        Assert.Equal(36m, sale.TotalAmount);
    }

    [Fact]
    public void Create_ShouldApplyTwentyPercentDiscount_WhenQuantityIsBetweenTenAndTwenty()
    {
        var sale = CreateSale(quantity: 10, unitPrice: 10m);
        var item = sale.Items.Single();

        Assert.Equal(20m, item.Discount);
        Assert.Equal(80m, item.TotalAmount);
        Assert.Equal(80m, sale.TotalAmount);
    }

    [Fact]
    public void Create_ShouldThrow_WhenQuantityIsGreaterThanTwenty()
    {
        Assert.Throws<SaleDomainException>(() => CreateSale(quantity: 21, unitPrice: 10m));
    }

    [Fact]
    public void CancelItem_ShouldRecalculateSaleTotal()
    {
        var sale = CreateSaleWithTwoItems();
        var itemToCancel = sale.Items.First();

        sale.CancelItem(itemToCancel.Id);

        Assert.True(itemToCancel.IsCancelled);
        Assert.Equal(45m, sale.TotalAmount);
    }

    [Fact]
    public void Cancel_ShouldCancelSaleAndAllItems()
    {
        var sale = CreateSaleWithTwoItems();

        sale.Cancel();

        Assert.Equal(SaleStatus.Cancelled, sale.Status);
        Assert.Equal(0m, sale.TotalAmount);
        Assert.All(sale.Items, item => Assert.True(item.IsCancelled));
    }

    private static Sale CreateSale(int quantity, decimal unitPrice)
    {
        return new Sale(
            "S-001",
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Customer",
            Guid.NewGuid(),
            "Branch",
            [new SaleItem(Guid.NewGuid(), "Product", quantity, unitPrice)]);
    }

    private static Sale CreateSaleWithTwoItems()
    {
        return new Sale(
            "S-002",
            DateTime.UtcNow,
            Guid.NewGuid(),
            "Customer",
            Guid.NewGuid(),
            "Branch",
            [
                new SaleItem(Guid.NewGuid(), "Product A", 5, 10m),
                new SaleItem(Guid.NewGuid(), "Product B", 5, 10m)
            ]);
    }
}

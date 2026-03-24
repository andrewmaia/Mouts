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

    [Fact]
    public void Update_ShouldModifyExistingItem_WhenProductRemainsTheSame()
    {
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productA = Guid.NewGuid();
        var productB = Guid.NewGuid();

        var sale = new Sale(
            "S-010",
            DateTime.UtcNow,
            customerId,
            "Customer",
            branchId,
            "Branch",
            [
                new SaleItem(productA, "Product A", 2, 10m),
                new SaleItem(productB, "Product B", 1, 20m)
            ]);

        var originalItemId = sale.Items.Single(x => x.ProductId == productA).Id;

        sale.Update(
            "S-010-UPDATED",
            DateTime.UtcNow,
            customerId,
            "Customer Updated",
            branchId,
            "Branch",
            [
                new SaleItem(productA, "Product A", 4, 10m),
                new SaleItem(productB, "Product B", 1, 20m)
            ]);

        var updatedItem = sale.Items.Single(x => x.ProductId == productA);

        Assert.Equal(originalItemId, updatedItem.Id);
        Assert.Equal(4, updatedItem.Quantity);
        Assert.Equal(4m, updatedItem.Discount);
        Assert.Equal(56m, sale.TotalAmount);
    }

    [Fact]
    public void Update_ShouldThrow_WhenPayloadContainsDuplicateProducts()
    {
        var productId = Guid.NewGuid();
        var sale = CreateSaleWithTwoItems();

        var exception = Assert.Throws<SaleDomainException>(() =>
            sale.Update(
                "S-011",
                DateTime.UtcNow,
                Guid.NewGuid(),
                "Customer",
                Guid.NewGuid(),
                "Branch",
                [
                    new SaleItem(productId, "Product A", 1, 10m),
                    new SaleItem(productId, "Product A Duplicate", 2, 10m)
                ]));

        Assert.Equal("A sale cannot contain duplicate products.", exception.Message);
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

using Mouts.Domain.Exceptions;

namespace Mouts.Domain.Entities;

public class SaleItem
{
    private SaleItem()
    {
    }

    public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        SetDetails(productId, productName, quantity, unitPrice);
        IsCancelled = false;
    }

    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }

    public void Update(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        EnsureNotCancelled();
        SetDetails(productId, productName, quantity, unitPrice);
    }

    public void Cancel()
    {
        if (IsCancelled)
            throw new SaleDomainException("Sale item is already cancelled.");

        IsCancelled = true;
        TotalAmount = 0;
    }

    private void SetDetails(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (productId == Guid.Empty)
            throw new SaleDomainException("ProductId is required.");

        if (string.IsNullOrWhiteSpace(productName))
            throw new SaleDomainException("ProductName is required.");

        if (quantity <= 0)
            throw new SaleDomainException("Quantity must be greater than zero.");

        if (quantity > 20)
            throw new SaleDomainException("Cannot sell more than 20 identical items.");

        if (unitPrice <= 0)
            throw new SaleDomainException("UnitPrice must be greater than zero.");

        ProductId = productId;
        ProductName = productName.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = CalculateDiscount(quantity, unitPrice);
        TotalAmount = (quantity * unitPrice) - Discount;
    }

    private static decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        var grossAmount = quantity * unitPrice;

        if (quantity >= 10)
            return grossAmount * 0.20m;

        if (quantity >= 4)
            return grossAmount * 0.10m;

        return 0;
    }

    private void EnsureNotCancelled()
    {
        if (IsCancelled)
            throw new SaleDomainException("Cancelled sale items cannot be changed.");
    }
}

using Mouts.Domain.DomainEvents;
using Mouts.Domain.DomainEvents.Common;
using Mouts.Domain.Enums;
using Mouts.Domain.Exceptions;

namespace Mouts.Domain.Entities;

public class Sale : AggregateRoot
{
    private readonly List<SaleItem> _items = [];

    private Sale()
    {
    }

    public Sale(
        string saleNumber,
        DateTime saleDate,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName,
        IEnumerable<SaleItem> items)
    {
        Id = Guid.NewGuid();
        Status = SaleStatus.Active;
        SetHeader(saleNumber, saleDate, customerId, customerName, branchId, branchName);
        ReplaceItems(items);
        AddDomainEvent(new SaleCreatedDomainEvent(Id));
    }

    public Guid Id { get; private set; }
    public string SaleNumber { get; private set; } = string.Empty;
    public DateTime SaleDate { get; private set; }
    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public Guid BranchId { get; private set; }
    public string BranchName { get; private set; } = string.Empty;
    public SaleStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    public void Update(
        string saleNumber,
        DateTime saleDate,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName,
        IEnumerable<SaleItem> items)
    {
        EnsureActive();
        SetHeader(saleNumber, saleDate, customerId, customerName, branchId, branchName);
        SyncItems(items);
        AddDomainEvent(new SaleModifiedDomainEvent(Id));
    }

    public void Cancel()
    {
        if (Status == SaleStatus.Cancelled)
            throw new SaleDomainException("Sale is already cancelled.");

        Status = SaleStatus.Cancelled;
        TotalAmount = 0;

        foreach (var item in _items.Where(item => !item.IsCancelled))
            item.Cancel();

        AddDomainEvent(new SaleCancelledDomainEvent(Id));
    }

    public void CancelItem(Guid itemId)
    {
        EnsureActive();

        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
            throw new SaleDomainException($"Sale item with id '{itemId}' was not found.");

        item.Cancel();
        RecalculateTotal();
        AddDomainEvent(new ItemCancelledDomainEvent(Id, item.Id));
    }

    private void ReplaceItems(IEnumerable<SaleItem> items)
    {
        var materializedItems = items?.ToList() ?? [];

        if (materializedItems.Count == 0)
            throw new SaleDomainException("A sale must contain at least one item.");

        _items.Clear();
        _items.AddRange(materializedItems);
        RecalculateTotal();
    }

    private void SyncItems(IEnumerable<SaleItem> items)
    {
        var materializedItems = items?.ToList() ?? [];

        if (materializedItems.Count == 0)
            throw new SaleDomainException("A sale must contain at least one item.");

        var duplicatedProducts = materializedItems
            .GroupBy(item => item.ProductId)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicatedProducts.Count != 0)
            throw new SaleDomainException("A sale cannot contain duplicate products.");

        _items.RemoveAll(existingItem => materializedItems.All(newItem => newItem.ProductId != existingItem.ProductId));

        foreach (var newItem in materializedItems)
        {
            var existingItem = _items.FirstOrDefault(item => item.ProductId == newItem.ProductId);

            if (existingItem is null)
            {
                _items.Add(newItem);
                continue;
            }

            existingItem.Update(newItem.ProductId, newItem.ProductName, newItem.Quantity, newItem.UnitPrice);
        }

        RecalculateTotal();
    }

    private void SetHeader(
        string saleNumber,
        DateTime saleDate,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName)
    {
        if (string.IsNullOrWhiteSpace(saleNumber))
            throw new SaleDomainException("SaleNumber is required.");

        if (saleDate == default)
            throw new SaleDomainException("SaleDate is required.");

        if (customerId == Guid.Empty)
            throw new SaleDomainException("CustomerId is required.");

        if (string.IsNullOrWhiteSpace(customerName))
            throw new SaleDomainException("CustomerName is required.");

        if (branchId == Guid.Empty)
            throw new SaleDomainException("BranchId is required.");

        if (string.IsNullOrWhiteSpace(branchName))
            throw new SaleDomainException("BranchName is required.");

        SaleNumber = saleNumber.Trim();
        SaleDate = saleDate;
        CustomerId = customerId;
        CustomerName = customerName.Trim();
        BranchId = branchId;
        BranchName = branchName.Trim();
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Where(item => !item.IsCancelled).Sum(item => item.TotalAmount);
    }

    private void EnsureActive()
    {
        if (Status == SaleStatus.Cancelled)
            throw new SaleDomainException("Cancelled sales cannot be changed.");
    }
}

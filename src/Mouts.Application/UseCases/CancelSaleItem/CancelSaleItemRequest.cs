namespace Mouts.Application.UseCases.CancelSaleItem;

public class CancelSaleItemRequest
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
}

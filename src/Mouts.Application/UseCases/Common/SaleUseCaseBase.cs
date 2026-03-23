using Mouts.Application.Common;
using Mouts.Domain.Entities;
using Mouts.Domain.Exceptions;

namespace Mouts.Application.UseCases.Common;

public abstract class SaleUseCaseBase
{
    protected static List<SaleItem> BuildSaleItems(IEnumerable<SaleItemInput> items)
    {
        return items.Select(item => new SaleItem(
            item.ProductId,
            item.ProductName,
            item.Quantity,
            item.UnitPrice)).ToList();
    }

    protected static TResponse DomainError<TResponse>(string message, BusinessError businessError = BusinessError.ValidationFailed)
        where TResponse : ResultResponse, new()
    {
        var response = new TResponse
        {
            BusinessError = businessError
        };

        response.AddError(message);
        return response;
    }

    protected static TResponse DomainError<TResponse>(SaleDomainException exception, BusinessError businessError = BusinessError.ValidationFailed)
        where TResponse : ResultResponse, new()
    {
        return DomainError<TResponse>(exception.Message, businessError);
    }
}

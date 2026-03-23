using FluentValidation;

namespace Mouts.Application.UseCases.CancelSale;

public class CancelSaleRequestValidator : AbstractValidator<CancelSaleRequest>
{
    public CancelSaleRequestValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
    }
}

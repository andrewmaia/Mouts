using FluentValidation;

namespace Mouts.Application.UseCases.GetSaleById;

public class GetSaleByIdRequestValidator : AbstractValidator<GetSaleByIdRequest>
{
    public GetSaleByIdRequestValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
    }
}

using FluentValidation;

namespace Mouts.Application.UseCases.GetSales;

public class GetSalesRequestValidator : AbstractValidator<GetSalesRequest>
{
    public GetSalesRequestValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.Size).GreaterThan(0).LessThanOrEqualTo(100);
        RuleFor(x => x)
            .Must(x => !x.SaleDateMin.HasValue || !x.SaleDateMax.HasValue || x.SaleDateMin <= x.SaleDateMax)
            .WithMessage("SaleDateMin must be less than or equal to SaleDateMax.");
        RuleFor(x => x)
            .Must(x => !x.TotalAmountMin.HasValue || !x.TotalAmountMax.HasValue || x.TotalAmountMin <= x.TotalAmountMax)
            .WithMessage("TotalAmountMin must be less than or equal to TotalAmountMax.");
    }
}

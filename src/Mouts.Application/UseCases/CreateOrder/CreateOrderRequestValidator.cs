using FluentValidation;
using Mouts.Application.UseCases.CreateOrder;

public class CreateOrderRequestValidator
    : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.TotalAmount)
            .GreaterThan(0)
            .WithMessage("TotalAmount must be greater than zero");
    }
}

using FluentValidation;


namespace Mouts.Application.UseCases.PayOrder;
public class PayOrderRequestValidator : AbstractValidator<PayOrderRequest>
{
    public PayOrderRequestValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required");
    }
}

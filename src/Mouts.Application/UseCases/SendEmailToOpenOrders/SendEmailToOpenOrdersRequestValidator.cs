using FluentValidation;
using Mouts.Application.UseCases.SendEmailToOpenOrders;

public class SendEmailToOpenOrdersRequestValidator
    : AbstractValidator<SendEmailToOpenOrdersRequest>
{
    public SendEmailToOpenOrdersRequestValidator()
    {

    }
}

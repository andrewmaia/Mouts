namespace Mouts.Application.UseCases.SendEmailToOpenOrders;
public class SendEmailToOpenOrdersUseCase : IUseCase<SendEmailToOpenOrdersRequest, SendEmailToOpenOrdersResponse>
{
    public async Task<SendEmailToOpenOrdersResponse> ExecuteAsync(SendEmailToOpenOrdersRequest request)
    {
        return await Task.FromResult(new SendEmailToOpenOrdersResponse());
    }
}

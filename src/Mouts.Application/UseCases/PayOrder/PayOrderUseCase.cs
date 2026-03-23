using Mouts.Application.Common;
using Mouts.Application.Common.Events;
using Mouts.Application.Interfaces;
using Mouts.Application.Repositories;


namespace Mouts.Application.UseCases.PayOrder;
public class PayOrderUseCase : IUseCase<PayOrderRequest, PayOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DomainEventsDispatcher _domainEventsDispatcher;

    public PayOrderUseCase(IOrderRepository orderRepository, IUnitOfWork unitOfWork, DomainEventsDispatcher domainEventsDispatche)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _domainEventsDispatcher = domainEventsDispatche;
    }

    public async Task<PayOrderResponse> ExecuteAsync(PayOrderRequest request)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        var response = new PayOrderResponse();
        if (order is null)
        {
            response.BusinessError = BusinessError.NotFound;
            response.AddError($"Order with id '{request.OrderId}' was not found.");
            return response;
        }

        order.Pay();
        await _unitOfWork.CommitAsync();

        await _domainEventsDispatcher.DispatchAsync(order.DomainEvents);
        order.ClearDomainEvents();

        response.OrderId = order.Id;
        return response;
    }
}

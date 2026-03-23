using Mouts.Application.ExternalServices.PostalCode;
using Mouts.Application.Interfaces;
using Mouts.Application.Repositories;
using Mouts.Domain.Entities;
using Mouts.Domain.Enums;
using Mouts.Domain.Services;

namespace Mouts.Application.UseCases.CreateOrder;

public class CreateOrderUseCase : IUseCase<CreateOrderRequest, CreateOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly OrderDomainService _orderService;

    public CreateOrderUseCase(IOrderRepository orderRepository, IUnitOfWork unitOfWork, OrderDomainService orderService)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _orderService = orderService;
    }

    public async Task<CreateOrderResponse> ExecuteAsync(CreateOrderRequest request)
    {
        var finalAmount = _orderService.ApplyDiscount(request.TotalAmount);
        var order = new Order(OrderStatus.Open, finalAmount);

        _orderRepository.Add(order);
        await _unitOfWork.CommitAsync();

        return new CreateOrderResponse
        {
            OrderId = order.Id
        };
    }
}

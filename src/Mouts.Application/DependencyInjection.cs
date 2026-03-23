using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Mouts.Application.Common.Events;
using Mouts.Application.EventHandlers;
using Mouts.Application.Execution;
using Mouts.Application.UseCases.CreateOrder;
using Mouts.Application.UseCases.PayOrder;
using Mouts.Application.UseCases.ProcessOrderPaid;
using Mouts.Application.UseCases.SendEmailToOpenOrders;
using Mouts.Domain.DomainEvents;
using Mouts.Domain.Services;


namespace Mouts.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<CreateOrderRequest, CreateOrderResponse>,CreateOrderUseCase>();
        services.AddScoped<IUseCase<PayOrderRequest, PayOrderResponse>, PayOrderUseCase>();
        services.AddScoped<IUseCase<SendEmailToOpenOrdersRequest, SendEmailToOpenOrdersResponse>, SendEmailToOpenOrdersUseCase>();
        services.AddScoped<IUseCase<ProcessOrderPaidRequest, ProcessOrderPaidResponse>, ProcessOrderPaidUseCase>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient<DomainEventsDispatcher>();
        services.AddScoped<IDomainEventHandler<OrderPaidDomainEvent>, IssueInvoiceOnOrderPaidHandler>();
        services.AddScoped<RequestExecutor>();
        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<OrderDomainService>();
        return services;
    }
}

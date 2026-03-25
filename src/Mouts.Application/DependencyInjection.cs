using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Mouts.Application.Mapping;
using Mouts.Application.Common.Events;
using Mouts.Application.EventHandlers;
using Mouts.Application.Execution;
using Mouts.Application.UseCases.CancelSale;
using Mouts.Application.UseCases.CancelSaleItem;
using Mouts.Application.UseCases.CreateSale;
using Mouts.Application.UseCases.GetSaleById;
using Mouts.Application.UseCases.GetSales;
using Mouts.Application.UseCases.UpdateSale;
using Mouts.Domain.DomainEvents;

namespace Mouts.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(SaleMappingProfile).Assembly);
        services.AddScoped<IUseCase<CreateSaleRequest, CreateSaleResponse>, CreateSaleUseCase>();
        services.AddScoped<IUseCase<UpdateSaleRequest, UpdateSaleResponse>, UpdateSaleUseCase>();
        services.AddScoped<IUseCase<CancelSaleRequest, CancelSaleResponse>, CancelSaleUseCase>();
        services.AddScoped<IUseCase<CancelSaleItemRequest, CancelSaleItemResponse>, CancelSaleItemUseCase>();
        services.AddScoped<IUseCase<GetSaleByIdRequest, GetSaleByIdResponse>, GetSaleByIdUseCase>();
        services.AddScoped<IUseCase<GetSalesRequest, GetSalesResponse>, GetSalesUseCase>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient<DomainEventsDispatcher>();
        services.AddScoped<IDomainEventHandler<SaleCreatedDomainEvent>, SaleCreatedDomainEventHandler>();
        services.AddScoped<IDomainEventHandler<SaleModifiedDomainEvent>, SaleModifiedDomainEventHandler>();
        services.AddScoped<IDomainEventHandler<SaleCancelledDomainEvent>, SaleCancelledDomainEventHandler>();
        services.AddScoped<IDomainEventHandler<ItemCancelledDomainEvent>, ItemCancelledDomainEventHandler>();
        services.AddScoped<RequestExecutor>();
        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services;
    }
}

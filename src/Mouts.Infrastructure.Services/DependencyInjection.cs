using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mouts.Application.Interfaces;
using Mouts.Infrastructure.Services.Messaging;
using Mouts.Infrastructure.Services.Storage;

namespace Mouts.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //Message Bus
        services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();

        // File storage
        services.Configure<AzureBlobStorageOptions>(configuration.GetSection("AzureBlobStorage"));
        services.AddScoped<IFileStorage, AzureBlobStorage>();


        // Observability
        services.AddSingleton<IObservability, AppInsightsObservability>();

        return services;
    }
}
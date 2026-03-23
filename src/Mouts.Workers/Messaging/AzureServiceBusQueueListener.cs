using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mouts.Application.Messaging;
using Mouts.Workers.Messaging.Handlers;
using System.Text.Json;

namespace Mouts.Workers.Messaging;

public sealed class AzureServiceBusQueueListener : BackgroundService
{
    private readonly ServiceBusClient _client;
    private readonly string[] _queues;
    private readonly List<ServiceBusProcessor> _processors = new();
    private readonly ILogger<AzureServiceBusQueueListener> _logger;
    private readonly IServiceProvider _serviceProvider;



    public AzureServiceBusQueueListener(IConfiguration configuration,ILogger<AzureServiceBusQueueListener> logger, IServiceProvider serviceProvider)
    {
        var cs = configuration.GetConnectionString("AzureServiceBus")?? configuration["AzureServiceBus:ConnectionString"];

        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Azure Service Bus connection string is not configured.");

        _queues = configuration
            .GetSection("AzureServiceBus:Queues")
            .Get<string[]>()
            ?? throw new InvalidOperationException(
                "AzureServiceBus:Queues is not configured.");

        _client = new ServiceBusClient(cs);
        _logger = logger;
        _serviceProvider = serviceProvider;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var queue in _queues)
        {
            var processor = _client.CreateProcessor(queue, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false
            });

            processor.ProcessMessageAsync += OnMessage;
            processor.ProcessErrorAsync += OnError;

            await processor.StartProcessingAsync(stoppingToken);
            _processors.Add(processor);
        }

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task OnMessage(ProcessMessageEventArgs args)
    {
        try
        {
            switch (args.EntityPath)
            {
                case "orders":
                    {
                        var body = args.Message.Body.ToString();

                        var message = JsonSerializer.Deserialize<OrderPaidMessage>( body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                            ?? throw new InvalidOperationException("Invalid message.");


                        using var scope = _serviceProvider.CreateScope();
                        var handler = scope.ServiceProvider.GetRequiredService<OrderPaidMessageHandler>();
                        await handler.HandleAsync(message, args.CancellationToken);

                        await args.CompleteMessageAsync(args.Message);
                        return;
                    }

                default:
                    // não reconheceu a fila -> não completa (vai retry/DLQ conforme config)
                    return;
            }
        }
        catch
        {
            await args.AbandonMessageAsync(args.Message);
            throw;
        }
    }


    private Task OnError(ProcessErrorEventArgs args)
    {
        _logger.LogError(
            args.Exception,
            "ServiceBus error | Entity={entity} | Source={source}",
            args.EntityPath,
            args.ErrorSource);

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var processor in _processors)
            await processor.DisposeAsync();

        await _client.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
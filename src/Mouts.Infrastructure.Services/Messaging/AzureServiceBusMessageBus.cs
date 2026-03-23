using Microsoft.Extensions.Configuration;
using Mouts.Application.Interfaces;
using System.Text.Json;
using Azure.Messaging.ServiceBus;


namespace Mouts.Infrastructure.Services.Messaging;

public class AzureServiceBusMessageBus: IMessageBus, IAsyncDisposable
{
    private readonly ServiceBusClient _client;

    public AzureServiceBusMessageBus(IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("AzureServiceBus")?? configuration["AzureServiceBus:ConnectionString"];

        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Azure Service Bus connection string is not configured.");

        _client = new ServiceBusClient(cs);
    }

    public async Task SendAsync<T>(string queueName, T message, CancellationToken ct = default)
    {
        var sender = _client.CreateSender(queueName);

        var json = JsonSerializer.Serialize(message, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var sbMessage = new ServiceBusMessage(json)
        {
            ContentType = "application/json",
            MessageId = Guid.NewGuid().ToString("N")
        };

        try
        {
            await sender.SendMessageAsync(sbMessage, ct);
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }

    public ValueTask DisposeAsync() => _client.DisposeAsync();
}

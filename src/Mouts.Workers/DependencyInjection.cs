using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mouts.Workers.Jobs;
using Mouts.Workers.Messaging;
using Mouts.Workers.Messaging.Handlers;
using Quartz;

namespace Mouts.Workers;
public static class DependencyInjection
{
    public static IServiceCollection AddQuartz(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // -------- Quartz Jobs --------
        var jobConfig = configuration
            .GetSection(ExampleJobConfiguration.SectionName)
            .Get<ExampleJobConfiguration>();

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey(nameof(ExampleJob));
            q.AddJob<ExampleJob>(opts => opts.WithIdentity(jobKey))
             .AddTrigger(opts => opts
                 .ForJob(jobKey)
                 //.StartNow());
                 .WithCronSchedule(jobConfig!.CronExpression!));
        });

        services.AddQuartzHostedService(o => o.WaitForJobsToComplete = true);

        // -------- Azure Service Bus Listener --------
        //services.AddHostedService<AzureServiceBusQueueListener>();
        //services.AddScoped<OrderPaidMessageHandler>();


        return services;
    }
}

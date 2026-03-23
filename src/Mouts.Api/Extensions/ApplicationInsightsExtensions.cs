namespace Mouts.Api.Extensions;

public static class ApplicationInsightsExtensions
{
    public static IServiceCollection AddApplicationInsights(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString =  configuration["ApplicationInsights:ConnectionString"];
        });

        return services;
    }
}
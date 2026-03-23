using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mouts.Application.ExternalServices.PostalCode;
using Mouts.ExternalServices.ViaCEP.Configuration;

namespace Mouts.ExternalServices.ViaCEP;

public static class DependencyInjection
{
    public static IServiceCollection AddPostalCodeService(
        this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<ViaCepOptions>(configuration.GetSection("ViaCepService"));

        services.AddHttpClient<IPostalCodeService, PostalCodeService>((sp,client) =>
        {
            var options = sp.GetRequiredService<IOptions<ViaCepOptions>>().Value;

            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        });

        return services;
    }
}

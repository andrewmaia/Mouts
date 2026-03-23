using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mouts.Application.Interfaces;
using Mouts.Application.Repositories;
using Mouts.Domain.Enums;
using Mouts.Infrastructure.PostgreSQL.Context;
using Mouts.Infrastructure.PostgreSQL.Repositories;

namespace Mouts.Infrastructure.PostgreSQL;
public static class DependencyInjection
{
    public static IServiceCollection AddPostgreSQL(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MoutsDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql =>
                {
                    npgsql.MapEnum<SaleStatus>("sale_status");
                }
            )
        );

        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

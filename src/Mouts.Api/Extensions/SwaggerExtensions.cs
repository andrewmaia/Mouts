using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Mouts.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Mouts API",
                Version = "v1",
                Description = "Documentação automática de Mouts API"
            });

            // pega o caminho do arquivo XML
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            // inclui os comentários XML
            c.IncludeXmlComments(xmlPath);

            // opcional: habilitar atributos Swagger
            c.EnableAnnotations();
        });

        return services;
    }
    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        if (app.Environment().IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mouts API v1");
                c.RoutePrefix = string.Empty; 
            });
        }

        return app;
    }

    // Helper para pegar o IHostEnvironment no IApplicationBuilder
    private static IHostEnvironment Environment(this IApplicationBuilder app)
    {
        return app.ApplicationServices.GetRequiredService<Microsoft.Extensions.Hosting.IHostEnvironment>();
    }
}


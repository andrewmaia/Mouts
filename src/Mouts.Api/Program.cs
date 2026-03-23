using Mouts.Api.Extensions;
using Mouts.Application;
using Mouts.ExternalServices.ViaCEP;
using Mouts.Infrastructure.PostgreSQL;
using Mouts.Infrastructure.Services;
using Mouts.Workers;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureKeyVault();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddPostgreSQL(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddDomainServices();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddQuartz(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddPostalCodeService(builder.Configuration);
builder.Services.AddApplicationInsights(builder.Configuration);
builder.Logging.AddApplicationInsights();

var app = builder.Build();


app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

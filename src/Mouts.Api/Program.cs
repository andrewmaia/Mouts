using Mouts.Api.Extensions;
using Mouts.Api.Mapping;
using Mouts.Application;
using Mouts.Infrastructure.PostgreSQL;
using Mouts.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureKeyVault();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(SalesApiMappingProfile).Assembly);
builder.Services.AddPostgreSQL(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddDomainServices();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddApplicationInsights(builder.Configuration);
builder.Logging.AddApplicationInsights();

var app = builder.Build();

app.UseSwaggerDocumentation();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

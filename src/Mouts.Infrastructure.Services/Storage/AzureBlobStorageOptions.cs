namespace Mouts.Infrastructure.Services.Storage;

public sealed class AzureBlobStorageOptions
{
    public string ConnectionString { get; init; } = default!;
    public string ContainerName { get; init; } = default!;
    public bool CreateContainerIfNotExists { get; init; } = true;
    public string? PublicBaseUrl { get; init; } 
}
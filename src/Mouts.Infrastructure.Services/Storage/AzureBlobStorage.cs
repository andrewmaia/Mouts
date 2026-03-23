using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Mouts.Application.Interfaces;
using Microsoft.Extensions.Options;


namespace Mouts.Infrastructure.Services.Storage;

public  class AzureBlobStorage : IFileStorage
{
    private readonly BlobContainerClient _container;
    private readonly AzureBlobStorageOptions _options;

    public AzureBlobStorage(IOptions<AzureBlobStorageOptions> options)
    {
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.ConnectionString))
            throw new ArgumentException("AzureBlobStorageOptions.ConnectionString is required.");

        if (string.IsNullOrWhiteSpace(_options.ContainerName))
            throw new ArgumentException("AzureBlobStorageOptions.ContainerName is required.");

        _container = new BlobContainerClient(_options.ConnectionString, _options.ContainerName);
    }

    #region Private Helpers

    private async Task EnsureContainerAsync(CancellationToken ct)
    {
        if (_options.CreateContainerIfNotExists)
        {
            await _container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
        }
    }

    private BlobClient GetBlob(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("path is required.");

        // Evita path começando com "/" (Azure trata diferente e pode confundir)
        path = path.TrimStart('/');
        return _container.GetBlobClient(path);
    }

    private Uri? BuildUrl(string path, BlobClient blob)
    {
        if (!string.IsNullOrWhiteSpace(_options.PublicBaseUrl))
        {
            var baseUrl = _options.PublicBaseUrl.TrimEnd('/');
            return new Uri($"{baseUrl}/{_options.ContainerName}/{path.TrimStart('/')}");
        }

        return blob.Uri;
    }
    #endregion

    public async Task<FileStorageUploadResult> UploadAsync(FileStorageUploadRequest request, CancellationToken ct = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request.Content is null) throw new ArgumentNullException(nameof(request.Content));
        if (!request.Content.CanRead) throw new ArgumentException("Content stream must be readable.");

        await EnsureContainerAsync(ct);

        var blob = GetBlob(request.Path);

        var response = await blob.UploadAsync(request.Content, request.Overwrite, ct);

        var props = response.Value;

        return new FileStorageUploadResult(
            Path: request.Path.TrimStart('/'),
            Url: BuildUrl(request.Path, blob),
            ETag: props.ETag.ToString(),
            LastModified: props.LastModified);
    }

    public async Task<FileStorageDownloadResult?> DownloadAsync(string path, CancellationToken ct = default)
    {
        await EnsureContainerAsync(ct);

        var blob = GetBlob(path);

        try
        {
            var download = await blob.DownloadStreamingAsync(cancellationToken: ct);
            var d = download.Value;

            return new FileStorageDownloadResult(
                Path: path.TrimStart('/'),
                Content: d.Content,
                ContentType: d.Details.ContentType,
                ContentLength: d.Details.ContentLength,
                Metadata: d.Details.Metadata,
                ETag: d.Details.ETag.ToString(),
                LastModified: d.Details.LastModified);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string path, CancellationToken ct = default)
    {
        await EnsureContainerAsync(ct);

        var blob = GetBlob(path);
        var resp = await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: ct);
        return resp.Value;
    }

    public async Task<bool> ExistsAsync(string path, CancellationToken ct = default)
    {
        await EnsureContainerAsync(ct);

        var blob = GetBlob(path);
        return await blob.ExistsAsync(ct);
    }


}

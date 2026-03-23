
namespace Mouts.Application.Interfaces;

public interface IFileStorage
{
    Task<FileStorageUploadResult> UploadAsync(
        FileStorageUploadRequest request,
        CancellationToken ct = default);

    Task<FileStorageDownloadResult?> DownloadAsync(
        string path,
        CancellationToken ct = default);

    Task<bool> DeleteAsync(
        string path,
        CancellationToken ct = default);

    Task<bool> ExistsAsync(
        string path,
        CancellationToken ct = default);

}

public sealed record FileStorageUploadRequest(
    string Path,
    Stream Content,
    bool Overwrite = false);

public sealed record FileStorageUploadResult(
    string Path,
    Uri? Url,
    string? ETag,
    DateTimeOffset? LastModified);

public sealed record FileStorageDownloadResult(
    string Path,
    Stream Content,
    string? ContentType,
    long? ContentLength,
    IDictionary<string, string>? Metadata,
    string? ETag,
    DateTimeOffset? LastModified);


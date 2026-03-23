namespace Mouts.Api.Contracts;

/// <summary>
/// Generic API response wrapper.
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Data returned when operation is successful.
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Errors returned when operation fails.
    /// </summary>
    public IEnumerable<string>? Errors { get; set; }
}
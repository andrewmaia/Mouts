namespace Mouts.Application.Common;

public class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; init; } = [];
    public int Page { get; init; }
    public int Size { get; init; }
    public int TotalCount { get; init; }
}

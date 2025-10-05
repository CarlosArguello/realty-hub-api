namespace Application.Common;

public class Paged<T> : PaginationBase
{
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
    public long Total { get; set; }

    public Paged() { }

    public Paged(IReadOnlyList<T> items, long total, int page, int pageSize)
    {
        Items = items;
        Total = total;
        Page = page;
        PageSize = pageSize;
    }
}
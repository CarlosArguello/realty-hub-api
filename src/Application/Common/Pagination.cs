namespace Application.Common.Pagination;

public static class Pagination
{
    public static (int Page, int PageSize, int Skip) Normalize(int page, int pageSize, long total)
    {
        var pageSafe = page < 1 ? PaginationDefaults.DefaultPage : page;
        var pageSizeSafe = pageSize < 1 ? PaginationDefaults.DefaultPageSize : Math.Min(pageSize, PaginationDefaults.MaxPageSize);
        var skip = (pageSafe - 1) * pageSizeSafe;

        if (skip >= total && total > 0)
        {
            pageSafe = (int)Math.Ceiling(total / (double)pageSizeSafe);
            skip = (pageSafe - 1) * pageSizeSafe;
        }
        return (pageSafe, pageSizeSafe, skip);
    }

    public static (int Page, int PageSize, int Skip) Normalize(this PaginationBase model, long total)
    => Normalize(model.Page, model.PageSize, total);
}
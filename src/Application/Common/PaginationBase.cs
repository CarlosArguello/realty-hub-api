using System.ComponentModel.DataAnnotations;

namespace Application.Common;

public abstract class PaginationBase
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = PaginationDefaults.DefaultPage;

    [Range(1, 200)]
    public int PageSize { get; set; } = PaginationDefaults.DefaultPageSize;

}
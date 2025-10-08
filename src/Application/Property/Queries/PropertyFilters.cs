using Application.Common;

namespace Application.Property.Queries;

public sealed class PropertyFilters : PaginationBase
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
using Application.Common;
using Application.Property.Dtos;

namespace Application.Property.Queries;
public interface IPropertyQueries
{
    Task<Paged<PropertySummaryDto>> GetAllAsync(PropertyFilters filters, CancellationToken cancellationToken);
}

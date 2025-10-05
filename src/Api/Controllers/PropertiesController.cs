
using Application.Property.Dtos;
using Application.Property.Queries;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{

    private readonly IPropertyQueries _queries;

    public PropertiesController(IPropertyQueries queries) => _queries = queries;

    [HttpGet()]
    public async Task<ActionResult<IReadOnlyList<PropertySummaryDto>>> getAllProperties([FromQuery] PropertyFilters? filters, CancellationToken ct)
    {
        filters ??= new PropertyFilters();
        var properties = await _queries.GetAllAsync(filters, ct);

        return Ok(properties);
    }

}
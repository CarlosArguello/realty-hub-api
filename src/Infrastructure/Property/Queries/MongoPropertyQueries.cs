using Application.Common;
using Application.Common.Pagination;
using Application.Property.Dtos;
using Application.Property.Queries;
using Infrastructure.Property.Documents;
using Infrastructure.Property.Lookups;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Property.Queries;


public sealed class MongoPropertyQueries(IMongoDatabase db) : IPropertyQueries
{

    private readonly IMongoCollection<PropertyDocument> _properties = db.GetCollection<PropertyDocument>(PropertyDocument.CollectionName);
    private readonly IMongoCollection<PropertyImageDocument> _propertyImgs = db.GetCollection<PropertyImageDocument>(PropertyImageDocument.CollectionName);
    private readonly IMongoCollection<OwnerDocument> _owners = db.GetCollection<OwnerDocument>(OwnerDocument.CollectionName);

    public async Task<Paged<PropertySummaryDto>> GetAllAsync(PropertyFilters propertyFilters, CancellationToken ct)
    {
        var fb = Builders<PropertyDocument>.Filter;
        var filter = fb.Empty;

        if (!string.IsNullOrWhiteSpace(propertyFilters.Name))
            filter &= fb.Regex(p => p.Name, new BsonRegularExpression(propertyFilters.Name, "i"));

        if (!string.IsNullOrWhiteSpace(propertyFilters.Address))
            filter &= fb.Regex(p => p.Address, new BsonRegularExpression(propertyFilters.Address, "i"));

        if (propertyFilters.MinPrice is not null)
            filter &= fb.Gte(p => p.Price, propertyFilters.MinPrice);

        if (propertyFilters.MaxPrice is not null)
            filter &= fb.Lte(p => p.Price, propertyFilters.MaxPrice);


        var total = await _properties.CountDocumentsAsync(filter, cancellationToken: ct);

        var (page, pageSize, skip) = Pagination.Normalize(propertyFilters, total);

        var items = await _properties.Aggregate()
            .Match(filter)
            .SortByDescending(p => p.Year)
            .Skip(skip)
            .Limit(pageSize)
            .Lookup<PropertyDocument, PropertyImageDocument, PropertyImagesJoin>(
                _propertyImgs,
                localField: p => p.Id,
                foreignField: i => i.PropertyId,
                @as: pwi => pwi.propertyImages
            )
            .Project(p => new PropertySummaryDto(
                    p.CodeInternal,
                    p.Name,
                    p.Address,
                    p.Price,
                    p.Year,
                    p.propertyImages
                        .Where(i => i.Enabled)
                        .Select(i => i.File ?? string.Empty)
                        .FirstOrDefault()
            )).ToListAsync(ct);

        return new Paged<PropertySummaryDto>(items, total, page, pageSize);

    }



    public async Task<PropertyFullDetailsDto> GetByCodeAsync(string code, CancellationToken ct)
    {
        var property = await _properties.Aggregate()
            .Match(p => p.CodeInternal == code)
            .Lookup<PropertyDocument, OwnerDocument, PropertyOwnersJoin>(
                _owners,
                localField: p => p.OwnerId,
                foreignField: o => o.Id,
                @as: pwo => pwo.Owners
            )
            .Lookup<PropertyOwnersJoin, PropertyImageDocument, PropertyFullJoin>(
                _propertyImgs,
                localField: p => p.Id,
                foreignField: i => i.PropertyId,
                @as: pwi => pwi.propertyImages
            )
            .Project(p => new PropertyFullDetailsDto(
                    p.CodeInternal,
                    p.Name,
                    p.Address,
                    p.Price,
                    p.Year,
                    new OwnerDto(
                        p.OwnerId.ToString(),
                        p.Owners.Select(o => o.Name).FirstOrDefault() ?? string.Empty,
                        p.Owners.Select(o => o.Address).FirstOrDefault(),
                        p.Owners.Select(o => o.Photo).FirstOrDefault(),
                        p.Owners.Select(o => o.Birthday).FirstOrDefault()
                    ),
                    new PropertyImageDto(
                        p.propertyImages.Select(i => i.File).FirstOrDefault() ?? string.Empty,
                        p.propertyImages.Select(i => (bool?)i.Enabled).FirstOrDefault() ?? false
                    )
                )).FirstOrDefaultAsync(ct);

        return property;

    }

}

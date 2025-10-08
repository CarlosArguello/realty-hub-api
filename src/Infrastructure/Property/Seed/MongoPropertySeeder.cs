using Application.Seed;
using Infrastructure.Property.Documents;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Infrastructure.Property.Seed;

public sealed class MongoPropertySeeder(IMongoDatabase db) : IDataSeeder
{

    public async Task RunAsync(CancellationToken ct = default)
    {
        var basePath = AppContext.BaseDirectory;
        var path = Path.Combine(basePath, "Property", "Seed", "Data");

        var owners = db.GetCollection<OwnerDocument>(OwnerDocument.CollectionName);
        var properties = db.GetCollection<PropertyDocument>(PropertyDocument.CollectionName);
        var propertyImages = db.GetCollection<PropertyImageDocument>(PropertyImageDocument.CollectionName);


        var uxCodeProperties = new CreateIndexModel<PropertyDocument>(
            Builders<PropertyDocument>.IndexKeys.Ascending(p => p.CodeInternal),
            new CreateIndexOptions { Name = "ux_properties_codeInternal", Unique = true }
        );

        var ixYearProperties = new CreateIndexModel<PropertyDocument>(
            Builders<PropertyDocument>.IndexKeys.Descending(p => p.Year),
            new CreateIndexOptions { Name = "ix_properties_year_desc" }
        );


        var ixPriceProperties = new CreateIndexModel<PropertyDocument>(
            Builders<PropertyDocument>.IndexKeys.Ascending(p => p.Price),
            new CreateIndexOptions { Name = "ix_properties_price" }
        );

        await properties.Indexes.CreateManyAsync([
            uxCodeProperties,
            ixYearProperties,
            ixPriceProperties,
        ], cancellationToken: ct);


        var uxFileImages = new CreateIndexModel<PropertyImageDocument>(
            Builders<PropertyImageDocument>.IndexKeys.Ascending(i => i.PropertyId).Ascending(i => i.File),
            new CreateIndexOptions { Name = "ux_img_propertyId_file", Unique = true }
        );

        var ixFileEnabledImages = new CreateIndexModel<PropertyImageDocument>(
            Builders<PropertyImageDocument>.IndexKeys.Ascending(i => i.PropertyId).Ascending(i => i.Enabled),
            new CreateIndexOptions { Name = "ix_img_propertyId_enabled" }
        );

        await propertyImages.Indexes.CreateManyAsync([
            uxFileImages,
            ixFileEnabledImages,
        ], cancellationToken: ct);


        var hasOwners = await owners.Find(FilterDefinition<OwnerDocument>.Empty).Limit(1).AnyAsync(ct);
        if (!hasOwners)
        {
            var ownerJson = await File.ReadAllTextAsync(Path.Combine(path, "owner.json"), ct);
            var ownerList = BsonSerializer.Deserialize<List<OwnerDocument>>(ownerJson);
            await owners.InsertManyAsync(ownerList, new InsertManyOptions { IsOrdered = false }, ct);

        }

        var hasProperties = await properties.Find(FilterDefinition<PropertyDocument>.Empty).Limit(1).AnyAsync(ct);
        if (!hasProperties)
        {
            var propertiesJson = await File.ReadAllTextAsync(Path.Combine(path, "property.json"), ct);
            var propertiesList = BsonSerializer.Deserialize<List<PropertyDocument>>(propertiesJson);
            await properties.InsertManyAsync(propertiesList, new InsertManyOptions { IsOrdered = false }, ct);
        }

        var hasPropertyImages = await propertyImages.Find(FilterDefinition<PropertyImageDocument>.Empty).Limit(1).AnyAsync(ct);
        if (!hasPropertyImages)
        {
            var propertyImagesJson = await File.ReadAllTextAsync(Path.Combine(path, "propertyImage.json"), ct);
            var propertyImagesList = BsonSerializer.Deserialize<List<PropertyImageDocument>>(propertyImagesJson);
            await propertyImages.InsertManyAsync(propertyImagesList, new InsertManyOptions { IsOrdered = false }, ct);
        }


    }


}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Property.Documents;

[BsonIgnoreExtraElements]
public class PropertyDocument
{
    public const string CollectionName = "property";

    [BsonId]
    public ObjectId Id { get; set; }

    public ObjectId OwnerId { get; set; }

    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }

}

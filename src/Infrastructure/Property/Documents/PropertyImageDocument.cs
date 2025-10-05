using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Property.Documents;

[BsonIgnoreExtraElements]
public class PropertyImageDocument
{
    public const string CollectionName = "propertyImage";

    [BsonId]
    public ObjectId Id { get; set; }

    public ObjectId PropertyId { get; set; }

    public string File { get; set; } = default!;
    public bool Enabled { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

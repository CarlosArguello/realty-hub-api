using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Property.Documents;

[BsonIgnoreExtraElements]
public class OwnerDocument
{
    public const string CollectionName = "owner";

    [BsonId]
    public ObjectId Id { get; set; }

    public string Name { get; set; } = default!;
    public string Address { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public DateTime? Birthday { get; set; }
}

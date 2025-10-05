namespace Infrastructure.Property.Lookups;

using System.Collections.Generic;
using Infrastructure.Property.Documents;

internal class PropertyImagesJoin: PropertyDocument {
}

internal class PropertyOwnersJoin : PropertyDocument
{
    public required IReadOnlyList<OwnerDocument> Owners { get; init; }
}

internal sealed class PropertyFullJoin: PropertyDocument {
    public required IReadOnlyList<PropertyImageDocument> propertyImages { get; init; }
    public required IReadOnlyList<OwnerDocument> Owners { get; init; }
}

using MongoDB.Bson.Serialization.Conventions;

namespace Infrastructure.Config;

public static class MongoConventions
{
    private static bool _registered;

    public static void RegisterOnce()
    {
        if (_registered) return;

        var pack = new ConventionPack {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true)
        };

        ConventionRegistry.Register("camel+ignore", pack, _ => true);

        _registered = true;
    }

}
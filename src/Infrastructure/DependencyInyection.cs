using Application.Property.Queries;
using Application.Seed;
using Infrastructure.Property.Queries;
using Infrastructure.Property.Seed;
using Infrastructure.Config;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        MongoConventions.RegisterOnce();


        var dbName = configuration["Mongo:Database"];
        var connection = configuration.GetConnectionString("Mongo")
                   ?? throw new InvalidOperationException("ConnectionStrings:Mongo no configurado.");



        services.AddSingleton<IMongoClient>(_ =>
        {
            var settings = MongoClientSettings.FromConnectionString(connection);
            return new MongoClient(settings);
        });

        services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(dbName);
        });

        services.AddScoped<IPropertyQueries, MongoPropertyQueries>();
        services.AddSingleton<IDataSeeder, MongoPropertySeeder>();

        return services;
    }
}

public interface IHostEnvironment
{
}
using Application.Property.Queries;
using Infrastructure.Config;
using Infrastructure.Property.Queries;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>() ?? [""];

builder.Services.AddCors(o => o.AddDefaultPolicy(p => p
    .WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod()
));

builder.Services.AddControllers();

MongoConventions.RegisterOnce();
// builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(builder.Configuration.GetConnectionString("Mongo")));
builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var settings = MongoClientSettings.FromConnectionString(
        builder.Configuration.GetConnectionString("Mongo"));

    settings.ClusterConfigurator = cb =>
    {
        cb.Subscribe<CommandStartedEvent>(e =>
        {
            Console.WriteLine($"[Mongo -> {e.CommandName}]");
            Console.WriteLine(e.Command.ToJson(new JsonWriterSettings { Indent = true }));
        });
    };

    return new MongoClient(settings);
});

var dbName = builder.Configuration["Mongo:Database"];

builder.Services.AddSingleton(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase(dbName);
});



builder.Services.AddScoped<IPropertyQueries, MongoPropertyQueries>();

var app = builder.Build();
app.UseCors();
app.MapControllers();
app.Run();

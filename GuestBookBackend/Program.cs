using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var mongoUrl = Environment.GetEnvironmentVariable("MONGO_URL") ?? "mongodb://localhost:27017";
var client = new MongoClient(mongoUrl);
var database = client.GetDatabase("GuestbookDB");
var collection = database.GetCollection<MessageItem>("Messages");


app.MapGet("/api/messages", async () =>
{
    var messages = await collection.Find(_ => true).ToListAsync();
    return Results.Ok(messages);
});


app.MapPost("/api/messages", async (MessageItem newMessage) =>
{
    await collection.InsertOneAsync(newMessage);
    return Results.Ok(newMessage);
});

app.Run();


public class MessageItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Text { get; set; } = string.Empty;
}
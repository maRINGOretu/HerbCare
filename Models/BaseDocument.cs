using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace hcAPI.Models;

public abstract class BaseDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
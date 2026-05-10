using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public abstract class BaseDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty; // ← без GenerateNewId()

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
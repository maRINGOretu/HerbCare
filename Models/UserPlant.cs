using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace hcAPI.Models;

public class UserPlant : Plant
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserPlantId { get; set; } = ObjectId.GenerateNewId().ToString();

    public int Quantity { get; set; } = 1;
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastWatered { get; set; }
    public string? CustomName { get; set; }
    public string? CustomDescription { get; set; }
}
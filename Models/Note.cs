using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace hcAPI.Models;

public class Note : BaseDocument
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string? UserPlantId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Icon { get; set; } = "📝";

    public string NoteColor { get; set; } = "#D6E8D0";

    public string? LinkedPlantName { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
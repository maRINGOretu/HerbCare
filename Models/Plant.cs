using MongoDB.Bson.Serialization.Attributes;

namespace hcAPI.Models;

public class Plant : BaseDocument
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string MedicinalProperties { get; set; } = string.Empty;
    public int WateringDays { get; set; }

    public string FertilizerType { get; set; } = string.Empty;
    public double FertilizerDosePer1L { get; set; } = 1.0;
    public int FertilizerIntervalDays { get; set; } = 14;

    public string PesticideType { get; set; } = string.Empty;
    public double PesticideDosePer1L { get; set; } = 2.0;
    public int PesticideIntervalDays { get; set; } = 30;

    [BsonElement("ImageUrl")]
    public string? ImageUrl { get; set; }

    public string Category { get; set; } = string.Empty;
}
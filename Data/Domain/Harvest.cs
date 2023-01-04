namespace KingsFarms.Core.Api.Data.Domain;

public class Harvest
{
    public int Id { get; set; }
    public DateTime? HarvestDate { get; set; }
    public int? Quantity { get; set; }
    public Bed? Bed { get; set; }
}
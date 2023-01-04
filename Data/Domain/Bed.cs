namespace KingsFarms.Core.Api.Data.Domain;

public class Bed
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Section { get; set; }

    public List<Harvest>? Harvests { get; set; }
}
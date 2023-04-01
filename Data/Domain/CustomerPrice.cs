namespace KingsFarms.Core.Api.Data.Domain;

public class CustomerPrice : DomainObject
{
    public decimal Price { get; set; }
    public decimal? ShipCost { get; set; }
    public int? Size { get; set; }


    public virtual Customer Customer { get; set; }
    public int CustomerId { get; set; }
}
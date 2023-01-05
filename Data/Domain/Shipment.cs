using System.ComponentModel.DataAnnotations;
using KingsFarms.Core.Api.Data.Domain;
using Microsoft.EntityFrameworkCore;

[Index(nameof(ShipmentDate))]
public class Shipment : DomainObject
{
    [Required] public DateTime? ShipmentDate { get; set; }
    [Required] public int? Quantity { get; set; }
    [Required] public int? Boxes { get; set; }
    public ShipmentTypeEnum? ShipmentType { get; set; }
    public Customer? Customer { get; set; }

}
using KingsFarms.Core.Api.Data.Domain;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KingsFarms.Core.Api.Data.Domain
{
    [Index(nameof(Key), IsUnique = true)]
    public class Customer : DomainObject
    {
        public int Id { get; set; }
        [Required][StringLength(20)] public string? Key { get; set; }

        [StringLength(50)] public string? Name { get; set; }

        [StringLength(250)] public string? Address { get; set; }

        [StringLength(100)] public string? City { get; set; }

        public int Zip { get; set; }

        [StringLength(250)] public string? StoreName { get; set; }

        public string? StorePhone { get; set; }
        public string? OtherPhone { get; set; }

        [StringLength(100)] public string? ContactName { get; set; }

        public string? ContactPhone { get; set; }

    }
}

public enum ShipmentTypeEnum
{
    None,
    FedExGround,
    FedEx2Day,
    DeltaCargo,
    SouthWestCargo,
    Usps
}
[Index(nameof(ShipmentDate))]
public class Shipment : DomainObject
{
    [Required] public DateTime? ShipmentDate { get; set; }
    [Required] public int? Quantity { get; set; }
    [Required] public int? Boxes { get; set; }
    public ShipmentTypeEnum? ShipmentType { get; set; }
    public Customer? Customer { get; set; }

}
[Index(nameof(Name))]
public class HorseFarm : DomainObject
{

    [StringLength(50)] public string? Name { get; set; }

    [StringLength(250)] public string? Address { get; set; }

    [StringLength(100)] public string? City { get; set; }

    public int? Zip { get; set; }

    public string? Phone { get; set; }

    [StringLength(100)] public string? ContactName { get; set; }
}


[Index(nameof(LoadDate))]
public class HorseFarmLoad : DomainObject
{
    [Required] public DateTime LoadDate { get; set; }

    [Required] public int Qty { get; set; }

    [Required] public HorseFarm? HorseFarm { get; set; }
}
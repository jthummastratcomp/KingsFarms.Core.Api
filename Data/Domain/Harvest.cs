using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KingsFarms.Core.Api.Data.Domain;

[Index(nameof(HarvestDate))]
public class Harvest : DomainObject
{
    [Required] public DateTime? HarvestDate { get; set; }

    [Required] public int? Quantity { get; set; }

    //[Required] public Bed? Bed { get; set; }
    [Required] public int BedId { get; set; }
}
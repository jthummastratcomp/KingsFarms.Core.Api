using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KingsFarms.Core.Api.Data.Domain;

[Index(nameof(HarvestDate))]
public class Harvest : DomainObject
{

    [Required] public DateTime? HarvestDate { get; set; }
    [Required] public int? Quantity { get; set; }
    [Required] public Bed? Bed { get; set; }
}
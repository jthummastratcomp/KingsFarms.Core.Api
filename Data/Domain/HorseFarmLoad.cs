using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KingsFarms.Core.Api.Data.Domain;

[Index(nameof(LoadDate))]
public class HorseFarmLoad : DomainObject
{
    [Required] public DateTime LoadDate { get; set; }

    [Required] public int Qty { get; set; }

    [Required] public HorseFarm? HorseFarm { get; set; }
}
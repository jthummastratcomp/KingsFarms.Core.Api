using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KingsFarms.Core.Api.Data.Domain;

//[Index(nameof(Number), IsUnique = true)]
public class Bed : DomainObject
{
    //[Required] 
    public int Number { get; set; }
    public string? Section { get; set; }
    public int PlantsCount { get; set; }
    public DateTime PlantedDate { get; set; }
    public List<Harvest> Harvests { get; }
}
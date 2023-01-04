using KingsFarms.Core.Api.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KingsFarms.Core.Api.Data.Domain;

[Index(nameof(Name), IsUnique = true)]
public class Bed :DomainObject
{

    [Required] [StringLength(50)] public string? Name { get; set; }
    [Required] public SectionEnum? Section { get; set; }
    public int PlantsCount { get; set; }
    public DateTime PlantedDate { get; set; }
}
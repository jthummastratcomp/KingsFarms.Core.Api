using System.ComponentModel.DataAnnotations;
using KingsFarms.Core.Api.Data.Domain;
using Microsoft.EntityFrameworkCore;

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
using System.ComponentModel.DataAnnotations;

namespace KingsFarms.Core.Api.Data.Domain;

//[Index(nameof(Key), IsUnique = true)]
public class Customer : DomainObject
{
    public Customer()
    {
        Invoices = new List<Invoice>();
    }

    [Required] [StringLength(20)] public string? Key { get; set; }

    [StringLength(50)] public string? Name { get; set; }

    [StringLength(250)] public string? Address { get; set; }

    [StringLength(100)] public string? City { get; set; }

    [StringLength(50)] public string? Zip { get; set; }

    [StringLength(250)] public string? StoreName { get; set; }

    public string? StorePhone { get; set; }
    public string? OtherPhone { get; set; }

    [StringLength(100)] public string? ContactName { get; set; }

    public string? ContactPhone { get; set; }

    public List<Invoice> Invoices { get; }
}
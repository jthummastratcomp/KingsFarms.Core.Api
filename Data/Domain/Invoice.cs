using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KingsFarms.Core.Api.Data.Domain;

//[Index(nameof(InvoiceNumber), IsUnique = true)]
public class Invoice : DomainObject
{
    [Required] [StringLength(50)] public string? InvoiceNumber { get; set; }

    public string? Description { get; set; }

    [Required] public DateTime? InvoiceDate { get; set; }

    [Required] [Precision(14, 2)] public decimal? Quantity { get; set; }

    [Required] [Precision(14, 2)] public decimal? Rate { get; set; }

    [Required] [Precision(14, 2)] public decimal? Amount { get; set; }

    [Precision(14, 2)] public decimal? ShippingCharge { get; set; }

    [Precision(14, 2)] public decimal? DiscountAmount { get; set; }

    public DateTime? DueDate { get; set; }
    public string? Memo { get; set; }

    [Required] public Customer? Customer { get; set; }
    [Required] public int? CustomerId { get; set; }
}
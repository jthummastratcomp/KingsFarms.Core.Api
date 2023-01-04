using System.ComponentModel.DataAnnotations;

namespace KingsFarms.Core.Api.Data.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string? Key { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? StoreName { get; set; }
        public string? StorePhone { get; set; }
        public string? OtherPhone { get; set; }
        public string? ContactName { get; set; }
        public string? ContactPhone { get; set; }

    }
}

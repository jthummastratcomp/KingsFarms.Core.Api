using HotTowel.Web.Controllers;

namespace HotTowel.Web.ViewModels
{
    public class CustomerHeaderViewModel
    {
        public string? CustomerKey { get; set; }
        public string? StoreName { get; set; }
        public AddressViewModel Address { get; set; }
        public ContactViewModel Contact { get; set; }
    }
}
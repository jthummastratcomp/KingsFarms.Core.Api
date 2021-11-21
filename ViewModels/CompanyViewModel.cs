namespace HotTowel.Web.ViewModels
{
    public class CompanyViewModel
    {
        public string CompanyName { get; set; }
        public string LegalName { get; set; }
        public CompanyEnum Company { get; set; }
        public AddressViewModel Address { get; set; }
    }
}
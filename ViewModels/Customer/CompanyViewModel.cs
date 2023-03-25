using KingsFarms.Core.Api.Enums;

namespace KingsFarms.Core.Api.ViewModels.Customer;

public class CompanyViewModel
{
    public string CompanyName { get; set; }
    public string LegalName { get; set; }
    public CompanyEnum Company { get; set; }
    public AddressViewModel Address { get; set; }
}
namespace KingsFarms.Core.Api.ViewModels.Customer;

public class CustomerHeaderViewModel
{
    public CustomerHeaderViewModel()
    {
        Address = new AddressViewModel();
        Contact = new ContactViewModel();
    }

    public string? CustomerKey { get; set; }
    public string? StoreName { get; set; }
    public AddressViewModel Address { get; set; }
    public ContactViewModel Contact { get; set; }
}
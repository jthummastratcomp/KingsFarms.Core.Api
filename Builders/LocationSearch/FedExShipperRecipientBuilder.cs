namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExShipperRecipientBuilder
{
    private FedExAddress _address;
    private FedExContact _contact;

    public FedExShipperRecipientBuilder WithAddress(FedExAddress address)
    {
        _address = address;
        return this;
    }

    public FedExShipperRecipientBuilder WithContact(FedExContact contact)
    {
        _contact = contact;
        return this;
    }

    public FedExShipperRecipient Build()
    {
        return new FedExShipperRecipient
        {
            Address = _address, Contact = _contact
        };
    }
}
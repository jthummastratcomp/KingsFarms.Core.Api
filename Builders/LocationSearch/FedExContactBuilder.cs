namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExContactBuilder
{
    private string _phone;
    private string _personName;

    public FedExContactBuilder WithPhone(string value)
    {
        _phone = value;
        return this;
    }
    public FedExContactBuilder WithPersonName(string value)
    {
        _personName = value;
        return this;
    }
    public FedExContact Build()
    {
        return new FedExContact { Phone = _phone, PersonName = _personName };
    }
}
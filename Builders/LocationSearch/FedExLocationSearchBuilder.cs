namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExLocationSearchBuilder
{
    private FedExLocation _fedExLocation;

    public FedExLocationSearchBuilder WithLocation()
    {
        _fedExLocation = new FedExLocation {Address = new FedExAddress()};
        return this;
    }

    public FedExLocationSearchBuilder WithLocation(FedExAddress fedExAddress)
    {
        _fedExLocation = new FedExLocation {Address = fedExAddress};
        return this;
    }

    public FedExLocationSearch Build()
    {
        return new FedExLocationSearch {Location = _fedExLocation};
    }
}
namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExAddressesToValidateBuilder
{
    private readonly List<FedExLocation> _fedExLocations = new();


    public FedExAddressesToValidateBuilder WithLocation(FedExAddress fedExAddress)
    {
        _fedExLocations.Add(new FedExLocation {Address = fedExAddress});
        return this;
    }

    public FedExValidateAddress Build()
    {
        return new FedExValidateAddress {Addresses = _fedExLocations};
    }
}
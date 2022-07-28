namespace HotTowel.Core.Api.Builders.LocationSearch;

public class SearchLocationAddressBuilder
{
    private string _countryCode;

    private string _postalCode;


    public SearchLocationAddressBuilder WithPostalCode(string value)
    {
        _postalCode = value;
        return this;
    }

    public SearchLocationAddressBuilder WithCountryCode(string value)
    {
        _countryCode = value;

        return this;
    }

    public FedExAddress Build()
    {
        return new FedExAddress { ZipCode = _postalCode, CountryCode = _countryCode };
    }

    
}
namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExAddressBuilder
{
    private readonly List<string?> _streetLines = new();
    private string? _city;
    private string _countryCode;
    private string? _postalCode;
    private string? _state;

    public FedExAddressBuilder WithStreetLine1(string? value)
    {
        _streetLines.Add(value);
        return this;
    }

    public FedExAddressBuilder WithStreetLine2(string? value)
    {
        _streetLines.Add(value);
        return this;
    }

    public FedExAddressBuilder WithCity(string? value)
    {
        _city = value;
        return this;
    }

    public FedExAddressBuilder WithState(string? value)
    {
        _state = value;
        return this;
    }

    public FedExAddressBuilder WithPostalCode(string? value)
    {
        _postalCode = value;
        return this;
    }

    public FedExAddressBuilder WithCountryCode(string value)
    {
        _countryCode = value;
        return this;
    }

    public FedExAddress Build()
    {
        return new FedExAddress
        {
            StreetLines = _streetLines,
            City = _city,
            State = _state,
            ZipCode = _postalCode,
            CountryCode = _countryCode
        };
    }
}
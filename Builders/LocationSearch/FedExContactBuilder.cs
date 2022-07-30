namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExContactBuilder
{
    private string? _companyName;
    private string? _personName;
    private string? _phone;

    public FedExContactBuilder WithPhone(string? value)
    {
        _phone = value;
        return this;
    }

    public FedExContactBuilder WithPersonName(string? value)
    {
        _personName = value;
        return this;
    }

    public FedExContactBuilder WithCompanyName(string? value)
    {
        _companyName = value;
        return this;
    }

    public FedExContact Build()
    {
        return new FedExContact { Phone = _phone,
            PersonName = _personName,
            CompanyName = _companyName };
    }
}
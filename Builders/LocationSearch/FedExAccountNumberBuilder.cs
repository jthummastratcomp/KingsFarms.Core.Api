namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExAccountNumberBuilder
{
    private string _number;


    public FedExAccountNumberBuilder WithNumber(string value)
    {
        _number = value;
        return this;
    }

    public FedExAccountNumber Build()
    {
        return new FedExAccountNumber { Number = _number };
    }
}
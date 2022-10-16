namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExWeightBuilder
{
    private string? _units;
    private double _value;

    public FedExWeightBuilder WithUnits(string? value)
    {
        _units = value;
        return this;
    }

    public FedExWeightBuilder WithValue(double value)
    {
        _value = value;
        return this;
    }

    public FedExWeight? Build()
    {
        return new FedExWeight
        {
            Units = _units,
            Value = _value
        };
    }
}
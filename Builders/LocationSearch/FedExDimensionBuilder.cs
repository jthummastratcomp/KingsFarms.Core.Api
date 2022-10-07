namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExDimensionBuilder
{
    private int _height;
    private int _length;
    private string _unit;
    private int _width;

    public FedExDimensionBuilder WithLength(int value)
    {
        _length = value;
        return this;
    }

    public FedExDimensionBuilder WithWidth(int value)
    {
        _width = value;
        return this;
    }

    public FedExDimensionBuilder WithHeight(int value)
    {
        _height = value;
        return this;
    }

    public FedExDimensionBuilder WithUnit(string value)
    {
        _unit = value;
        return this;
    }

    public FedExDimension Build()
    {
        return new FedExDimension
        {
            Height = _height,
            Width = _width,
            Length = _length,
            Units = _unit
        };
    }
}
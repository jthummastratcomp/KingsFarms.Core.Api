namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExPackageItemBuilder
{
    private FedExDimension? _dimension;
    private FedExWeight? _weight;

    public FedExPackageItemBuilder WithWeight(FedExWeight? value)
    {
        _weight = value;
        return this;
    }

    public FedExPackageItemBuilder WithDimension(FedExDimension? value)
    {
        _dimension = value;
        return this;
    }

    public FedExPackageItem Build()
    {
        return new FedExPackageItem {Weight = _weight};
    }
}
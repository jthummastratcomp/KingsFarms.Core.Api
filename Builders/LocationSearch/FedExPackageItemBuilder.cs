namespace HotTowel.Core.Api.Builders.LocationSearch;

public class FedExPackageItemBuilder
{
    private FedExWeight _weight;

    public FedExPackageItemBuilder WithWeight(FedExWeight value)
    {
        _weight = value;
        return this;
    }

    public FedExPackageItem Build()
    {
        return new FedExPackageItem { Weight = _weight };
    }
}
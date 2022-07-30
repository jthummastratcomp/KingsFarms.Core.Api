namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExLabelSpecificationBuilder
{
    private string? _imageType;
    private string? _stockType;

    public FedExLabelSpecificationBuilder WithStockType(string? value)
    {
        _stockType = value;
        return this;
    }

    public FedExLabelSpecificationBuilder WithImageType(string? value)
    {
        _imageType = value;
        return this;
    }

    public FedExLabelSpecification Build()
    {
        return new FedExLabelSpecification
        {
            ImageType = _imageType,
            StockType = _stockType
        };
    }
}
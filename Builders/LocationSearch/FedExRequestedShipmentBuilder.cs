namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExRequestedShipmentBuilder
{
    private readonly List<FedExPackageItem> _packageItems;
    private readonly List<FedExShipperRecipient> _recipients;
    private bool _blockInsightVisibility;
    private FedExLabelSpecification? _labelSpecification;
    private int _packageCount;
    private string? _packagingType;
    private FedExShippingPayment? _payment;
    private string? _pickupType;
    private string? _serviceType;
    private DateTime? _shipDate;
    private FedExShipperRecipient? _shipper;

    public FedExRequestedShipmentBuilder()
    {
        _packageItems = new List<FedExPackageItem>();
        _recipients = new List<FedExShipperRecipient>();
    }

    public FedExRequestedShipmentBuilder WithShipDate(DateTime? value)
    {
        _shipDate = value;
        return this;
    }

    public FedExRequestedShipmentBuilder WithShipper(FedExShipperRecipient shipper)
    {
        _shipper = shipper;
        return this;
    }

    public FedExRequestedShipmentBuilder WithRecipient(FedExShipperRecipient recipient)
    {
        _recipients.Add(recipient);
        return this;
    }

    public FedExRequestedShipmentBuilder WithPickupType(string value)
    {
        _pickupType = value;
        return this;
    }

    public FedExRequestedShipmentBuilder WithServiceType(string value)
    {
        _serviceType = value;
        return this;
    }

    public FedExRequestedShipmentBuilder WithPackagingType(string value)
    {
        _packagingType = value;
        return this;
    }

    public FedExRequestedShipmentBuilder WithShippingPayment(FedExShippingPayment payment)
    {
        _payment = payment;
        return this;
    }

    public FedExRequestedShipmentBuilder WithBlockInsightVisibility(bool value)
    {
        _blockInsightVisibility = value;
        return this;
    }

    public FedExRequestedShipmentBuilder WithLabelSpecification(FedExLabelSpecification labelSpecification)
    {
        _labelSpecification = labelSpecification;
        return this;
    }

    public FedExRequestedShipmentBuilder WithPackageCount(int? value)
    {
        _packageCount = value ?? 3;
        return this;
    }

    public FedExRequestedShipmentBuilder WithPackageItem(FedExPackageItem item)
    {
        for (int i = 0; i < _packageCount; i++)
        {
            _packageItems.Add(item);
        }
        return this;
    }

    public FedExRequestedShipment? Build()
    {
        return new FedExRequestedShipment
        {
            Shipper = _shipper,
            Recipients = _recipients,
            PackageItems = _packageItems,
            //ShipDate = _shipDate.HasValue? _shipDate.Value.ToString("d") : DateTime.Today.ToString("d"),
            ServiceType = _serviceType,
            PackagingType = _packagingType,
            PickupType = _pickupType,
            BlockInsightVisibility = _blockInsightVisibility,
            ShippingPayment = _payment,
            LabelSpec = _labelSpecification,
            PackageCount = _packageCount
        };
    }
}
namespace HotTowel.Core.Api.Builders.LocationSearch;

public class FedExRequestedShipmentBuilder
{
    private FedExShipperRecipient _shipper;
    private List<FedExShipperRecipient> _recepients = new List<FedExShipperRecipient>();
    private string _pickupType;
    private string _packagingType;
    private string _serviceType;
    private FedExShippingPayment _payment;
    private FedExLabelSpecification _labelSpecification;
    private List<FedExPackageItem> _packageItems = new List<FedExPackageItem>();

    public FedExRequestedShipmentBuilder WithShipper(FedExShipperRecipient shipper)
    {
        _shipper = shipper;
        return this;
    }

    public FedExRequestedShipmentBuilder WithRecipient(FedExShipperRecipient recipient)
    {
        _recepients.Add(recipient);
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

    public FedExRequestedShipmentBuilder WithLabelSpecification(FedExLabelSpecification labelSpecification)
    {
        _labelSpecification = labelSpecification;
        return this;
    }

    public FedExRequestedShipmentBuilder WithPackageItem(FedExPackageItem item)
    {
        _packageItems.Add(item);
        return this;
    }

    public FedExRequestedShipment Build()
    {
        return new FedExRequestedShipment
        {
            Shipper = _shipper,
            Recepients = _recepients,
            PickupType = _pickupType,
            PackagingType = _packagingType,
            ServiceType = _serviceType,
            ShippingPayment = _payment,
            LabelSpec = _labelSpecification,
            PackageItems = _packageItems
        };
    }
}
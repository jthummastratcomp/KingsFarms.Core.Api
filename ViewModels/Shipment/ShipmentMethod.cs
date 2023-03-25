namespace KingsFarms.Core.Api.ViewModels.Shipment;

public class ShipmentMethod
{
    public string TrackingNumber { get; set; }
    public string Carrier { get; set; } // = "FedEx";
    public string LabelUrl { get; set; }
}
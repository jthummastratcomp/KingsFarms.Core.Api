using KingsFarms.Core.Api.ViewModels.Invoice;

namespace KingsFarms.Core.Api.ViewModels.Shipment;

public class ShipmentsBillViewModel
{
    public ShipmentsBillViewModel()
    {
        Bill = new InvoiceBillViewModel();
        ShipMethod = new ShipmentMethod();
    }

    public int Year { get; set; }
    public int NumberOfShipments { get; set; }
    public decimal ShippedQuantity { get; set; }
    public int BoxCount { get; set; }
    public int HarvestedQuantity { get; set; }
    public InvoiceBillViewModel Bill { get; set; }
    public ShipmentMethod ShipMethod { get; set; }
}
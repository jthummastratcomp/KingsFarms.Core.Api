namespace KingsFarms.Core.Api.ViewModels
{
    public class ShipmentsBillViewModel
    {
        public int Year { get; set; }
        public int NumberOfShipments { get; set; }
        public decimal ShippedQuantity { get; set; }
        public int BoxCount { get; set; }
        public int HarvestedQuantity { get; set; }
        public InvoiceBillViewModel Bill { get; set; }
        public ShipmentMethod ShipMethod { get; set; }
    }
}
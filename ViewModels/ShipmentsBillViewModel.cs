namespace HotTowel.Web.ViewModels
{
    public class ShipmentsBillViewModel
    {
        public int Year { get; set; }
        public int NumberOfShipments { get; set; }
        public decimal ShippedQuantity { get; set; }
        public InvoiceBillViewModel Bill { get; set; }
    }
}
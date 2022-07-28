namespace HotTowel.Core.Api.ViewModels
{
    public class CustomerDashboardViewModel
    {
        public CustomerDashboardViewModel()
        {
            CustomerHeader = new CustomerHeaderViewModel();
            Price = new CustomerPriceViewModel();
            Bill = new InvoiceBillViewModel();
            Shipment = new ShipmentsBillViewModel();
        }
        public int Id { get; set; }
        public CustomerHeaderViewModel CustomerHeader { get; set; }
        public CustomerPriceViewModel Price { get; set; }
        public InvoiceBillViewModel Bill { get; set; }
        public ShipmentsBillViewModel Shipment { get; set; }
        public string LastPaymentDate { get; set; }

    }
}
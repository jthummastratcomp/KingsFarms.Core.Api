namespace KingsFarms.Core.Api.ViewModels
{
    public class CustomerInvoicesViewModel
    {
        public CustomerInvoicesViewModel()
        {
            CustomerHeader = new CustomerHeaderViewModel();
            Price = new CustomerPriceViewModel();
            Cost = new InvoiceCostViewModel();
            Bill = new InvoiceBillViewModel();
            Shipment = new ShipmentsBillViewModel();
        }

        public int Id { get; set; }
        //public string CustomerKey { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceDateDisplay => InvoiceDate.ToString("MM/dd/yyyy");
        public DateTime DueDate { get; set; }
        public string Status { get; set; }

        public CustomerHeaderViewModel CustomerHeader { get; set; }
        public CustomerPriceViewModel Price { get; set; }
        public InvoiceCostViewModel Cost { get; set; }
        public InvoiceBillViewModel Bill { get; set; }
        public ShipmentsBillViewModel Shipment { get; set; }

        public DateTime LastPaymentDate { get; set; }
        public int CustomerId { get; set; }
        public string Memo { get; set; }
    }
}
namespace KingsFarms.Core.Api.ViewModels
{
    public class InvoiceCostViewModel
    {
        public decimal Quantity { get; set; }
        public string QuantityDisplay => Quantity.ToString("##");
        public decimal Amount { get; set; }
        public string AmountDisplay => Amount.ToString("C0");
    }
}
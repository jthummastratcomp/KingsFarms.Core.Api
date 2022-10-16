namespace KingsFarms.Core.Api.ViewModels;

public class InvoiceBillViewModel
{
    public decimal Billed { get; set; }
    public string BilledDisplay => Billed.ToString("C0");
    public decimal Paid { get; set; }
    public string PaidDisplay => Paid.ToString("C0");
    public decimal Balance { get; set; }
    public string BalanceDisplay => Balance.ToString("C0");
    public decimal ShipmentCost { get; set; }
    public string ShipmentCostDisplay => ShipmentCost.ToString("C0");
    public decimal ChargesDiscounts { get; set; }
}
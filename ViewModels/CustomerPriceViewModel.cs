namespace HotTowel.Core.Api.ViewModels
{
    public class CustomerPriceViewModel
    {
        public decimal Rate { get; set; }
        public string RateDisplay => Rate.ToString("C0");
        public decimal BoxSize { get; set; }
        public string BoxSizeDisplay => BoxSize.ToString("##");
        public decimal ShipmentRate { get; set; }
        public string ShipmentRateDisplay => ShipmentRate.ToString("C0");
    }
}
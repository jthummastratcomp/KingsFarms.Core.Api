namespace KingsFarms.Core.Api.ViewModels;

public class PrepareInvoicesViewModel
{
    public PrepareInvoicesViewModel()
    {
        UsdaInfo = new UsdaInfoDto();
    }
    public string CustomerKey { get; set; }
    public int WeekQty { get; set; }
    public int BoxSize { get; set; }
    public DateTime Week { get; set; }
    public UsdaInfoDto UsdaInfo { get; set; }
}

//public class PrepareUsdaInvoicesViewModel
//{
//    public PrepareUsdaInvoicesViewModel()
//    {
//        UsdaInfo = new UsdaInfoDto();
//    }
//    public string CustomerKey { get; set; }

//    public int WeekQty { get; set; }

//    //public int BoxSize { get; set; }
//    public DateTime Week { get; set; }
//    public UsdaInfoDto UsdaInfo { get; set; }
//}

public class UsdaInfoDto
{
    public string HarvestDate { get; set; }
    public string Bed { get; set; }
    public string Lot { get; set; }
}
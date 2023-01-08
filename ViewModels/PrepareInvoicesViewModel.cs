namespace KingsFarms.Core.Api.ViewModels;

public class PrepareInvoicesViewModel
{
    public string CustomerKey { get; set; }
    public int WeekQty { get; set; }

    public DateTime Week { get; set; }
    //public UsdaInfoDto UsdaInfo { get; set; }
}
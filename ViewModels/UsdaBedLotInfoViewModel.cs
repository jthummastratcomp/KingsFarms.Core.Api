namespace KingsFarms.Core.Api.ViewModels;

public class UsdaBedLotInfoViewModel
{
    public UsdaBedLotInfoViewModel()
    {
        Week = DateTime.Today;
        LineItems = new List<UsdaBedLotInfo>();
    }

    public List<UsdaBedLotInfo> LineItems { get; set; }
    public DateTime Week { get; set; }
}

public class UsdaBedLotInfo
{
    public UsdaBedLotInfo()
    {
        HarvestDate = DateTime.Today;
    }

    public DateTime HarvestDate { get; set; }
    public string Bed { get; set; }
    public string Lots { get; set; }
    public string Customer { get; set; }
    public int Quantity { get; set; }
}
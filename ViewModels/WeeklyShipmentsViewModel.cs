namespace KingsFarms.Core.Api.ViewModels;

public class WeeklyShipmentsViewModel
{
    public int Year { get; set; }
    public List<int> ShippedQuantityPerWeek { get; set; }
}

public class WeeklyShipmentsGraphViewModel
{
    public List<WeeklyShipmentsViewModel> Series { get; set; }
    public List<string> Categories { get; set; }
}
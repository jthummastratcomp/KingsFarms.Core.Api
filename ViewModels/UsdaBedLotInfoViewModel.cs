namespace KingsFarms.Core.Api.ViewModels;

public class UsdaBedLotInfoViewModel
{
    public UsdaBedLotInfoViewModel()
    {
        HarvestDate = DateTime.Today;
        Lots = new List<int>();
    }
    public DateTime HarvestDate { get; set; }
    public int Bed { get; set; }
    public List<int> Lots { get; set; }
    public string Customer { get; set; }
}
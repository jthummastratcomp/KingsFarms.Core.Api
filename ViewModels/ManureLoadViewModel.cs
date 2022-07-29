namespace HotTowel.Core.Api.ViewModels;

public class ManureLoadViewModel
{
    public Dictionary<string, int> Loads { get; set; } = new();
    public DateTime ManureLoadDate { get; set; }
    public string ManureLoadDateDisplay => ManureLoadDate.ToString("MM/dd/yyyy");

    public void AddFarmLoad(string farmName, int qty)
    {
        if(string.IsNullOrEmpty(farmName)) return;
        if (Loads.ContainsKey(farmName))
            Loads[farmName] = qty;
        else Loads.Add(farmName, qty);
    }
}
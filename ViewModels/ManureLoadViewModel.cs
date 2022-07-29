using System.Text;
using KingsFarms.Core.Api.Helpers;

namespace KingsFarms.Core.Api.ViewModels;

public class ManureLoadViewModel
{
    public Dictionary<string, int> Loads { get; set; } = new();
    public DateTime ManureLoadDate { get; set; }
    public string ManureLoadDateDisplay => ManureLoadDate.ToString("MM/dd/yyyy");

    public string ManureLoadDisplay
    {
        get
        {
            if (Loads.Count == 0) return string.Empty;

            if (!Utils.HasRows(Loads)) return string.Empty;

            var builder = new StringBuilder();
            foreach (var model in Loads) builder.AppendLine($"{model.Key}:{model.Value}");

            return builder.ToString();
        }
    }

    public void AddFarmLoad(string farmName, int qty)
    {
        if (string.IsNullOrEmpty(farmName)) return;
        if (Loads.ContainsKey(farmName))
            Loads[farmName] = qty;
        else Loads.Add(farmName, qty);
    }
}
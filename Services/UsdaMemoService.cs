using System.Text;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class UsdaMemoService : IUsdaMemoService
{
    public string? GetUsdaMemo(Queue<SearchDto>? queue, PrepareInvoicesViewModel viewModel)
    {
        if (queue == null) return string.Empty;

        var permit = "2022-064";
        var week = viewModel.Week.ToString("MMddyy");

        var boxes = viewModel.WeekQty / viewModel.BoxSize;

        var list = new List<SearchDto>();

        for (var i = 0; i < boxes; i++) list.Add(queue.Dequeue());

        var usda_week = $"{permit}-{week}";

        var lotsList = new Dictionary<string, string>();
        var usda_lots = new List<int>();

        foreach (var dto in list)
        {
            var bed = dto.Id;
            var block = GetBlock(bed);
            var lot = Utils.ParseToInteger(dto.Data);
            var other = dto.DataType;

            var usda_block = $"{usda_week}-{block}-{bed}-";


            if (!lotsList.ContainsKey(usda_block))
            {
                usda_lots = new List<int> { lot };
                lotsList.Add(usda_block, $"{LotsToString(usda_lots)} /{other}");
            }
            else
            {
                if (usda_lots.Contains(lot)) continue;

                usda_lots.Add(lot);
                lotsList[usda_block] = $"{LotsToString(usda_lots)} /{other}";
            }
        }

        return BuildMemo(lotsList);
    }

    private static string GetBlock(string bed)
    {
        var bedNumber = Utils.ParseToInteger(bed);
        return bedNumber switch
        {
            >= 1 and <= 11 => "74552",
            >= 12 and <= 22 => "74560",
            >= 23 and <= 28 => "74558",
            >= 29 and <= 37 => "74556",
            >= 38 and <= 51 => "74554",
            _ => string.Empty
        };
    }

    private static string LotsToString(ICollection<int> usdaLots)
    {
        if (!Utils.HasRows(usdaLots)) return "?";

        if (usdaLots.Count == 1) return usdaLots.First().ToString();

        var first = usdaLots.First();
        var last = usdaLots.Last();

        return $"{first}-{last}";
    }

    private static string? BuildMemo(Dictionary<string, string> lotsList)
    {
        if (!Utils.HasRows(lotsList)) return string.Empty;

        var sb = new StringBuilder();

        foreach (var lot in lotsList) sb.AppendLine($"{lot.Key}{lot.Value}");

        return sb.ToString().Replace(",", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace(" ", string.Empty).Trim();
    }
}
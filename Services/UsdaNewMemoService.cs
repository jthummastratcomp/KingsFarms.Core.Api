using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class UsdaNewMemoService : IUsdaMemoService
{
    public string GetUsdaMemo(Queue<SearchDto>? queue1, PrepareInvoicesViewModel viewModel)
    {
        if (string.IsNullOrEmpty(viewModel.UsdaInfo.HarvestDate)
            || string.IsNullOrEmpty(viewModel.UsdaInfo.Bed)
            || string.IsNullOrEmpty(viewModel.UsdaInfo.Lot)) return string.Empty;

        const string permit = "2022-064";
        var week = $"{viewModel.UsdaInfo.HarvestDate}{viewModel.Week.Year:D2}";
        var usdaBed = viewModel.UsdaInfo.Bed;
        var usdaBlock = GetBlock(usdaBed);
        var usdaLot = viewModel.UsdaInfo.Lot;

        return $"USDA #: {permit}-{week}-{usdaBlock}-{usdaBed}-{usdaLot}-FL";
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
}
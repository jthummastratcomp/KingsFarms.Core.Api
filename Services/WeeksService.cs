using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class WeeksService : IWeeksService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly IHarvestService _harvestService;
    private readonly string _weeklyOrdersFile;

    public WeeksService(string azStoreConnStr, string azStoreContName, string weeklyOrdersFile, IHarvestService harvestService)
    {
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _weeklyOrdersFile = weeklyOrdersFile;
        _harvestService = harvestService;
    }

    public List<SearchDto> GetInvoiceWeeksListForYear(int year)
    {
        return Utils.GetWeeksOfYear(year);
    }


    private static int GetWeekOfYearForInvoicesFromSheet(DateTime? weekDate)
    {
        var weekOfYear = Utils.GetWeekOfYear(weekDate.Value);
        var firstMondayOfYear = Utils.GetFirstMondayOfYear(DateTime.Today.Year);
        var firstWeekOfYear = Utils.GetWeekOfYear(firstMondayOfYear);
        if (firstWeekOfYear > 1) weekOfYear -= 1;
        return weekOfYear;
    }
}

public interface IWeeksService
{
}
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.ViewModels.Harvest;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IHarvestService
{
    List<HarvestViewModel> GetHarvestDataBySeason(int harvestYear);
    List<HarvestViewModel> GetHarvestDataByCalendar(int calendarYear);
    int GetHarvestYearTotalBySeason(int year);
    int GetHarvestYearTotalByCalendar(int calendarYear);
    int GetHarvestStatusTotal(DashboardStatusEnum status);
    HarvestDto GetHarvestDataCalendarAll();
    HarvestDto GetHarvestDataSeasonAll();
    IEnumerable<HarvestViewModel> GetAllHarvestData();
    List<SectionHarvestViewModel> GetHarvestByYearBySection();
    List<BedHarvestChartViewModel> GetHarvestByYearByBed();
}
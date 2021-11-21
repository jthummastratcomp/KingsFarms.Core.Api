using System.ComponentModel;

namespace HotTowel.Web.ViewModels
{
    public enum DashboardStatusEnum
    {
        [Description("All Years")] AllYears,
        [Description("Current Year")] CurrentYear,
        [Description("Last Year")] LastYear
    }
}
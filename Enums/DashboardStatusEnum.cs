using System.ComponentModel;

namespace HotTowel.Core.Api.Enums
{
    public enum DashboardStatusEnum
    {
        [Description("All Years")] AllYears,
        [Description("Current Year")] CurrentYear,
        [Description("Last Year")] LastYear
    }
}
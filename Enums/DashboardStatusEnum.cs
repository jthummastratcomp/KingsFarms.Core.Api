using System.ComponentModel;

namespace KingsFarms.Core.Api.Enums;

public enum DashboardStatusEnum
{
    [Description("All Years")] AllYears,
    [Description("Current Year")] CurrentYear,
    [Description("Last Year")] LastYear
}
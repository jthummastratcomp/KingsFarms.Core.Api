using System.Diagnostics;

namespace KingsFarms.Core.Api.ViewModels.Harvest;

[DebuggerDisplay("ThisYear: {ThisYear}, LastYear: {LastYear}, AllYears: {AllYears}")]
public class HarvestDto
{
    public int? ThisYear { get; set; }
    public int? LastYear { get; set; }
    public int? AllYears { get; set; }
}
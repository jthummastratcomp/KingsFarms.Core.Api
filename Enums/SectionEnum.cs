using System.ComponentModel;

namespace HotTowel.Web.ViewModels
{
    public enum SectionEnum
    {
        [Description("Mid West")] MidWest,
        [Description("South West")] SouthWest,
        [Description("South East 1")] SouthEastOne,
        [Description("South East 2")] SouthEastTwo,
        [Description("Mid East 1")] MidEastOne,
        [Description("Mid East 2")] MidEastTwo,
        None
    }
}
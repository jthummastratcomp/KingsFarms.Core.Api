namespace HotTowel.Core.Api.ViewModels
{
    public class SearchDto
    {
        public string Id { get; set; }
        public string Data { get; set; }
        //public string SortOrderIndex { get; set; }
        public string DataType { get; set; }
    }

    public class DashboardViewModel
    {
        public string Name { get; set; }
        public SearchDto Data1 { get; set; }
        public SearchDto Data2 { get; set; }
        public SearchDto Data3 { get; set; }
    }
}
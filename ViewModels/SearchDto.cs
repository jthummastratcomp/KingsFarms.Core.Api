using System.Diagnostics;

namespace KingsFarms.Core.Api.ViewModels
{
    [DebuggerDisplay("Id: {Id}, Data: {Data}, Type: {DataType}")]
    public class SearchDto
    {
        public SearchDto()
        {
            
        }
        public string? Id { get; set; }
        public string? Data { get; set; }
        //public string SortOrderIndex { get; set; }
        public string DataType { get; set; }
    }
}
using System.Diagnostics;

namespace KingsFarms.Core.Api.ViewModels;

[DebuggerDisplay("Id: {Id}, Data: {Data}, Type: {DataType}")]
public class SearchDto
{
    public string? Id { get; set; }

    public string? Data { get; set; }

    public List<string> Messages { get; set; }
    public string? DataType { get; set; }
}
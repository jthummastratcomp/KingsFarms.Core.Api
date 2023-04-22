using EPPlus.DataExtractor;
using KingsFarms.Core.Api.Domain;
using OfficeOpenXml;

namespace KingsFarms.Core.Api.Application.Common.Interfaces;

public class HarvestContext : IHarvestContext
{
    private readonly IAzureBlobService _azureBlobService;

    public HarvestContext(IAzureBlobService azureBlobService)
    {
        _azureBlobService = azureBlobService;
    }
    public List<Domain.Bed> GetBedsInfo()
    {

        var blob = _azureBlobService.GetHarvestBlob();
        

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using (var package = new ExcelPackage(memoryStream))
            {
                var beds = package.Workbook.Worksheets["2020_2021_HARVEST"]
                        .Extract<WeeklyHarvest>()
                        .WithProperty(p => p.WeekNumber, "A")
                        .WithProperty(p => p.Week, "B")
                        .WithProperty(p => p.Total, "D")
                        .WithCollectionProperty(p=>p.Beds,
                            item => item.BedNumber, 1,
                            item => item.Quantity, "F", "AG")
                        .GetData(13,16)
                        .ToList();

                var plantsByBed = package.Workbook.Worksheets["2020_2021_HARVEST"]
                    .Extract<PlantsCount>()
                    .WithProperty(p => p.WeekNumber, "A")
                    //.WithProperty(p => p.Week, "B")
                    .WithProperty(p => p.Total, "D")
                    .WithCollectionProperty(p => p.Beds,
                        item => item.BedNumber, 1,
                        item => item.Quantity, "F", "AG")
                    .GetData(2,2)
                    .ToList();

                var list =  beds;
            }
        }

        
        return new List<Domain.Bed>();
    }
}

public class WeeklyHarvest
{
    public string Week { get; set; }

    public string WeekNumber { get; set; }
    public int Total { get; set; }
    public List<Bed> Beds { get; set; }
}

public class PlantsCount
{
    //public string Week { get; set; }

    public string WeekNumber { get; set; }
    public int Total { get; set; }
    public List<Bed> Beds { get; set; }
}

public class Bed
{
    public int Quantity { get; set; }
    public string BedNumber { get; set; }
}

//public class ColumnDataPlantsCount
//{
//    public int Count { get; set; }
//    public string BedNumber { get; set; }
//}
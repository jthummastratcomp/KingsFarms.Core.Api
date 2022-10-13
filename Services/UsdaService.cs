using System.Drawing;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Services;

public class UsdaService : IUsdaService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly ILogger _logger;
    private readonly string _weeklyOrdersUsdaFile;

    public UsdaService(ILogger logger, string azStoreConnStr, string azStoreContName)
    {
        _logger = logger;
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
    }


    public string UpdateUsdaInfo(UsdaBedLotInfoViewModel viewModel)
    {
        var day = viewModel.Week.ToString("yyyyMMdd");
        var fileName = $"{day}-lot-assign.xlsx";

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);

        container.CreateIfNotExists(PublicAccessType.Blob);

        var blob = container.GetBlockBlobClient(fileName);

        using var package = new ExcelPackage();

        var lotTab = package.Workbook.Worksheets.Add("Lot");

        lotTab.Cells["A1"].Value = "Lot Assignments";
        lotTab.Cells["A1:E1"].Merge = true;
        lotTab.Cells["A1:E1"].Style.Font.Bold = true;
        lotTab.Cells["A1:E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        lotTab.Cells["A1:E1"].Style.Font.Size = 25;
        lotTab.Cells["A1:E1"].EntireRow.Height = 35;

        lotTab.Cells["A3"].Value = viewModel.Week.ToString("MM/dd/yyyy");
        lotTab.Cells["A3:E3"].Merge = true;
        lotTab.Cells["A3:E3"].Style.Font.Bold = true;
        lotTab.Cells["A3:E3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        lotTab.Cells["A3:E3"].Style.Font.Size = 16;
        lotTab.Cells["A3:E3"].EntireRow.Height = 25;

        lotTab.Cells["A5"].Value = "CUSTOMER";
        lotTab.Cells["A5"].EntireColumn.Width = 15;
        lotTab.Cells["B5"].Value = "HARVEST DT";
        lotTab.Cells["B5"].EntireColumn.Width = 15;
        lotTab.Cells["B5"].EntireColumn.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        lotTab.Cells["C5"].Value = "BED #";
        lotTab.Cells["C5"].EntireColumn.Width = 15;
        lotTab.Cells["C5"].EntireColumn.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        lotTab.Cells["D5"].Value = "LOT #";
        lotTab.Cells["D5"].EntireColumn.Width = 15;
        lotTab.Cells["D5"].EntireColumn.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        lotTab.Cells["E5"].Value = "QUANTITY";
        lotTab.Cells["E5"].EntireColumn.Width = 15;
        lotTab.Cells["E5"].EntireColumn.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        lotTab.Cells["A5:E5"].Style.Font.Bold = true;
        lotTab.Cells["A5:E5"].Style.Font.Size = 12;
        lotTab.Cells["A5:E5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        lotTab.Cells["A5:E5"].Style.Fill.BackgroundColor.SetColor(Color.Bisque);

        int row = 6;
        foreach (var lotInfo in viewModel.LineItems)
        {
            lotTab.Cells[$"A{row}"].Value = lotInfo.Customer;
            lotTab.Cells[$"B{row}"].Value = lotInfo.HarvestDate.ToString("MM/dd/yyyy");
            lotTab.Cells[$"C{row}"].Value = lotInfo.Bed;
            lotTab.Cells[$"D{row}"].Value = lotInfo.Lots;
            lotTab.Cells[$"E{row}"].Value = lotInfo.Quantity;
            row++;
        }
        

        using var stream = blob.OpenWrite(true);

        package.SaveAs(stream);


        return string.Empty;
    }
}
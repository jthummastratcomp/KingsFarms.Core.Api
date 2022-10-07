using System.Data;
using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Mappers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using OfficeOpenXml;

namespace KingsFarms.Core.Api.Services;

public class WeeklyOrdersService : IWeeklyOrdersService
{
    private readonly string _azStoreConnStr;
    private readonly string _azStoreContName;
    private readonly IHarvestService _harvestService;
    private readonly string _weeklyOrdersFile;

    public WeeklyOrdersService(string azStoreConnStr, string azStoreContName, string weeklyOrdersFile, IHarvestService harvestService)
    {
        _azStoreConnStr = azStoreConnStr;
        _azStoreContName = azStoreContName;
        _weeklyOrdersFile = weeklyOrdersFile;
        _harvestService = harvestService;
    }

    public List<SearchDto> GetInvoiceWeeksListForYear(int year)
    {
        return Utils.GetWeeksOfYear(year);
    }

    public List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string? week, CompanyEnum company)
    {
        var list = new List<CustomerInvoicesViewModel>();

        //var weekDateTime = Utils.ParseToDateTime(week);
        //if (!weekDateTime.HasValue) return list;

        var weekDate = Utils.ParseToDateTime(week);
        if (!weekDate.HasValue) return list;

        var weekOfYear = GetWeekOfYearForInvoicesFromSheet(weekDate);
        var currentColumnInDt = weekOfYear + 2;

        var year = weekDate.GetValueOrDefault().Year;

        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_weeklyOrdersFile);

        DataTable dtKings, dtMansi, dtCustomer;

        using (var memoryStream = new MemoryStream())
        {
            blob.DownloadTo(memoryStream);

            using (var package = new ExcelPackage(memoryStream))
            {
                var kingsTab = package.Workbook.Worksheets["KINGS"];
                var mansiTab = package.Workbook.Worksheets["MANSI"];
                var customerTab = package.Workbook.Worksheets["ALL CUSTOMERS"];

                dtKings = EpplusUtils.ExcelPackageToDataTable(kingsTab);
                dtMansi = EpplusUtils.ExcelPackageToDataTable(mansiTab);
                dtCustomer = EpplusUtils.ExcelPackageToDataTable(customerTab);
            }
        }


        var customersList = Mapper.MapToCustomerDashboardViewModelList(dtCustomer);

        var dtSource = company is CompanyEnum.Kings or CompanyEnum.KingsSandbox
            ? dtKings
            : company == CompanyEnum.Mansi
                ? dtMansi
                : null;

        if (dtSource == null) return list;


        var harvestList = company is CompanyEnum.Kings or CompanyEnum.KingsSandbox
            ? _harvestService.GetHarvestDataBySeason(year)
            : null;

        var prepList = PrepareInvoicesViewModels(weekDate.GetValueOrDefault(), currentColumnInDt, dtSource, customersList, dtKings, dtMansi, harvestList);

        var invoiceNumbersList = GetInvoiveNumbers(prepList, currentColumnInDt, dtKings, dtMansi);

        var bedPools = GetBedPoolsFromHarvest(weekDate.GetValueOrDefault(), harvestList);

        var queues = GetBoxLotQueues(bedPools, prepList);


        //Dictionary<int, Queue<SearchDto>> boxLotsBySize = GetBoxLotQueues(weekDate.GetValueOrDefault(), prepList, harvestList);

        //list = CustomerInvoicesViewModels(week, dtSource, customersList, dtKings, dtMansi, harvestList);
        list = GetCustomerInvoicesViewModels(prepList, customersList, invoiceNumbersList, queues);


        return list;
    }

    public List<CustomerDashboardViewModel> GetCustomersFromOrdersFile()
    {
        var client = new BlobServiceClient(_azStoreConnStr);
        var container = client.GetBlobContainerClient(_azStoreContName);
        var blob = container.GetBlockBlobClient(_weeklyOrdersFile);

        using var memoryStream = new MemoryStream();
        blob.DownloadTo(memoryStream);

        using var package = new ExcelPackage(memoryStream);
        var customerTab = package.Workbook.Worksheets["ALL CUSTOMERS"];

        var dtCustomer = EpplusUtils.ExcelPackageToDataTable(customerTab);

        var customersList = Mapper.MapToCustomerDashboardViewModelList(dtCustomer);

        return customersList;
    }

    private List<CustomerInvoicesViewModel> GetCustomerInvoicesViewModels(List<PrepareInvoicesViewModel> prepList, List<CustomerDashboardViewModel> customerList, List<SearchDto> invoiceNumbersList, Queue<SearchDto> queue)
    {
        var list = new List<CustomerInvoicesViewModel>();
        foreach (var viewModel in prepList)
        {
            var usdaMemo = GetUsdaMemo(queue, viewModel);

            var dto = invoiceNumbersList.FirstOrDefault(x => x.Id == viewModel.CustomerKey);
            var invoiceNumber = dto == null ? string.Empty : dto.Data;

            var custumerDto = customerList.FirstOrDefault(x => x.CustomerHeader.CustomerKey == viewModel.CustomerKey);


            list.Add(PrepareInvoice(viewModel.Week, viewModel.WeekQty, custumerDto, invoiceNumber, usdaMemo));
        }

        return list;
    }

    private List<SearchDto> GetInvoiveNumbers(List<PrepareInvoicesViewModel> prepList, int currentColumnInDt, DataTable dtKings, DataTable dtMansi)
    {
        var list = new List<SearchDto>();

        foreach (var viewModel in prepList)
        {
            var invoiceNumber = GetNewInvoiceNumber(viewModel.CustomerKey, currentColumnInDt, dtKings, dtMansi);
            list.Add(new SearchDto { Id = viewModel.CustomerKey, Data = invoiceNumber });
        }

        return list;
    }


    //private static List<CustomerInvoicesViewModel> GetCustomerInvoicesViewModels(string? week, DataTable dtSource, List<CustomerDashboardViewModel> customersList, DataTable dtKings, DataTable dtMansi, List<HarvestViewModel> harvests)
    //{
    //    var list = new List<CustomerInvoicesViewModel>();

    //    if (dtSource.Rows.Count <= 0) return list;

    //    var weekDate = Utils.ParseToDateTime(week);
    //    if (!weekDate.HasValue) return list;


    //    var bedPools = MapToBedPools(harvests.FirstOrDefault(x => x.HarvestDate == weekDate.GetValueOrDefault()));
    //    var boxLotQueue = Utils.HasRows(bedPools) ? GetBoxLots(bedPools) : null;

    //    var weekOfYear = GetWeekOfYearForInvoicesFromSheet(weekDate);
    //    var currentColumnInDt = weekOfYear + 2;
    //    var startRow = 1;

    //    for (var row = startRow; row < dtSource.Rows.Count; row++)
    //    {
    //        var dataRow = dtSource.Rows[row];

    //        var customerKey = dataRow[0].ToString();
    //        if (string.IsNullOrEmpty(customerKey)) continue;

    //        var viewModel = customersList.FirstOrDefault(x => x.CustomerHeader.CustomerKey == customerKey);
    //        if (viewModel == null) continue;

    //        var weekQty = Utils.ParseToInteger(dataRow[currentColumnInDt].ToString());
    //        if (weekQty == 0) continue;

    //        var customerBoxSize = Utils.ParseToInteger((viewModel?.Price?.BoxSize ?? 12).ToString(CultureInfo.InvariantCulture));

    //        var usdaMemo = GetUsdaMemo(boxLotQueue, weekQty, customerBoxSize, weekDate.GetValueOrDefault());

    //        var invoiceNumber = GetNewInvoiceNumber(customerKey, currentColumnInDt, dtKings, dtMansi);

    //        list.Add(PrepareInvoice(weekDate.Value, weekQty, viewModel, invoiceNumber, usdaMemo));
    //    }

    //    return list;
    //}

    private Dictionary<string, int> GetBedPoolsFromHarvest(DateTime weekDate, List<HarvestViewModel> harvests)
    {
        return MapToBedPools(harvests.FirstOrDefault(x => x.HarvestDate == weekDate));
    }

    //private Dictionary<int, Queue<SearchDto>> GetBoxLotQueues(Dictionary<string, int> bedPools, List<PrepareInvoicesViewModel> prepList)
    private Queue<SearchDto> GetBoxLotQueues(Dictionary<string, int> bedPools, List<PrepareInvoicesViewModel> prepList)
    {
        //Dictionary<int, Queue<SearchDto>> queues = new Dictionary<int, Queue<SearchDto>>();

        //List<int> sizes = new List<int>() { 12, 10, 5 };
        //foreach (var siz in sizes)
        //{
        //    var sizePresent = prepList.FirstOrDefault(x => x.BoxSize == siz);
        //    if (sizePresent != null)
        //    {
        //        var queue = GetBoxLots(bedPools, prepList.Where(x => x.BoxSize == siz).ToList());
        //        queues.Add(siz, queue);
        //    }
        //}

        //return queues;
        return Utils.HasRows(bedPools) ? GetBoxLots(bedPools, prepList) : new Queue<SearchDto>();
        //return Utils.HasRows(bedPools) ? GetBoxLots(bedPools) : new Queue<SearchDto>();
    }

    private static Queue<SearchDto> GetBoxLots(Dictionary<string, int> pools, List<PrepareInvoicesViewModel> prepList)
        //private static Queue<SearchDto> GetBoxLots(Dictionary<string, int> pools)
    {
        var box12Total = prepList.Where(x=>x.BoxSize == 12).Select(x=>x.WeekQty).Sum()/12;
        var box10Total = prepList.Where(x => x.BoxSize == 10).Select(x => x.WeekQty).Sum()/10;
        var box5Total = prepList.Where(x => x.BoxSize == 5).Select(x => x.WeekQty).Sum()/5;

        var boxCount = 0;
        var lotSequence = 0;

        var box12 = 0;
        var box10 = 0;
        var box5 = 0;

        var list12 = new Queue<SearchDto>();
        var list10 = new Queue<SearchDto>();
        var list5 = new Queue<SearchDto>();

        foreach (var pool in pools)
        {
            var bed = pool.Key;
            var bedHarvestQty = pool.Value;

            while (bedHarvestQty > 0 )
            {
                if (box12 <= box12Total)
                {
                    if (boxCount % 5 == 0) lotSequence++;

                    list12.Enqueue(new SearchDto { Id = bed, Data = lotSequence.ToString(), DataType = $"bed{bed}:{bedHarvestQty.ToString()}" });

                    boxCount++;
                    bedHarvestQty -= 12;

                    box12++;
                    continue;
                }

                if (box10 <= box10Total)
                {
                    if (boxCount % 5 == 0) lotSequence++;

                    list10.Enqueue(new SearchDto { Id = bed, Data = lotSequence.ToString(), DataType = $"bed{bed}:{bedHarvestQty.ToString()}" });

                    boxCount++;
                    bedHarvestQty -= 10;

                    box10++;
                    continue;
                }

                if (box5 <= box5Total)
                {
                    if (boxCount % 5 == 0) lotSequence++;

                    list5.Enqueue(new SearchDto { Id = bed, Data = lotSequence.ToString(), DataType = $"bed{bed}:{bedHarvestQty.ToString()}"  });

                    boxCount++;
                    bedHarvestQty -= 5;

                    box5++;
                    continue;
                }
            }

            
        }

        var list = new Queue<SearchDto>();

        var list12Count = list12.Count;
        var list10Count = list10.Count;
        var list5Count = list5.Count;

        for (var i = 0; i < list12Count; i++) list.Enqueue(list12.Dequeue());
        for (var i = 0; i < list10Count; i++) list.Enqueue(list10.Dequeue());
        for (var i = 0; i < list5Count; i++) list.Enqueue(list5.Dequeue());

        return list;
    }

    private static List<PrepareInvoicesViewModel> PrepareInvoicesViewModels(DateTime weekDate, int currentColumnInDt, DataTable dtSource, List<CustomerDashboardViewModel> customersList, DataTable dtKings, DataTable dtMansi,
        List<HarvestViewModel> harvests)
    {
        var list = new List<PrepareInvoicesViewModel>();

        if (dtSource.Rows.Count <= 0) return list;

        //var weekDate = Utils.ParseToDateTime(week);
        //if (!weekDate.HasValue) return list;


        //var bedPools = MapToBedPools(harvests.FirstOrDefault(x => x.HarvestDate == weekDate.GetValueOrDefault()));
        //var boxLotQueue = Utils.HasRows(bedPools) ? GetBoxLots(bedPools) : null;

        //var weekOfYear = GetWeekOfYearForInvoicesFromSheet(weekDate);
        //var currentColumnInDt = weekOfYear + 2;
        var startRow = 1;

        for (var row = startRow; row < dtSource.Rows.Count; row++)
        {
            var dataRow = dtSource.Rows[row];

            var customerKey = dataRow[0].ToString();
            if (string.IsNullOrEmpty(customerKey)) continue;

            var viewModel = customersList.FirstOrDefault(x => x.CustomerHeader.CustomerKey == customerKey);
            if (viewModel == null) continue;

            var weekQty = Utils.ParseToInteger(dataRow[currentColumnInDt].ToString());
            if (weekQty == 0) continue;

            var boxSize = weekQty % 12 == 0
                ? 12
                : weekQty % 10 == 0
                    ? 10
                    : weekQty % 5 == 0
                        ? 5
                        : 0;

            if (boxSize == 0) continue;

            list.Add(new PrepareInvoicesViewModel { CustomerKey = customerKey, WeekQty = weekQty, BoxSize = boxSize, Week = weekDate });
            //var customerBoxSize = Utils.ParseToInteger((viewModel?.Price?.BoxSize ?? 12).ToString(CultureInfo.InvariantCulture));

            //var usdaMemo = GetUsdaMemo(boxLotQueue, weekQty, customerBoxSize, weekDate.GetValueOrDefault());

            //var invoiceNumber = GetNewInvoiceNumber(customerKey, currentColumnInDt, dtKings, dtMansi);

            //list.Add(PrepareInvoice(weekDate.Value, weekQty, viewModel, invoiceNumber, usdaMemo));
        }

        return list.OrderByDescending(x => x.BoxSize).ThenBy(x => x.CustomerKey).ToList();
    }

    //private static List<CustomerInvoicesViewModel> CustomerInvoicesViewModels(string? week, DataTable dtSource, List<CustomerDashboardViewModel> customersList, DataTable dtKings, DataTable dtMansi, List<HarvestViewModel> harvests)
    //{
    //    var list = new List<CustomerInvoicesViewModel>();

    //    if (dtSource.Rows.Count <= 0) return list;

    //    var weekDate = Utils.ParseToDateTime(week);
    //    if (!weekDate.HasValue) return list;


    //    var bedPools = MapToBedPools(harvests.FirstOrDefault(x => x.HarvestDate == weekDate.GetValueOrDefault()));
    //    var boxLotQueue = Utils.HasRows(bedPools) ? GetBoxLots(bedPools) : null;

    //    var weekOfYear = GetWeekOfYearForInvoicesFromSheet(weekDate);
    //    var currentColumnInDt = weekOfYear + 2;
    //    var startRow = 1;

    //    for (var row = startRow; row < dtSource.Rows.Count; row++)
    //    {
    //        var dataRow = dtSource.Rows[row];

    //        var customerKey = dataRow[0].ToString();
    //        if (string.IsNullOrEmpty(customerKey)) continue;

    //        var viewModel = customersList.FirstOrDefault(x => x.CustomerHeader.CustomerKey == customerKey);
    //        if (viewModel == null) continue;

    //        var weekQty = Utils.ParseToInteger(dataRow[currentColumnInDt].ToString());
    //        if (weekQty == 0) continue;

    //        var customerBoxSize = Utils.ParseToInteger((viewModel?.Price?.BoxSize??12).ToString(CultureInfo.InvariantCulture));

    //        var usdaMemo = GetUsdaMemo(boxLotQueue, weekQty, customerBoxSize, weekDate.GetValueOrDefault());

    //        var invoiceNumber = GetNewInvoiceNumber(customerKey, currentColumnInDt, dtKings, dtMansi);

    //        list.Add(PrepareInvoice(weekDate.Value, weekQty, viewModel, invoiceNumber, usdaMemo));
    //    }

    //    return list;
    //}

    //private static string GetUsdaMemo(Queue<SearchDto>? queue, int customerQty, int customerBoxSize, DateTime weekDate)
    private static string GetUsdaMemo(Queue<SearchDto>? queue, PrepareInvoicesViewModel viewModel)
    {
        if (queue == null) return string.Empty;

        var permit = "2022-064";
        var week = viewModel.Week.ToString("MMddyy");


        //var boxes = viewModel.WeekQty / 12;
        var boxes = viewModel.WeekQty / viewModel.BoxSize;

        var list = new List<SearchDto>();

        for (var i = 0; i < boxes; i++) list.Add(queue.Dequeue());

        var usda_week = $"{permit}-{week}";

        var lotsList = new Dictionary<string, string>();
        var usda_lots = new List<int>();

        foreach (var dto in list)
        {
            var bed = dto.Id;
            var block = GetBlock(bed);
            var lot = Utils.ParseToInteger(dto.Data);
            var other = dto.DataType;

            var usda_block = $"{usda_week}-{block}-{bed}-";


            if (!lotsList.ContainsKey(usda_block))
            {
                usda_lots = new List<int> { lot };
                lotsList.Add(usda_block, $"{LotsToString(usda_lots)} /{other}");
            }
            else
            {
                if (usda_lots.Contains(lot)) continue;

                usda_lots.Add(lot);
                lotsList[usda_block] = $"{LotsToString(usda_lots)} /{other}";
            }
        }

        return BuildMemo(lotsList);
    }

    private static string BuildMemo(Dictionary<string, string> lotsList)
    {
        if (!Utils.HasRows(lotsList)) return string.Empty;

        var sb = new StringBuilder();

        //sb.AppendJoin(";", lotsList);

        foreach (var lot in lotsList) sb.AppendLine($"{lot.Key}{lot.Value}");


        return sb.ToString().Replace(",", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace(" ", string.Empty).Trim();
    }

    private static string LotsToString(ICollection<int> usdaLots)
    {
        if (!Utils.HasRows(usdaLots)) return "?";

        if (usdaLots.Count == 1) return usdaLots.First().ToString();

        var first = usdaLots.First();
        var last = usdaLots.Last();

        return $"{first}-{last}";
    }


    private static string GetBlock(string bed)
    {
        var bedNumber = Utils.ParseToInteger(bed);
        return bedNumber switch
        {
            >= 1 and <= 11 => "74552",
            >= 12 and <= 22 => "74560",
            >= 23 and <= 28 => "74558",
            >= 29 and <= 37 => "74556",
            >= 38 and <= 51 => "74554",
            _ => string.Empty
        };
    }

    private static Dictionary<string, int> MapToBedPools(HarvestViewModel? harvestViewModel)
    {
        var list = new Dictionary<string, int>();
        if (harvestViewModel == null) return list;

        var bedsHarvested = harvestViewModel.BedHarvests;
        if (!Utils.HasRows(bedsHarvested)) return list;

        foreach (var viewModel in bedsHarvested)
        {
            if (string.IsNullOrEmpty(viewModel.BedNumber)) continue;
            list.Add(viewModel.BedNumber.ToLower().Replace("bed", string.Empty).Trim(), viewModel.HarvestQty);
        }

        return list;
    }


    private static CustomerInvoicesViewModel PrepareInvoice(DateTime weekDate, int weekQty, CustomerDashboardViewModel viewModel, string newInvoiceNumber, string usdaMemo)
    {
        var model = new CustomerInvoicesViewModel
        {
            InvoiceNumber = newInvoiceNumber,
            CustomerHeader = viewModel.CustomerHeader,
            InvoiceDate = weekDate,
            DueDate = weekDate,
            Price = new CustomerPriceViewModel
            {
                Rate = viewModel.Price.Rate,
                ShipmentRate = viewModel.Price.ShipmentRate,
                BoxSize = viewModel.Price.BoxSize
            },
            Cost = new InvoiceCostViewModel
            {
                Quantity = weekQty,
                Amount = viewModel.Price.Rate * weekQty
            },
            Bill = new InvoiceBillViewModel
            {
                ShipmentCost = GetShippingBoxCost(weekQty, viewModel.Price.BoxSize, viewModel.Price.ShipmentRate),
                Billed = viewModel.Price.Rate * weekQty + GetShippingBoxCost(weekQty, viewModel.Price.BoxSize, viewModel.Price.ShipmentRate)
            },
            Memo = usdaMemo
        };

        if (viewModel.CustomerHeader.CustomerKey == "MYT-DEN")
        {
            model.Bill.ChargesDiscounts = 5.76m;
            model.Bill.Billed += 5.76m;
        }

        if (viewModel.CustomerHeader.CustomerKey == "TRI-CHA")
        {
            model.Bill.ChargesDiscounts = -30m;
            model.Bill.Billed -= 30m;
        }

        return model;
    }

    private static string GetNewInvoiceNumber(string customerKey, int currentColumn, DataTable dtKings, DataTable dtMansi)
    {
        if (currentColumn <= 2) return $"{customerKey}-101/{DateTime.Today:yy}";

        var newInvoiceNumber = 101;

        //find number of shipments prior to current week for Kings for customerKey
        newInvoiceNumber = GetLastInvoiceNumberFromDataTable(customerKey, currentColumn, dtKings, newInvoiceNumber);
        newInvoiceNumber = GetLastInvoiceNumberFromDataTable(customerKey, currentColumn, dtMansi, newInvoiceNumber);

        return $"{customerKey}-{newInvoiceNumber}/{DateTime.Today:yy}";
    }

    private static int GetLastInvoiceNumberFromDataTable(string customerKey, int currentColumn, DataTable dataTable, int newInvoiceNumber)
    {
        var found = false;
        foreach (DataRow dataRow in dataTable.Rows)
        {
            if (dataRow[0].ToString() == customerKey) found = true;
            if (!found) continue;

            //loop thru each week up to one week prior to current week
            for (var col = 2; col < currentColumn; col++)
            {
                var weekQty = Utils.ParseToInteger(dataRow[col].ToString());
                if (weekQty > 0) newInvoiceNumber += 1;
            }

            break;
        }

        return newInvoiceNumber;
    }

    private static int GetWeekOfYearForInvoicesFromSheet(DateTime? weekDate)
    {
        var weekOfYear = Utils.GetWeekOfYear(weekDate.Value);
        var firstMondayOfYear = Utils.GetFirstMondayOfYear(DateTime.Today.Year);
        var firstWeekOfYear = Utils.GetWeekOfYear(firstMondayOfYear);
        if (firstWeekOfYear > 1) weekOfYear -= 1;
        return weekOfYear;
    }

    private static decimal GetShippingBoxCost(int weekQty, decimal boxSize, decimal shipmentRate)
    {
        if (shipmentRate == 0) return 0;

        var shipmentCost = weekQty / boxSize * shipmentRate;
        return Math.Round(shipmentCost);
    }
}

internal class PrepareInvoicesViewModel
{
    public string CustomerKey { get; set; }
    public int WeekQty { get; set; }
    public int BoxSize { get; set; }
    public DateTime Week { get; set; }
}
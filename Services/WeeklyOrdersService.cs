using System.Data;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Mappers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using OfficeOpenXml;

namespace KingsFarms.Core.Api.Services
{
    public class WeeklyOrdersService : IWeeklyOrdersService
    {
        private readonly string _azStoreConnStr;
        private readonly string _azStoreContName;
        private readonly string _weeklyOrdersFile;

        public WeeklyOrdersService(string azStoreConnStr, string azStoreContName, string weeklyOrdersFile)
        {
            _azStoreConnStr = azStoreConnStr;
            _azStoreContName = azStoreContName;
            _weeklyOrdersFile = weeklyOrdersFile;
        }

        public List<SearchDto> GetInvoiceWeeksListForYear()
        {
            return Utils.GetWeeksOfYear(DateTime.Today.Year);
        }

        public List<CustomerInvoicesViewModel> LoadInvoicesForWeek(string? week, CompanyEnum company)
        {
            var list = new List<CustomerInvoicesViewModel>();

            var client = new BlobServiceClient(_azStoreConnStr);
            var container = client.GetBlobContainerClient(_azStoreContName);
            var blob = container.GetBlockBlobClient(_weeklyOrdersFile);

            using (var memoryStream = new MemoryStream())
            {
                blob.DownloadTo(memoryStream);

                using (var package = new ExcelPackage(memoryStream))
                {
                    var kingsTab = package.Workbook.Worksheets["KINGS"];
                    var mansiTab = package.Workbook.Worksheets["MANSI"];
                    var customerTab = package.Workbook.Worksheets["ALL CUSTOMERS"];

                    var dtKings = EpplusUtils.ExcelPackageToDataTable(kingsTab);
                    var dtMansi = EpplusUtils.ExcelPackageToDataTable(mansiTab);
                    var dtCustomer = EpplusUtils.ExcelPackageToDataTable(customerTab);

                    var customersList = Mapper.MapToCustomerDashboardViewModelList(dtCustomer);

                    var dtSource = company == CompanyEnum.Kings || company == CompanyEnum.KingsSandbox
                        ? dtKings
                        : company == CompanyEnum.Mansi
                            ? dtMansi
                            : null;

                    if (dtSource == null) return list;

                    list = CustomerInvoicesViewModels(week, dtSource, customersList, dtKings, dtMansi);
                }
            }

            return list;
        }

        private static List<CustomerInvoicesViewModel> CustomerInvoicesViewModels(string? week, DataTable dtSource, List<CustomerDashboardViewModel> customersList, DataTable dtKings, DataTable dtMansi)
        {
            var list = new List<CustomerInvoicesViewModel>();

            if (dtSource.Rows.Count <= 0) return list;

            var weekDate = Utils.ParseToDateTime(week);
            if (!weekDate.HasValue) return list;

            var weekOfYear = GetWeekOfYearForInvoicesFromSheet(weekDate);
            var currentColumnInDt = weekOfYear + 1;
            var startRow = 1;

            for (var row = startRow; row < dtSource.Rows.Count; row++)
            {
                var dataRow = dtSource.Rows[row];

                var customerKey = dataRow[0].ToString();
                var customerDashboardViewModel = customersList.FirstOrDefault(x => x.CustomerHeader.CustomerKey == customerKey);
                if (customerDashboardViewModel == null) continue;

                var weekQty = Utils.ParseToInteger(dataRow[currentColumnInDt].ToString());
                if (weekQty == 0) continue;

                list.Add(PrepareInvoice(weekDate.Value, weekQty, customerDashboardViewModel, GetNewInvoiceNumber(customerKey, currentColumnInDt, dtKings, dtMansi)));
            }

            return list;
        }

        private static CustomerInvoicesViewModel PrepareInvoice(DateTime weekDate, int weekQty, CustomerDashboardViewModel viewModel, string newInvoiceNumber)
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
                }
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
}
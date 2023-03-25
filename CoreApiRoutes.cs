namespace KingsFarms.Core.Api;

public static class CoreApiRoutes
{
    //CUSTOMER 
    public const string GetCustomersFromOrdersFile = "api/customers/file";
    public const string GetCustomersFromDb = "api/customers";
    public const string SendCustomersToDb = "api/customers/sendToDb";

    //INVOICES
    public const string LoadInvoicesForWeek = "api/orders/{week}/{company}/load";
    public const string GetInvoiceWeeksListForYear = "api/orders/weeks/{year}";

    //HARVEST
    public const string HarvestDataBySeason = "api/harvest/data/{season}";
    public const string HarvestDataByCalendar = "api/harvest/calendar/{calendar}";
    public const string HarvestYearTotalBySeason = "api/harvest/{season}/yearTotal";
    public const string HarvestYearTotalByCalendar = "api/harvest/calendar/{calendar}/total";
    public const string HarvestStatusTotal = "api/harvest/{status}/statusTotal";
    public const string HarvestDataCalendarAll = "api/harvest/calendarall";
    public const string HarvestDataSeasonAll = "api/harvest/seasonall";
    public const string HarvestDataAll = "api/harvest/list";
    public const string HarvestByYear = "api/harvest/byYear";
    public const string HarvestByYearByBed = "api/harvest/byYearByBed";
    public const string HarvestByYearBySection = "api/harvest/byYearBySection";

    public const string SaveHarvestData = "api/harvest/save";

    //BEDS
    public const string BedList = "api/beds/list";
    public const string BedInfo = "api/beds/info";
    public const string BedInfoGrouped = "api/beds/info/grouped";

    //MANURE
    public const string ManureForMonth = "api/manure/month/{month}";
    public const string ManureAllMonths = "api/manure/month/all";
    public const string ManureForFarm = "api/manure/farm/{farm}";
    public const string ManureFarms = "api/manure/farms";
    public const string ManureFarmsLoads = "api/manure/farms/loads";

    //DASHBOARD
    public const string Dashboard = "api/dashboard";

    //FEDEX
    public const string FedExLocationInfo = "api/fedex/location/info";
    public const string ValidatedAddress = "api/fedex/validate/address";
    public const string CreateShipment = "api/fedex/shipment/create";
    public const string CreateShipmentRequest = "api/fedex/shipment/createrequest";

    //WEEKS
    public const string FirstMondayOfYear = "api/orders/{year}/monday";
    public const string FirstSaturdayOfYear = "api/orders/{year}/saturday";

    //SYNC
    public const string SyncBedsInfo = "api/sync/bedsInfo";
    public const string SyncHarvestsInfo = "api/sync/harvestsInfo";

    //MESSAGING
    public const string SendCustomerInvoiceSms = "api/messaging/sendsms";
    public const string SendCustomerInvoiceBulkYearSms = "api/messaging/sendsmsyear";
}
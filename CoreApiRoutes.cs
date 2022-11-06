namespace KingsFarms.Core.Api;

public static class CoreApiRoutes
{
    //CUSTOMER 
    public const string GetCustomersFromOrdersFile = "api/customers/file";

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

    //BEDS
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
}
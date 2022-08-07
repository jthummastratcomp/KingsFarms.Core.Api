namespace KingsFarms.Core.Api;

public static class ApiRoutes
{
    public const string ReportsServiceUrl = "api/admin/reportsServiceUrl";

    //CUSTOMER 
    public const string CustomersDashboard = "api/customerdashboard/{currentUser}/{filter}/{filterValue}/{status}";
    public const string AllCustomersBillTotal = "api/customer/{status}/allBillTotal";
    public const string AllCustomersShipmentsTotal = "api/customer/{status}/shipments";
    public const string AllCustomersYearlyShipmentsBills = "api/customer/yearlyShipmentsBills";
    public const string CustomerHeader = "api/customer/{customerId}/header";
    public const string CustomerInvoices = "api/customer/{customerId}/invoices";
    public const string CustomerBillTotal = "api/customer/{customerId}/billTotal";
    public const string CustomerPriceTotal = "api/customer/{customerId}/priceTotal";
    public const string AddCustomerInvoice = "api/customer/{customerId}/invoice/add";
    public const string CustomerYearlyShipmentsBills = "api/customer/{customerId}/yearlyShipmentsBills";
    public const string CustomerWeeklyShipments = "api/customer/{customerId}/weeklyShipments";
    public const string AllCustomersWeeklyShipmentsForGraph = "shipments/weekly/graph";
    
    public const string LoadInvoicesForWeek = "api/invoices/{week}/load";
    public const string GenerateInvoicesForWeek = "api/invoices/{week}/generate";

    //ADMIN
    public const string CompanyInfo = "api/admin/companyInfo";
    public const string LoadCustomerCache = "api/admin/loadCustomerCache";
    public const string LoadInvoiceCache = "api/admin/loadInvoiceCache";

    //OPERATIONS
    public const string FieldOperations = "api/operation/field";

    //HARVEST
    public const string HarvestWeeks = "api/harvest/weeks";
    public const string HarvestData = "api/harvest/data/{year}";
    public const string HarvestYearTotal = "api/harvest/{year}/yearTotal";
    public const string HarvestStatusTotal = "api/harvest/{status}/statusTotal";

    //BEDS
    public const string BedInfo = "api/beds/info";
    public const string BedInfoGrouped = "api/beds/info/grouped";
    public const string BedInfoHarvestFieldOps = "api/harvest/bedInfoHarvestOps";

    //MANURE
    public const string ManureForMonth = "api/manure/month/{month}";
    public const string ManureAllMonths = "api/manure/month/all";
    public const string ManureForFarm = "api/manure/farm/{farm}";
    public const string ManureAllFarms = "api/manure/farm/all";
    

    //DASHBOARD
    public const string Dashboard = "api/dashboard";


    //FEDEX
    public const string FedExLocationInfo = "api/fedex/location/info";
    public const string ValidatedAddress = "api/fedex/validate/address";
    public const string CreateShipment = "api/fedex/shipment/create";
    public const string CreateShipmentRequest = "api/fedex/shipment/createrequest";

}
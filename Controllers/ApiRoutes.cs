namespace HotTowel.Core.Api.Controllers;

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
    public const string AllCustomersWeeklyShipmentsForGraph = "api/customer/weeklyShipmentsGraph";
    public const string HarvestWeeks = "harvest/weeks";
    public const string LoadInvoicesForWeek = "api/invoices/{week}/load";
    public const string GenerateInvoicesForWeek = "api/invoices/{week}/generate";

    //ADMIN
    public const string CompanyInfo = "api/admin/companyInfo";
    public const string LoadCustomerCache = "api/admin/loadCustomerCache";
    public const string LoadInvoiceCache = "api/admin/loadInvoiceCache";

    //OPERATIONS
    public const string FieldOperations = "api/operation/field";

    //HARVEST
    public const string HarvestInfo = "api/harvest/info";
    public const string BedInfo = "api/harvest/bedInfo";
    public const string BedInfoGrouped = "api/harvest/bedInfo/grouped";
    public const string BedInfoHarvestFieldOps = "api/harvest/bedInfoHarvestOps";
}
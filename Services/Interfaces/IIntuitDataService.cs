using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;

namespace HotTowel.Web.Services.Interfaces
{
    public interface IIntuitDataService
    {
        void SetDataService(DataService dataService);
        DataService GetDataService();
        CompanyInfo GetCompany();
    }
}
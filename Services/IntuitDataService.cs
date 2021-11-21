using System.Linq;
using HotTowel.Web.Cache;
using HotTowel.Web.Services.Interfaces;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;

namespace HotTowel.Web.Services
{
    public class IntuitDataService : IIntuitDataService
    {
        private DataService _dataService;

        public void SetDataService(DataService dataService)
        {
            _dataService = dataService;
        }

        //[CacheTimeout]
        public DataService GetDataService()
        {
            return _dataService;
        }

        public CompanyInfo GetCompany()
        {
            return _dataService.FindAll(new Intuit.Ipp.Data.CompanyInfo()).FirstOrDefault();

        }
    }
}
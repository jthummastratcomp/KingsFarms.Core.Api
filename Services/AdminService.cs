using System.Collections.Generic;
using System.Linq;
using HotTowel.Web.Cache;
using HotTowel.Web.Helpers;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services
{
    public class AdminService : IAdminService
    {
        private readonly IIntuitDataService _intuitDataService;

        public AdminService(IIntuitDataService intuitDataService)
        {
            _intuitDataService = intuitDataService;
        }

        [CacheTimeout]
        public CompanyViewModel GetCompanyInfo()
        {
            var company = _intuitDataService.GetCompany();
            var vm = Mapper.MapCompanyInfoToCompanyViewModel(company);
            return vm;
        }

        [CacheTimeout]
        public List<Customer> LoadAllCustomers()
        {
            var dataService = _intuitDataService.GetDataService();
            return dataService.FindAll(new Customer()).ToList();
        }
    }
}
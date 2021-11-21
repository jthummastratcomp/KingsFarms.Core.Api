using System.Collections.Generic;
using System.Linq;
using HotTowel.Web.Cache;
using HotTowel.Web.Services.Interfaces;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services
{
    public class CustomerLoadService : ICustomerLoadService
    {
        private readonly IIntuitDataService _intuitDataService;

        public CustomerLoadService(IIntuitDataService intuitDataService)
        {
            _intuitDataService = intuitDataService;
        }

        [CacheTimeout]
        public List<Customer> LoadAllCustomers()
        {
            var dataService = _intuitDataService.GetDataService();
            return dataService.FindAll(new Customer()).ToList();
        }
    }
}
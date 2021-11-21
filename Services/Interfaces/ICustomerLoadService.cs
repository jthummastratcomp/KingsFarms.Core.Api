using System.Collections.Generic;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services.Interfaces
{
    public interface ICustomerLoadService
    {
        List<Customer> LoadAllCustomers();
    }
}
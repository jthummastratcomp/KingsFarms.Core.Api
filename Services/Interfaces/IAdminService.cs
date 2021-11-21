using System.Collections.Generic;
using HotTowel.Web.ViewModels;
using Intuit.Ipp.Data;

namespace HotTowel.Web.Services.Interfaces
{
    public interface IAdminService
    {
        CompanyViewModel GetCompanyInfo();
    }
}
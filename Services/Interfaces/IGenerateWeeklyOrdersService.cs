using System.Collections.Generic;
using HotTowel.Web.ViewModels;

namespace HotTowel.Web.Services.Interfaces
{
    public interface IGenerateWeeklyOrdersService
    {
        List<string> GenerateInvoicesForWeek(string week, CompanyEnum company);
    }
}
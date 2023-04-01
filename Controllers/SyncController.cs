using KingsFarms.Core.Api.Results;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels.Customer;
using KingsFarms.Core.Api.ViewModels.Harvest;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class SyncController : ControllerBase
{
    private readonly ISyncService _syncService;

    public SyncController(ISyncService syncService)
    {
        _syncService = syncService;
    }


    //[HttpGet(CoreApiRoutes.SyncBeds)]
    //public IQueryResult SyncBedsInfo()
    //{
    //    var list = _syncService.SyncBeds();

    //    return new QueryResult<string> {Data = list, Status = new SuccessResult()};
    //}

    //[HttpGet(CoreApiRoutes.SyncHarvests)]
    //public IQueryResult SyncHarvestsInfo()
    //{
    //    var list = _syncService.SyncHarvests();

    //    return new QueryResult<string> {Data = list, Status = new SuccessResult()};
    //}

    [HttpGet(CoreApiRoutes.SyncCustomers)]
    public IQueryResult SyncCustomers()
    {
        var list = _syncService.SyncCustomers();

        return new QueryResult<string> {Data = list, Status = new SuccessResult()};
    }

    //[HttpGet(CoreApiRoutes.SyncInvoices)]
    //public IQueryResult SyncInvoices()
    //{
    //    var list = _syncService.SyncInvoices();

    //    return new QueryResult<string> {Data = list, Status = new SuccessResult()};
    //}

    //[HttpPost(CoreApiRoutes.SendCustomersToDb)]
    //public IQueryResult SyncCustomers(List<CustomerHeaderViewModel> list)
    //{
    //    var response = _syncService.SyncCustomers(list);

    //    return new QueryResult<string> {Data = response, Status = new SuccessResult()};
    //}

    //[HttpPost(CoreApiRoutes.SaveHarvestData)]
    //public IQueryResult SaveHarvestData(HarvestViewModel viewModel)
    //{
    //    var response = _syncService.SaveHarvestData(viewModel);

    //    return new QueryResult<string> {Data = response, Status = new SuccessResult()};
    //}
}
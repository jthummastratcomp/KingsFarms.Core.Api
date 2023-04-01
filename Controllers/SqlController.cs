using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Results;
using KingsFarms.Core.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class SqlController : ControllerBase
{
    private readonly ISqlService _sqlService;

    public SqlController(ISqlService sqlService)
    {
        _sqlService = sqlService;
    }

    //[HttpGet(CoreApiRoutes.GetBedsFromSql)]
    //public IQueryResult SyncBeds()
    //{
    //    var list = _sqlService.GetBeds();

    //    return new QueryResult<List<Bed>> {Data = list, Status = new SuccessResult()};
    //}

    //[HttpGet(CoreApiRoutes.GetHarvestsFromSql)]
    //public IQueryResult GetHarvestsFromSql()
    //{
    //    var list = _sqlService.GetHarvests();

    //    return new QueryResult<List<Harvest>> {Data = list, Status = new SuccessResult()};
    //}

    [HttpGet(CoreApiRoutes.GetCustomersFromSql)]
    public IQueryResult GetCustomersFromSql()
    {
        var list = _sqlService.GetCustomers();

        return new QueryResult<List<Customer>> {Data = list, Status = new SuccessResult()};
    }

    //[HttpGet(CoreApiRoutes.GetInvoicesFromSql)]
    //public IQueryResult GetInvoicesFromSql()
    //{
    //    var list = _sqlService.GetInvoices();

    //    return new QueryResult<List<Invoice>> {Data = list, Status = new SuccessResult()};
    //}
}
using HotTowel.Core.Api.Enums;
using HotTowel.Core.Api.Results;
using HotTowel.Core.Api.Services.Interfaces;
using HotTowel.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace HotTowel.Core.Api.Controllers;

[ApiController]
public class ManureController : ControllerBase
{
    private readonly ILogger _logger;

    private readonly IHorseManureService _manureService;


    public ManureController(IHorseManureService manureService, ILogger logger)
    {
        _manureService = manureService;

        _logger = logger;
    }


    [HttpGet(ApiRoutes.ManureForMonth, Name = "ManureForMonth")]
    public IQueryResult GetManureForMonth(MonthEnum month)
    {
        _logger.Information("GetManureForMonth");
        
        return new QueryResult<List<ManureLoadViewModel>> { Data = GetManureLoadsForMonth(month), Status = new SuccessResult() };
    }

    [HttpGet(ApiRoutes.ManureAllMonths, Name = "ManureAllMonths")]
    public IQueryResult GetManureAllMonths()
    {
        _logger.Information("GetManureAllMonths");

        var list = GetManureLoadsAllMonths();

        return new QueryResult<List<ManureLoadViewModel>> { Data = list, Status = new SuccessResult() };
    }

    private List<ManureLoadViewModel> GetManureLoadsAllMonths()
    {
        var list = GetManureLoadsForMonth(MonthEnum.January);
        list.AddRange(GetManureLoadsForMonth(MonthEnum.February));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.March));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.April));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.May));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.June));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.July));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.August));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.September));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.October));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.November));
        list.AddRange(GetManureLoadsForMonth(MonthEnum.December));
        return list;
    }

    [HttpGet(ApiRoutes.ManureForFarm, Name = "ManureForFarm")]
    public IQueryResult GetManureForFarm(string farm)
    {
        _logger.Information("GetManureForFarm");

        var list = GetManureLoadsAllMonths();

        list = list.Where(x => x.Loads.ContainsKey(farm)).ToList();

        return new QueryResult<List<ManureLoadViewModel>> { Data = list, Status = new SuccessResult() };
    }
    

    private List<ManureLoadViewModel> GetManureLoadsForMonth(MonthEnum month)
    {
        var list = _manureService.GetManureLoadForMonth(month);
        return list;
    }
}
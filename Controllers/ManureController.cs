using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Results;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Controllers;

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


    [HttpGet(CoreApiRoutes.ManureForMonth, Name = "ManureForMonth")]
    public IQueryResult GetManureForMonth(MonthEnum month)
    {
        _logger.Information("GetManureForMonth");

        return new QueryResult<List<ManureLoadViewModel>> { Data = _manureService.GetManureLoadForMonth(month), Status = new SuccessResult() };
    }

    [HttpGet(CoreApiRoutes.ManureAllMonths, Name = "ManureAllMonths")]
    public IQueryResult GetManureAllMonths()
    {
        _logger.Information("GetManureAllMonths");

        var list = _manureService.GetManureLoads();

        return new QueryResult<List<ManureLoadViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(CoreApiRoutes.ManureForFarm, Name = "ManureForFarm")]
    public IQueryResult GetManureForFarm(string farm)
    {
        _logger.Information("GetManureForFarm");

        var list = _manureService.GetManureLoads();

        list = list.Where(x => x.Loads.ContainsKey(farm)).ToList();

        return new QueryResult<List<ManureLoadViewModel>> { Data = list, Status = new SuccessResult() };
    }

    [HttpGet(CoreApiRoutes.ManureFarms, Name = "ManureFarms")]
    public IQueryResult GetManureFarms()
    {
        return new QueryResult<List<string>> { Data = GetFarmsList(), Status = new SuccessResult() };
    }

    private List<string> GetFarmsList()
    {
        var farmsList = new List<string>();
        var list = _manureService.GetManureLoads();

        foreach (var viewModel in list) farmsList.AddRange(viewModel.Loads.Keys.ToList());

        return farmsList.Distinct().ToList();
    }

    [HttpGet(CoreApiRoutes.ManureFarmsLoads, Name = "ManureFarmsLoads")]
    public IQueryResult GetManureFarmLoads()
    {
        var farmLoads = new Dictionary<string, int>();
        var list = _manureService.GetManureLoads();

        foreach (var loads in list.Select(viewModel => viewModel.Loads))
        foreach (var (farm, value) in loads)
            if (farmLoads.ContainsKey(farm)) farmLoads[farm] += value;
            else farmLoads.Add(farm, value);

        return new QueryResult<Dictionary<string, int>> { Data = farmLoads, Status = new SuccessResult() };
    }
}
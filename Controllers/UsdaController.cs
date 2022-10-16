using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class UsdaController : ControllerBase
{
    private readonly IUsdaService _usdaService;


    public UsdaController(IUsdaService usdaService)
    {
        _usdaService = usdaService;
    }


    [HttpPost(CoreApiRoutes.UpdateUsdaInfo)]
    public string UpdateUsdaInfo(UsdaBedLotInfoViewModel usdaInfoViewModel)
    {
        return _usdaService.UpdateUsdaInfo(usdaInfoViewModel);
    }

    [HttpPost(CoreApiRoutes.CreateUsdaInfo)]
    public string CreateUsdaInfo(UsdaBedLotInfoViewModel usdaInfoViewModel)
    {
        return _usdaService.UpdateUsdaInfo(usdaInfoViewModel);
    }
}
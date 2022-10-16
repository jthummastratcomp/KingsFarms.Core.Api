using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IUsdaService
{
    string UpdateUsdaInfo(UsdaBedLotInfoViewModel viewModel);
}
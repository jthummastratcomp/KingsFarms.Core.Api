using KingsFarms.Core.Api.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IUsdaQueueService
{
    Queue<SearchDto> GetQueues(DateTime weekDate, List<HarvestViewModel> harvestList, List<PrepareInvoicesViewModel> prepList);
}
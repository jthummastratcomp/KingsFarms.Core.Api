using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class UsdaQueueService : IUsdaQueueService
{
    public Queue<SearchDto> GetQueues( DateTime weekDate, List<HarvestViewModel> harvestList, List<PrepareInvoicesViewModel> prepList)
    {
        var bedPools = GetBedPoolsFromHarvest(weekDate, harvestList);

        var queues = GetBoxLotQueues(bedPools, prepList);
        return queues;
    }

    private Dictionary<string, int> GetBedPoolsFromHarvest(DateTime weekDate, List<HarvestViewModel> harvests)
    {
        return MapToBedPools(harvests.FirstOrDefault(x => x.HarvestDate == weekDate));
    }

    private static Dictionary<string, int> MapToBedPools(HarvestViewModel? harvestViewModel)
    {
        var list = new Dictionary<string, int>();
        if (harvestViewModel == null) return list;

        var bedsHarvested = harvestViewModel.BedHarvests.OrderByDescending(x=>x.BedNumber).ToList();
        if (!Utils.HasRows(bedsHarvested)) return list;

        foreach (var viewModel in bedsHarvested)
        {
            if (string.IsNullOrEmpty(viewModel.BedNumber)) continue;
            list.Add(viewModel.BedNumber.ToLower().Replace("bed", string.Empty).Trim(), viewModel.HarvestQty);
        }

        return list;
    }

    private Queue<SearchDto> GetBoxLotQueues(Dictionary<string, int> bedPools, List<PrepareInvoicesViewModel> prepList)
    {
        return Utils.HasRows(bedPools) ? GetBoxLots(bedPools, prepList) : new Queue<SearchDto>();
    }


    private static Queue<SearchDto> GetBoxLots(Dictionary<string, int> pools, List<PrepareInvoicesViewModel> prepList)
    {
        var box12Total = prepList.Where(x => x.BoxSize == 12).Select(x => x.WeekQty).Sum() / 12;
        var box10Total = prepList.Where(x => x.BoxSize == 10).Select(x => x.WeekQty).Sum() / 10;
        var box5Total = prepList.Where(x => x.BoxSize == 5).Select(x => x.WeekQty).Sum() / 5;

        var boxCount = 0;
        var lotSequence = 0;

        var box12 = 0;
        var box10 = 0;
        var box5 = 0;

        var list12 = new Queue<SearchDto>();
        var list10 = new Queue<SearchDto>();
        var list5 = new Queue<SearchDto>();

        foreach (var pool in pools)
        {
            var bed = pool.Key;
            var bedHarvestQty = pool.Value;

            while (bedHarvestQty > 0)
            {
                if (box12 <= box12Total)
                {
                    if (boxCount % 5 == 0) lotSequence++;

                    list12.Enqueue(new SearchDto { Id = bed, Data = lotSequence.ToString(), DataType = $"bed{bed}:{bedHarvestQty.ToString()}" });

                    boxCount++;
                    bedHarvestQty -= 12;

                    box12++;
                    continue;
                }

                if (box10 <= box10Total)
                {
                    if (boxCount % 5 == 0) lotSequence++;

                    list10.Enqueue(new SearchDto { Id = bed, Data = lotSequence.ToString(), DataType = $"bed{bed}:{bedHarvestQty.ToString()}" });

                    boxCount++;
                    bedHarvestQty -= 10;

                    box10++;
                    continue;
                }

                if (box5 <= box5Total)
                {
                    if (boxCount % 5 == 0) lotSequence++;

                    list5.Enqueue(new SearchDto { Id = bed, Data = lotSequence.ToString(), DataType = $"bed{bed}:{bedHarvestQty.ToString()}" });

                    boxCount++;
                    bedHarvestQty -= 5;

                    box5++;
                    continue;
                }
            }


        }

        var list = new Queue<SearchDto>();

        var list12Count = list12.Count;
        var list10Count = list10.Count;
        var list5Count = list5.Count;

        for (var i = 0; i < list12Count; i++) list.Enqueue(list12.Dequeue());
        for (var i = 0; i < list10Count; i++) list.Enqueue(list10.Dequeue());
        for (var i = 0; i < list5Count; i++) list.Enqueue(list5.Dequeue());

        return list;
    }


   
}
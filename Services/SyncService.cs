using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Providers;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Services;

public class SyncService : ISyncService
{
    private readonly IBedService _bedService;
    private readonly IHarvestService _harvestService;
    private readonly IUnitOfWork _unitOfWork;

    public SyncService(IBedService bedService, IHarvestService harvestService, IUnitOfWork unitOfWork)
    {
        _bedService = bedService;
        _harvestService = harvestService;
        _unitOfWork = unitOfWork;
    }

    public string SyncBedsInfo()
    {
        var list = _bedService.GetBedsInfo();


        foreach (var vm in list)
        {
            var bedNumber = Utils.ParseToInteger(vm.BedNumber.ToLower().Replace("bed", "").Trim());

            var bed = _unitOfWork.BedsRepo.Find(x => x.Number == bedNumber).FirstOrDefault();
            if (bed == null)
            {
                _unitOfWork.BedsRepo.Add(new Bed { Number = bedNumber, Section = vm.SectionDisplay, PlantedDate = vm.PlantedDate, PlantsCount = vm.PlantsCount });
            }
            else
            {
                bed.Section = vm.SectionDisplay;
                bed.PlantedDate = vm.PlantedDate;
                bed.PlantsCount = vm.PlantsCount;

                _unitOfWork.BedsRepo.Update(bed);
            }
        }

        _unitOfWork.SaveChanges();
        return "Synchronized Beds";
    }

    public string SyncHarvestInfo()
    {
        var list = _harvestService.GetAllHarvestData();


        foreach (var vm in list)
        {
            var bed = _unitOfWork.BedsRepo.Get(vm.BedNumber);
            if (bed != null)
            {
                var harvest = new Harvest()
                {
                    HarvestDate = vm.HarvestDate, Quantity = vm.HarvestQty
                };
                harvest.BedId = bed.Id;
                _unitOfWork.HarvestRepo.Add(harvest);
            }


        }

        _unitOfWork.SaveChanges();
        return "Synchronized Harvests";
    }
}
namespace KingsFarms.Core.Api.ViewModels.Harvest;

public class BedsMaintenanceViewModel
{
    public int Id { get; set; }

    //public HarvestBedViewModel HarvestBed { get; set; }
    public List<FieldOperationViewModel> FieldOperations { get; set; }
}
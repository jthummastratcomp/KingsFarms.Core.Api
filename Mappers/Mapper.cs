using System.Data;
using KingsFarms.Core.Api.Enums;
using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Mappers;

public class Mapper
{
    public static List<CustomerDashboardViewModel> MapToCustomerDashboardViewModelList(DataTable dtCustomer)
    {
        var list = new List<CustomerDashboardViewModel>();
        if (dtCustomer == null || dtCustomer.Rows.Count == 0) return list;

        list.AddRange(from DataRow row in dtCustomer.Rows
            select new CustomerDashboardViewModel
            {
                CustomerHeader = new CustomerHeaderViewModel
                {
                    CustomerKey = GetRowValue(row, "Customer"),
                    StoreName = GetRowValue(row, "Store Name"),
                    Address = new AddressViewModel
                    {
                        Street = GetRowValue(row, "Address"),
                        City = GetRowValue(row, "City"),
                        State = GetRowValue(row, "State"),
                        Zip = GetRowValue(row, "Zip")
                    },
                    Contact = new ContactViewModel
                    {
                        FirstName = GetRowValue(row, "First Name"),
                        LastName = GetRowValue(row, "Last Name"),
                        Email = GetRowValue(row, "Email"),
                        Phone = GetRowValue(row, "Phone")
                    }
                },
                Price = new CustomerPriceViewModel
                {
                    Rate = Utils.ParseToDecimal(GetRowValue(row, "Rate")),
                    BoxSize = Utils.ParseToInteger(GetRowValue(row, "Size")),
                    ShipmentRate = Utils.ParseToDecimal(GetRowValue(row, "Ship"))
                }
            });

        return list;
    }

    private static string? GetRowValue(DataRow row, string columnName)
    {
        var value = row[columnName].ToString();
        return string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
    }

    //public static CompanyViewModel MapCompanyInfoToCompanyViewModel(CompanyInfo company)
    //{
    //    if (company == null) return new CompanyViewModel();

    //    return new CompanyViewModel()
    //    {
    //        CompanyName = company.CompanyName,
    //        LegalName = company.LegalName,
    //        Company = company.CompanyName.GetEnum<CompanyEnum>(),
    //        Address = new AddressViewModel()
    //        {
    //            Street = company.CustomerCommunicationAddr.Line1,
    //            City = company.CustomerCommunicationAddr.City,
    //            State = company.CustomerCommunicationAddr.CountrySubDivisionCode,
    //            Zip = company.CustomerCommunicationAddr.PostalCode
    //        }
    //    };
    //}

    public static List<BedHarvestFieldOpsCosmosDbModel> MapBedHarvestFieldOpsViewModelToBedHarvestFieldOpsCosmosDbModel(List<BedHarvestFieldOpsViewModel> viewModelList)
    {
        return viewModelList.Select(MapBedHarvestFieldOpsViewModelToBedHarvestFieldOpsCosmosDbModel).ToList();
    }

    public static BedHarvestFieldOpsCosmosDbModel MapBedHarvestFieldOpsViewModelToBedHarvestFieldOpsCosmosDbModel(BedHarvestFieldOpsViewModel viewModel)
    {
        if (viewModel == null) return new BedHarvestFieldOpsCosmosDbModel();

        return new BedHarvestFieldOpsCosmosDbModel
        {
            Id = viewModel.Id,
            PartitionKey = viewModel.PartitionKey,

            BedNumber = viewModel.BedNumber,
            Section = viewModel.Section.GetDescription(),
            PlantedDate = viewModel.PlantedDateDisplay,
            PlantsCount = viewModel.PlantsCount,
            Harvests = viewModel.Harvests.ToArray()
            //FieldOperations = viewModel.FieldOperations.ToArray()
            //Address = new AddressViewModel()
            //{
            //    Street = company.CustomerCommunicationAddr.Line1,
            //    City = company.CustomerCommunicationAddr.City,
            //    State = company.CustomerCommunicationAddr.CountrySubDivisionCode,
            //    Zip = company.CustomerCommunicationAddr.PostalCode
            //}
        };
    }

    public static List<BedHarvestFieldOpsViewModel> MapBedHarvestFieldOpsCosmosDbModelToBedHarvestFieldOpsViewModel(List<BedHarvestFieldOpsCosmosDbModel> modelList)
    {
        return modelList.Select(MapBedHarvestFieldOpsCosmosDbModelToBedHarvestFieldOpsViewModel).ToList();
    }

    public static BedHarvestFieldOpsViewModel MapBedHarvestFieldOpsCosmosDbModelToBedHarvestFieldOpsViewModel(BedHarvestFieldOpsCosmosDbModel model)
    {
        if (model == null) return new BedHarvestFieldOpsViewModel();

        return new BedHarvestFieldOpsViewModel
        {
            Id = model.Id,
            PartitionKey = model.PartitionKey,

            BedNumber = model.BedNumber,
            Section = model.Section.GetEnum<SectionEnum>(),
            PlantedDate = Utils.ParseToDateTime(model.PlantedDate).GetValueOrDefault(),
            PlantsCount = model.PlantsCount,
            Harvests = model.Harvests.ToList()
            //FieldOperations = model.FieldOperations.ToList()
            //Address = new AddressViewModel()
            //{
            //    Street = company.CustomerCommunicationAddr.Line1,
            //    City = company.CustomerCommunicationAddr.City,
            //    State = company.CustomerCommunicationAddr.CountrySubDivisionCode,
            //    Zip = company.CustomerCommunicationAddr.PostalCode
            //}
        };
    }

    public static List<BedFieldOpsViewModel> MapBedFieldOpsCosmosDbModelToBedFieldOpsViewModel(List<BedFieldOpsCosmosDbModel> modelList)
    {
        return modelList.Select(MapBedFieldOpsCosmosDbModelToBedFieldOpsViewModel).ToList();
    }

    public static BedFieldOpsViewModel MapBedFieldOpsCosmosDbModelToBedFieldOpsViewModel(BedFieldOpsCosmosDbModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.OperationDate) || string.IsNullOrEmpty(model.WorkType)) return new BedFieldOpsViewModel();

        return new BedFieldOpsViewModel
        {
            OperationDate = Utils.ParseToDateTime(model.OperationDate).GetValueOrDefault(),
            WorkType = model.WorkType.GetEnum<FieldOperationEnum>()


            //Address = new AddressViewModel()
            //{
            //    Street = company.CustomerCommunicationAddr.Line1,
            //    City = company.CustomerCommunicationAddr.City,
            //    State = company.CustomerCommunicationAddr.CountrySubDivisionCode,
            //    Zip = company.CustomerCommunicationAddr.PostalCode
            //}
        };
    }
}
using KingsFarms.Core.Api.Builders.LocationSearch;
using KingsFarms.Core.Api.Requests;

namespace KingsFarms.Core.Api.Builders;

public static class FedExShipmentBuilder
{
    //public static FedExCreateShipment? BuildShipment()
    //{
    //    return new FedExCreateShipmentBuilder()
    //        .WithLabelOptions(FedExLabelOptions.URL_ONLY)
    //        .WithRequestedShipment(BuildRequestedShipment())
    //        .WithAccountNumber(BuildShipmentAccountNumber())
    //        .Build();
    //}

    public static FedExCreateShipment? BuildShipment(CreateShipmentRequest? request)
    {
        return new FedExCreateShipmentBuilder()
            .WithLabelOptions(FedExLabelOptions.URL_ONLY)
            .WithRequestedShipment(BuildRequestedShipment(request))
            .WithAccountNumber(BuildShipmentAccountNumber())
            .Build();
    }

    private static FedExRequestedShipment? BuildRequestedShipment(CreateShipmentRequest? request)
    {
        return new FedExRequestedShipmentBuilder()
            .WithShipper(BuildShipmentShipper())
            .WithRecipient(BuildShipmentRecipient(request?.To))
            //.WithShipDate(DateTime.Today.ToString("d"))
            .WithServiceType(FedExServiceType.GROUND)
            .WithPackagingType(FedExPackagingType.YOUR_PACKAGING)
            .WithPickupType(FedExPickupType.PICKUP)
            .WithBlockInsightVisibility(false)
            .WithShippingPayment(BuildShipmentPayment())
            .WithLabelSpecification(BuildShipmentLabelSpecification())
            .WithPackageCount(4)
            .WithPackageItem(BuildShipmentPackageItem())
            .Build();
    }

    private static FedExAccountNumber? BuildShipmentAccountNumber()
    {
        return new FedExAccountNumberBuilder()
            .WithNumber("740561073") //developer
            //519249120 //prod jay
            //673904076 //prod grthummakings
            .Build();
    }

    private static FedExPackageItem BuildShipmentPackageItem()
    {
        return new FedExPackageItemBuilder()
            .WithWeight(new FedExWeightBuilder()
                .WithValue(12)
                .WithUnits("LB")
                .Build())
            //.WithDimension(new FedExDimensionBuilder()
            //    .WithLength(24)
            //    .WithWidth(12)
            //    .WithHeight(14)
            //    .WithUnit("IN")
            //    .Build())
            .Build();
    }

    private static FedExShippingPayment BuildShipmentPayment()
    {
        return new FedExShippingPaymentBuilder()
            .WithPaymentType(FedExPaymentType.SENDER)
            .Build();
    }

    private static FedExLabelSpecification BuildShipmentLabelSpecification()
    {
        return new FedExLabelSpecificationBuilder()
            .WithImageType(FedExImageType.PDF)
            .WithStockType(FedExStockType.PAPER_85x11_TOP_HALF_LABEL)
            .Build();
    }

    private static FedExShipperRecipient BuildShipmentRecipient(ShipmentContactRequest? contact)
    {
        return new FedExShipperRecipientBuilder()
            .WithContact(new FedExContactBuilder()
                .WithPersonName(contact != null ? contact.PersonName : "Jay")
                .WithPhone("15138850368")
                .WithCompanyName(contact != null ? contact.CompanyName : "Some Comp")
                .Build())
            .WithAddress(new FedExAddressBuilder()
                .WithStreetLine1(contact != null ? contact.Street : "4029 Melampy Creek Ln")
                .WithCity(contact != null ? contact.City : "Mason")
                .WithState(contact != null ? contact.State : "OH")
                .WithPostalCode(contact != null ? contact.ZipCode : "45040")
                .WithCountryCode("US")
                .Build())
            .Build();
    }

    private static FedExShipperRecipient BuildShipmentShipper()
    {
        return new FedExShipperRecipientBuilder()
            .WithContact(new FedExContactBuilder()
                .WithPersonName("George Thumma")
                .WithPhone("16096084088")
                .WithCompanyName("Kings Properties")
                .Build())
            .WithAddress(new FedExAddressBuilder()
                .WithStreetLine1("3595 N Kings Hwy")
                .WithCity("Ft Pierce")
                .WithState("FL")
                .WithPostalCode("34951")
                .WithCountryCode("US")
                .Build())
            .Build();
    }
}
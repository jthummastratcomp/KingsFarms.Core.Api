using KingsFarms.Core.Api.Builders.LocationSearch;

namespace KingsFarms.Core.Api.Services;

public static class FedExShipmentBuilder
{
    public static FedExCreateShipment? BuildShipment()
    {
        return new FedExCreateShipmentBuilder()
            .WithLabelOptions(FedExLabelOptions.URL_ONLY)
            .WithRequestedShipment(BuildRequestedShipment())
            .WithAccountNumber(BuildShipmentAccountNumber())
            .Build();
    }

    private static FedExRequestedShipment? BuildRequestedShipment()
    {
        return new FedExRequestedShipmentBuilder()
            .WithShipper(BuildShipmentShipper())
            .WithRecipient(BuildShipmentRecipient())
            //.WithShipDate(DateTime.Today.ToString("d"))
            .WithServiceType(FedExServiceType.GROUND)
            .WithPackagingType(FedExPackagingType.YOUR_PACKAGING)
            .WithPickupType(FedExPickupType.PICKUP)
            .WithBlockInsightVisibility(false)
            .WithShippingPayment(BuildShipmentPayment())
            .WithLabelSpecification(BuildShipmentLabelSpecification())
            .WithPackageCount(1)
            .WithPackageItem(BuildShipmentPackageItem())
            .Build();
    }

    private static FedExAccountNumber? BuildShipmentAccountNumber()
    {
        return new FedExAccountNumberBuilder()
            .WithNumber("740561073")
            .Build();
    }

    private static FedExPackageItem BuildShipmentPackageItem()
    {
        return new FedExPackageItemBuilder()
            .WithWeight(new FedExWeightBuilder()
                .WithValue(10)
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

    private static FedExShipperRecipient BuildShipmentRecipient()
    {
        return new FedExShipperRecipientBuilder()
            .WithContact(new FedExContactBuilder()
                .WithPersonName("Jay")
                .WithPhone("15138850368")
                .WithCompanyName("Some Comp")
                .Build())
            .WithAddress(new FedExAddressBuilder()
                .WithStreetLine1("4029 Melampy Creek Ln")
                .WithCity("Mason")
                .WithState("OH")
                .WithPostalCode("45040")
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
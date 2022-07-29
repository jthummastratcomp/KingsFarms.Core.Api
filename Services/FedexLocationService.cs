using HotTowel.Core.Api.Builders.LocationSearch;
using HotTowel.Core.Api.Services.Endpoints;
using HotTowel.Core.Api.Services.Extensions;
using HotTowel.Core.Api.Services.Interfaces;
using HotTowel.Core.Api.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using ILogger = Serilog.ILogger;

namespace HotTowel.Core.Api.Services;

public class FedexLocationService : IFedexLocationService
{
    private readonly ILogger _logger;
    private readonly IFedexTokenService _tokenService;
    private readonly string _url;

    public FedexLocationService(ILogger logger, IFedexTokenService tokenService, string url)
    {
        _logger = logger;
        _tokenService = tokenService;
        _url = url;
    }

    public List<FedexLocationViewModel> GetLocations()
    {
        var client = new RestClient(new RestClientOptions(_url));
        var request = new RestRequest(FedExEndpoints.FedExLocationsEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(JsonConvert.SerializeObject(new FedExLocationSearchBuilder()
                .WithLocation(new FedExAddressBuilder()
                    .WithCountryCode("US")
                    .WithPostalCode("45040")
                    .Build())
                .Build()));

        var resp = client.Post(request);

        return new List<FedexLocationViewModel> { new() { BedNumber = resp.Content } };
    }

    public List<FedexLocationViewModel> ValidateAddress()
    {
        var client = new RestClient(new RestClientOptions(_url));
        var request = new RestRequest(FedExEndpoints.FedExValidateAddressEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(JsonConvert.SerializeObject(new FedExAddressesToValidateBuilder()
                .WithLocation(new FedExAddressBuilder()
                    .WithCountryCode("US")
                    .WithPostalCode("45040")
                    .WithCity("Cincinnati")
                    .WithStreetLine1("029 Melampy Creek Ln")
                    .Build())
                .Build()));

        var resp = client.Post(request);

        return new List<FedexLocationViewModel> { new() { BedNumber = resp.Content } };
    }

    public List<FedexLocationViewModel> CreateShipment(string? json = null)
    {
        var buildJson = string.IsNullOrEmpty(json) ? JsonConvert.SerializeObject(BuildShipment()) : json;

        var client = new RestClient(new RestClientOptions(_url));
        var request = new RestRequest(FedExEndpoints.FedExCreateShipmentEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(buildJson);

        var resp = client.Post(request);

        return new List<FedexLocationViewModel> { new() { BedNumber = resp.Content } };
    }

    private static FedExCreateShipment? BuildShipment()
    {
        return new FedExCreateShipmentBuilder()
            .WithRequestedShipment(BuildRequestedShipment())
            .WithLabelOptions(FedExLabelOptions.LABEL)
            .WithAccountNumber(BuildShipmentAccountNumber())
            .Build();
    }

    private static FedExRequestedShipment BuildRequestedShipment()
    {
        return new FedExRequestedShipmentBuilder()
            .WithShipper(BuildShipmentShipper())
            .WithRecipient(BuildShipmentRecipient())
            .WithPickupType(FedExPickupType.PICKUP)
            .WithServiceType(FedExServiceType.OVERNIGHT)
            .WithPackagingType(FedExPackagingType.BOX)
            .WithLabelSpecification(BuildShipmentLabelSpecification())
            .WithShippingPayment(BuildShipmentPayment())
            .WithPackageItem(BuildShipmentPackageItem())
            .Build();
    }

    private static FedExAccountNumber BuildShipmentAccountNumber()
    {
        return new FedExAccountNumberBuilder()
            .WithNumber("740561073")
            .Build();
    }

    private static FedExPackageItem BuildShipmentPackageItem()
    {
        return new FedExPackageItemBuilder()
            .WithWeight(new FedExWeightBuilder()
                .WithUnits("KG")
                .WithValue(68.5)
                .Build())
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
            .WithStockType(FedExStockType.LETTER)
            .WithImageType(FedExImageType.PDF)
            .Build();
    }

    private static FedExShipperRecipient BuildShipmentRecipient()
    {
        return new FedExShipperRecipientBuilder()
            .WithAddress(new FedExAddressBuilder()
                .WithStreetLine1("4029 Melampy Creek Ln")
                .WithCity("Mason")
                .WithState("OH")
                .WithPostalCode("45040")
                .WithCountryCode("US")
                .Build())
            .WithContact(new FedExContactBuilder()
                .WithPhone("15138850368")
                .WithPersonName("Jay")
                .Build())
            .Build();
    }

    private static FedExShipperRecipient BuildShipmentShipper()
    {
        return new FedExShipperRecipientBuilder()
            .WithAddress(new FedExAddressBuilder()
                .WithStreetLine1("3595 N Kings Hwy")
                .WithCity("Ft Pierce")
                .WithState("FL")
                .WithPostalCode("34951")
                .WithCountryCode("US")
                .Build())
            .WithContact(new FedExContactBuilder()
                .WithPhone("16096084088")
                .WithPersonName("George")
                .Build())
            .Build();
    }
}
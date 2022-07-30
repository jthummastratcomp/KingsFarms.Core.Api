using KingsFarms.Core.Api.Builders.LocationSearch;
using KingsFarms.Core.Api.Services.Endpoints;
using KingsFarms.Core.Api.Services.Extensions;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Services;

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
        var json = JsonConvert.SerializeObject(new FedExLocationSearchBuilder()
            .WithLocation(new FedExAddressBuilder()
                .WithCountryCode("US")
                .WithPostalCode("45040")
                .Build())
            .Build(), Formatting.Indented);

        var client = new RestClient(new RestClientOptions(_url));
        var request = new RestRequest(FedExEndpoints.FedExLocationsEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(json);

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
                    .WithStreetLine1("4029 Melampy Creek Ln")
                    .Build())
                .Build()));

        var resp = client.Post(request);

        return new List<FedexLocationViewModel> { new() { BedNumber = resp.Content } };
    }

    public CreateShipmentResponse? CreateShipment(string? json = null)
    {
        var buildJson = string.IsNullOrEmpty(json) ? JsonConvert.SerializeObject(FedExShipmentBuilder.BuildShipment()) : json;

        var client = new RestClient(new RestClientOptions(_url));
        var request = new RestRequest(FedExEndpoints.FedExCreateShipmentEndPoint)
            .AddHeaderAuthorization(_tokenService.GetAccessToken())
            .AddHeaderLocale()
            .AddHeaderContentTypeJson()
            .AddBody(buildJson);

        var resp = client.Post(request);

        if (resp.Content != null)
        {
            var des = JsonConvert.DeserializeObject<CreateShipmentResponse>(resp.Content);
            return des;
        }

        return null;
    }

    
}

public class CreateShipmentResponse
{
    [JsonProperty("transactionId")] public string? TransId { get; set; }
    [JsonProperty("output")] public CreateShipmentResponseOutput? Output { get; set; }
    
}

public class CreateShipmentResponseOutput
{
    [JsonProperty("transactionShipments")] public List<TransactionShipment>? Shipments { get; set; }
    [JsonProperty("alerts")] public List<ShipmentAlert>? Alerts { get; set; }
}

public class ShipmentAlert
{
    [JsonProperty("code")] public string? Code { get; init; }
    [JsonProperty("alertType")] public string? AlertType { get; init; }
    [JsonProperty("message")] public string? Message { get; init; }
}

public class TransactionShipment
{
    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
    [JsonProperty("serviceName")] public string? ServiceName { get; init; }
    [JsonProperty("shipDatestamp")] public string? ShipDate { get; init; }
    [JsonProperty("serviceCategory")] public string? ServiceCategory { get; init; }
    [JsonProperty("masterTrackingNumber")] public string? TrackingNumberMaster { get; init; }
    [JsonProperty("alerts")] public List<ShipmentAlert>? Alerts { get; set; }
    [JsonProperty("pieceResponses")] public List<PieceResponse>? PieceResponses { get; init; }
    //[JsonProperty("shipmentDocuments")] public List<ShipmentDocuments>? ShipmentDocuments { get; init; }
    //[JsonProperty("packageDocuments")] public List<ShipmentDocuments>? PackageDocuments { get; init; }
    [JsonProperty("completedShipmentDetail")] public CompletedShipmentDetail? CompletedShipmentDetail { get; init; }
    //[JsonProperty("shipmentAdvisoryDetails")] public ShipmentAdvisoryDetail? ShipmentAdvisoryDetail { get; init; }

}

//public class ShipmentAdvisoryDetail
//{
//    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
//    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
//    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
//    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
//    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
//    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
//}

public class CompletedShipmentDetail
{
    [JsonProperty("carrierCode")] public string? carrierCode { get; init; }
    [JsonProperty("packagingDescription")] public string? packagingDescription { get; init; }
    [JsonProperty("usDomestic")] public bool? usDomestic { get; init; }
    [JsonProperty("exportComplianceStatement")] public string? exportComplianceStatement { get; init; }
    [JsonProperty("serviceType")] public string? ServiceType { get; init; }

    [JsonProperty("masterTrackingId")] public TrackingId? masterTrackingId { get; init; }
    [JsonProperty("serviceDescription")] public ServiceDescription? serviceDescription { get; init; }
    [JsonProperty("operationalDetail")] public OperationalDetail? operationalDetail { get; init; }
    [JsonProperty("shipmentRating")] public ShipmentRating? shipmentRating { get; init; }
    [JsonProperty("completedPackageDetails")] public List<CompletedPackageDetails>? completedPackageDetails { get; init; }

}

public class CompletedPackageDetails
{
    [JsonProperty("sequenceNumber")] public int? sequenceNumber { get; init; }
    [JsonProperty("trackingIds")] public List<TrackingId>? trackingIds { get; init; }
    [JsonProperty("groupNumber")] public int? groupNumber { get; init; }
    [JsonProperty("packageRating")] public PackageRating? packageRating { get; init; }
    [JsonProperty("signatureOption")] public string? signatureOption { get; init; }
    [JsonProperty("operationalDetail")] public OperationalDetail? operationalDetail { get; init; }
}

public class PackageRating
{
    [JsonProperty("actualRateType")] public string? actualRateType { get; init; }
    [JsonProperty("effectiveNetDiscount")] public double? effectiveNetDiscount { get; init; }
    [JsonProperty("packageRateDetails")] public List<PackageRateDetails>? packageRateDetails { get; init; }
}

public class PackageRateDetails
{
    [JsonProperty("rateType")] public string? rateType { get; init; }
    [JsonProperty("ratedWeightMethod")] public string? ratedWeightMethod { get; init; }
    [JsonProperty("minimumChargeType")] public string? minimumChargeType { get; init; }
    [JsonProperty("billingWeight")] public FedExWeight? billingWeight { get; init; }
    [JsonProperty("baseCharge")] public double? baseCharge { get; init; }
    [JsonProperty("totalFreightDiscounts")] public double? totalFreightDiscounts { get; init; }
    [JsonProperty("netFreight")] public double? netFreight { get; init; }
    [JsonProperty("totalSurcharges")] public double? totalSurcharges { get; init; }
    [JsonProperty("netFedExCharge")] public double? netFedExCharge { get; init; }
    [JsonProperty("totalTaxes")] public double? totalTaxes { get; init; }
    [JsonProperty("netCharge")] public double? netCharge { get; init; }
    [JsonProperty("totalRebates")] public double? totalRebates { get; init; }
    [JsonProperty("surcharges")] public List<Surcharges>? surcharges { get; init; }
    [JsonProperty("currency")] public string? currency { get; init; }
}

public class Surcharges
{
    [JsonProperty("surchargeType")] public string? surchargeType { get; init; }
    [JsonProperty("level")] public string? level { get; init; }
    [JsonProperty("description")] public string? description { get; init; }
    [JsonProperty("amount")] public double? amount { get; init; }
}

public class ShipmentRating
{
    [JsonProperty("actualRateType")] public string? actualRateType { get; init; }
    [JsonProperty("shipmentRateDetails")] public List<ShipmentRateDetails>? shipmentRateDetails { get; init; }
}

public class ShipmentRateDetails
{
    [JsonProperty("rateType")] public string? rateType { get; init; }
    [JsonProperty("rateScale")] public string? rateScale { get; init; }
    [JsonProperty("rateZone")] public string? rateZone { get; init; }
    [JsonProperty("ratedWeightMethod")] public string? ratedWeightMethod { get; init; }
    [JsonProperty("dimDivisor")] public int? dimDivisor { get; init; }
    [JsonProperty("fuelSurchargePercent")] public double? fuelSurchargePercent { get; init; }
    [JsonProperty("totalBillingWeight")] public FedExWeight? totalBillingWeight { get; init; }
    [JsonProperty("totalBaseCharge")] public double? totalBaseCharge { get; init; }
    [JsonProperty("totalFreightDiscounts")] public double? totalFreightDiscounts { get; init; }
    [JsonProperty("totalNetFreight")] public double? totalNetFreight { get; init; }
    [JsonProperty("totalSurcharges")] public double? totalSurcharges { get; init; }
    [JsonProperty("totalNetFedExCharge")] public double? totalNetFedExCharge { get; init; }
    [JsonProperty("totalTaxes")] public double? totalTaxes { get; init; }
    [JsonProperty("totalNetCharge")] public double? totalNetCharge { get; init; }
    [JsonProperty("totalRebates")] public double? totalRebates { get; init; }
    [JsonProperty("totalDutiesAndTaxes")] public double? totalDutiesAndTaxes { get; init; }
    [JsonProperty("totalAncillaryFeesAndTaxes")] public double? totalAncillaryFeesAndTaxes { get; init; }
    [JsonProperty("totalDutiesTaxesAndFees")] public double? totalDutiesTaxesAndFees { get; init; }
    [JsonProperty("totalNetChargeWithDutiesAndTaxes")] public double? totalNetChargeWithDutiesAndTaxes { get; init; }
    [JsonProperty("surcharges")] public List<Surcharges>? surcharges { get; init; }
    [JsonProperty("freightDiscounts")] public List<FreightDiscount>? freightDiscounts { get; init; }
    [JsonProperty("taxes")] public List<Taxes>? taxes { get; init; }
    [JsonProperty("currency")] public string? currency { get; init; }
}

public class Taxes
{
}

public class FreightDiscount
{
}

public class OperationalDetail
{
    [JsonProperty("barcodes")] public Barcodes? barcodes { get; init; }
    [JsonProperty("astraHandlingText")] public string? astraHandlingText { get; init; }
    [JsonProperty("operationalInstructions")] public List<OperationalInstruction>? operationalInstructions { get; init; }

}

public class Barcodes
{
        [JsonProperty("binaryBarcodes")] public List<Barcode>? binaryBarcodes { get; init; }
        [JsonProperty("stringBarcodes")] public List<Barcode>? stringBarcodes { get; init; }
}

public class Barcode
{
    [JsonProperty("type")] public string? type { get; init; }
    [JsonProperty("value")] public string? value { get; init; }
}

public class OperationalInstruction
{
    [JsonProperty("number")] public int? number { get; init; }
    [JsonProperty("content")] public string? content { get; init; }
}

public class ServiceDescription
{
    //    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
    //    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
    //    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
    //    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
    //    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
    //    [JsonProperty("serviceType")] public string? ServiceType { get; init; }
}

public class TrackingId
{
    [JsonProperty("trackingIdType")] public string? trackingIdType { get; init; }
    [JsonProperty("trackingNumber")] public string? trackingNumber { get; init; }
}

public class ShipmentDocuments
{
    [JsonProperty("contentKey")] public string? ContentKey { get; init; }
    [JsonProperty("copiesToPrint")] public int CopiesToPrint { get; init; }
    [JsonProperty("contentType")] public string? contentType { get; init; }
    [JsonProperty("trackingNumber")] public string? trackingNumber { get; init; }
    [JsonProperty("docType")] public string? docType { get; init; }
    [JsonProperty("encodedLabel")] public string? encodedLabel { get; init; }
    [JsonProperty("url")] public string? url { get; init; }
    [JsonProperty("alerts")] public List<ShipmentAlert>? Alerts { get; set; }
}

public class PieceResponse  
{
    [JsonProperty("netChargeAmount")] public double? netChargeAmount { get; init; }
    [JsonProperty("acceptanceTrackingNumber")] public string? acceptanceTrackingNumber { get; init; }
    [JsonProperty("serviceCategory")] public string? serviceCategory { get; init; }
    [JsonProperty("listCustomerTotalCharge")] public string? listCustomerTotalCharge { get; init; }
    [JsonProperty("deliveryTimestamp")] public string? deliveryTimestamp { get; init; }
    [JsonProperty("trackingIdType")] public string? trackingIdType { get; init; }
    [JsonProperty("additionalChargesDiscount")] public double? additionalChargesDiscount { get; init; }
    [JsonProperty("netListRateAmount")] public double? netListRateAmount { get; init; }
    [JsonProperty("baseRateAmount")] public double? baseRateAmount { get; init; }
    [JsonProperty("packageSequenceNumber")] public double? packageSequenceNumber { get; init; }
    [JsonProperty("netDiscountAmount")] public double? netDiscountAmount { get; init; }
    [JsonProperty("codcollectionAmount")] public double? codcollectionAmount { get; init; }
    [JsonProperty("acceptanceType")] public string? acceptanceType { get; init; }
    [JsonProperty("successful")] public bool? successful { get; init; }
    [JsonProperty("masterTrackingNumber")] public string? TrackingNumberMaster { get; init; }
    [JsonProperty("customerReferences")] public List<CustomerReferences>? customerReferences { get; init; }
    [JsonProperty("packageDocuments")] public List<PackageDocuments>? packageDocuments { get; init; }

}

public class PackageDocuments
{
    [JsonProperty("url")] public string? url { get; init; }
    [JsonProperty("contentType")] public string? contentType { get; init; }
    [JsonProperty("copiesToPrint")] public int? copiesToPrint { get; init; }
    [JsonProperty("docType")] public string? docType { get; init; }
}

public class CustomerReferences
{
    [JsonProperty("customerReferenceType")] public string? customerReferenceType { get; init; }
    [JsonProperty("value")] public string? value { get; init; }
}
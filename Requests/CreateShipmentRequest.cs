namespace KingsFarms.Core.Api.Requests;

public class CreateShipmentRequest
{
    public string? FedExAccountNumber { get; init; }
    public ShipmentContactRequest? From { get; init; }
    public ShipmentContactRequest? To { get; init; }
    public int? BoxCount { get; init; }
    public int? Weight { get; init; }
}

public class ShipmentContactRequest
{
    public string? Phone { get; init; }
    public string? PersonName { get; set; }
    public string? CompanyName { get; set; }

    public string? ZipCode { get; init; }

    //public string? CountryCode { get; init; } = "US";
    public string? City { get; init; }
    public string? State { get; init; }
    public string? Street { get; init; }
}
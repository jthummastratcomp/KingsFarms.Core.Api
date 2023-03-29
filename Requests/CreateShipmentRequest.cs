namespace KingsFarms.Core.Api.Requests;

public class CreateShipmentRequest
{
    public string? FedExAccountNumber { get; init; }
    public ShipmentContactRequest? From { get; init; }
    public ShipmentContactRequest? To { get; init; }
    public int? BoxCount { get; init; }
    public int? Weight { get; init; }
    public DateTime ShipDate { get; set; } = DateTime.Today;
}
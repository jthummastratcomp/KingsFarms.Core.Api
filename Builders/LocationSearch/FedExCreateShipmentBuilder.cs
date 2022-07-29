namespace HotTowel.Core.Api.Builders.LocationSearch;

public class FedExCreateShipmentBuilder
{
    private FedExAccountNumber _accountNumber;
    private string _labelOptions;
    private FedExRequestedShipment _requestedShipment;


    public FedExCreateShipmentBuilder WithRequestedShipment(FedExRequestedShipment requestedShipment)
    {
        _requestedShipment = requestedShipment;
        return this;
    }

    public FedExCreateShipmentBuilder WithLabelOptions(string labelOptions)
    {
        _labelOptions = labelOptions;
        return this;
    }

    public FedExCreateShipmentBuilder WithAccountNumber(FedExAccountNumber accountNumber)
    {
        _accountNumber = accountNumber;
        return this;
    }

    public FedExCreateShipment Build()
    {
        return new FedExCreateShipment
        {
            RequestedShipment = _requestedShipment,
            LabelOptions = _labelOptions,
            AccountNumber = _accountNumber
        };
    }
}
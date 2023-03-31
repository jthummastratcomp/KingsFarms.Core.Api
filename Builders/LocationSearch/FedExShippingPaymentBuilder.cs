namespace KingsFarms.Core.Api.Builders.LocationSearch;

public class FedExShippingPaymentBuilder
{
    private string? _paymentType;

    public FedExShippingPaymentBuilder WithPaymentType(string? value)
    {
        _paymentType = value;
        return this;
    }

    public FedExShippingPayment Build()
    {
        return new FedExShippingPayment {PaymentType = _paymentType};
    }
}
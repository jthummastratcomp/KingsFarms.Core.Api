using KingsFarms.Core.Api.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace KingsFarms.Core.Api.Services;

public class MessagingService : IMessagingService
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromSmsPhone;
    private readonly string _toSmsPhone;
    private readonly bool _useRealToPhone;

    public MessagingService(string accountSid, string authToken, string fromSmsPhone, bool useRealToPhone, string toSmsPhone)
    {
        _accountSid = accountSid;
        _authToken = authToken;
        _fromSmsPhone = fromSmsPhone;
        _useRealToPhone = useRealToPhone;
        _toSmsPhone = toSmsPhone;
    }

    public string SendSms(string message, string toPhone)
    {
        // Find your Account SID and Auth Token at twilio.com/console
        // and set the environment variables. See http://twil.io/secure
        //string accountSid = ConfigurationManager.AppSettings["TWILIO_ACCOUNT_SID"];
        //string authToken = ConfigurationManager.AppSettings["TWILIO_AUTH_TOKEN"];
        //string fromSMSPhone = ConfigurationManager.AppSettings["TWILIO_SMS_FROM"];
        //bool useRealToPhone = Utils.ParseToBoolean(ConfigurationManager.AppSettings["TWILIO_SMS_TO_Real"]);

        var toSmsPhone = _useRealToPhone ? toPhone : _toSmsPhone;

        TwilioClient.Init(_accountSid, _authToken);

        MessageResource.Create(
            body: $"{message}. Msg from Kings Sunshine Farms, FL" ,
            from: new PhoneNumber(_fromSmsPhone),
            to: new PhoneNumber(toSmsPhone)
        );

        return $"SMS sent to {toSmsPhone}";
    }
}
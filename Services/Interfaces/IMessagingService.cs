namespace KingsFarms.Core.Api.Services.Interfaces;

public interface IMessagingService
{
    string SendSms(string message, string toPhone);
}
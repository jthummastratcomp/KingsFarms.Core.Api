using KingsFarms.Core.Api.Helpers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.Controllers;

[ApiController]
public class SmsMessagingController : ControllerBase
{
    private readonly IMessagingService _messagingService;


    public SmsMessagingController(IMessagingService messagingService)
    {
        _messagingService = messagingService;
    }

    [HttpPost]
    [Route(CoreApiRoutes.SendCustomerInvoiceSms)]
    public string SendCustomerInvoiceSms(MessagingViewModel? viewModel)
    {
        var result = "Unable to send SMS";
        if (viewModel == null || string.IsNullOrEmpty(viewModel.Phone)) return result;

        if (viewModel.DueDate == DateTime.MinValue
            || string.IsNullOrEmpty(viewModel.InvoiceNumber)
            || viewModel.Balance == decimal.Zero) return result;

        //var link = "https://connect.intuit.com/t/scs-v1-2d6fce6fc6b6411c80ba924efd448af4cb80c85f71ef4182bad5988a6bf2976f0f50eaf72c2e43559e9ef309b3d7fe97?locale=en_US";

        var message = string.Format(@$"Your invoice# {viewModel.InvoiceNumber} dated {viewModel.DueDate:MM/dd/yyyy} is due. 
Please pay {viewModel.BalanceDisplay} at your earliest. {viewModel.PaymentLink}");

        result = _messagingService.SendSms(message, viewModel.Phone);

        return $"{result} {message}";
    }

    [HttpPost]
    [Route(CoreApiRoutes.SendCustomerInvoiceBulkYearSms)]
    public string SendCustomerInvoiceBulkYearSms(List<MessagingViewModel> viewModels)
    {
        var result = "Unable to send SMS";
        if (!Utils.HasRows(viewModels)) return result;

        var phoneNumber = viewModels.Where(x => !string.IsNullOrEmpty(x.Phone)).Select(x => x.Phone).Distinct().FirstOrDefault();
        if (string.IsNullOrEmpty(phoneNumber)) return result;

        var years = viewModels.Where(x => !string.IsNullOrEmpty(x.DueDateDisplay)).Select(x => x.DueDate.Year).Distinct().ToList();
        var yearsConcat = string.Join(',', years);

        var invoicesCount = viewModels.Count();
        var invoicesTotalBalance = viewModels.Sum(x => x.Balance);

        //var link = "https://connect.intuit.com/t/scs-v1-2d6fce6fc6b6411c80ba924efd448af4cb80c85f71ef4182bad5988a6bf2976f0f50eaf72c2e43559e9ef309b3d7fe97?locale=en_US";

        var message = string.Format(@$"You have {invoicesCount} invoices due in {yearsConcat} for total amount {invoicesTotalBalance:C0}. 
Please pay at your earliest.");

        result = _messagingService.SendSms(message, phoneNumber);

        return $"{result} {message}";
    }
}
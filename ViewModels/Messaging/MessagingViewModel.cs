namespace KingsFarms.Core.Api.ViewModels.Messaging;

public class MessagingViewModel
{
    public string? InvoiceNumber { get; set; } = string.Empty;

    public string? Phone { get; set; } = string.Empty;

    public string DueDateDisplay => DueDate.ToString("MM/dd/yyyy");
    public DateTime DueDate { get; set; }

    public decimal Balance { get; set; } = decimal.Zero;
    public string BalanceDisplay => Balance.ToString("C0");

    public string? PaymentLink { get; set; } = string.Empty;
}
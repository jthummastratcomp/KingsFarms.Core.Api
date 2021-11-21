public class WeeklyOrdersService : IWeeklyOrdersService
{
    public List<SearchDto> GetInvoiceWeeksListForYear()
    {
        return Utils.GetWeeksOfYear(DateTime.Today.Year);
    }
}
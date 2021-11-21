public static class Utils
{
    public static string ParseToDateTimeYearMonthDay(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }

    public static DateTime GetFirstMondayOfYear(int year)
    {
        var dateTime = new DateTime(year - 1, 12, 30);
        return dateTime.AddDays(8 - (int)dateTime.DayOfWeek);
    }

    public static List<SearchDto> GetWeeksOfYear(int year)
    {
        var weeksList = new List<SearchDto>();

        var firstMonday = GetFirstMondayOfYear(year);
        var weekNumber = 1;
        var nextMonday = AddToWeeksList(weeksList, weekNumber, firstMonday);
        while (nextMonday.Year == DateTime.Today.Year)
        {
            weekNumber += 1;
            nextMonday = AddToWeeksList(weeksList, weekNumber, nextMonday);
        }
        return weeksList;
    }

    private static DateTime AddToWeeksList(ICollection<SearchDto> weeksList, int weekNumber, DateTime monday)
    {
        var date = ParseToDateTimeYearMonthDay(monday);

        weeksList.Add(new SearchDto { Id = $"Week {weekNumber}", Data = date });
        return monday.AddDays(7);
    }
}
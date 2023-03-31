using System.Collections;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using KingsFarms.Core.Api.ViewModels;

namespace KingsFarms.Core.Api.Helpers;

public static class Utils
{
    public static bool HasRows<T>(ICollection<T> list)
    {
        return list != null && list.Count != 0;
    }

    public static int GetListHash(IList list)
    {
        return list.Cast<object>().Aggregate(19, (current, foo) => current * 31 + foo.GetHashCode());
    }

    public static int ParseToInteger(string? integerValue)
    {
        if (!string.IsNullOrEmpty(integerValue) && int.TryParse(integerValue, out var value)) return value;
        return 0;
    }

    public static int? ParseToIntegerNullable(string integerValue)
    {
        if (!string.IsNullOrEmpty(integerValue) && int.TryParse(integerValue, out var value)) return value;
        return null;
    }

    public static long ParseToLong(string longValue)
    {
        if (!string.IsNullOrEmpty(longValue) && long.TryParse(longValue, out var value)) return value;
        return 0;
    }

    public static decimal ParseToDecimal(string? decimalValue)
    {
        if (!string.IsNullOrEmpty(decimalValue) && decimal.TryParse(decimalValue, out var value)) return value;
        return 0;
    }

    public static decimal? ParseToDecimalNullable(string decimalValue)
    {
        if (!string.IsNullOrEmpty(decimalValue) && decimal.TryParse(decimalValue.Replace("$", string.Empty), out var value)) return value;
        return null;
    }

    public static double ParseToDouble(string doubleValue)
    {
        if (!string.IsNullOrEmpty(doubleValue) && double.TryParse(doubleValue, out var value)) return value;
        return 0;
    }

    public static double? ParseToDoubleNullable(string doubleValue)
    {
        if (!string.IsNullOrEmpty(doubleValue) && double.TryParse(doubleValue, out var value)) return value;
        return null;
    }

    public static bool ParseToBoolean(string boolValue)
    {
        if (!string.IsNullOrEmpty(boolValue) && bool.TryParse(boolValue, out var value)) return value;
        if (!string.IsNullOrEmpty(boolValue) && boolValue == "1") return true;
        if (!string.IsNullOrEmpty(boolValue) && boolValue.ToLower() == "true") return true;
        return !string.IsNullOrEmpty(boolValue) && boolValue.ToLower() == "y";
    }

    public static DateTime? ParseToDateTime(string? dateTimeValue)
    {
        if (!string.IsNullOrEmpty(dateTimeValue) && DateTime.TryParse(dateTimeValue, out var value)) return value;
        return null;
    }

    public static TimeSpan? ParseToTimeSpan(string timeValue)
    {
        if (!string.IsNullOrEmpty(timeValue) && TimeSpan.TryParse(timeValue, out var value)) return value;
        return null;
    }

    public static string SanitizeSql(string sql)
    {
        return sql.Replace("'", "''");
    }

    public static string FlipName(string ldapDisplayName)
    {
        if (string.IsNullOrEmpty(ldapDisplayName)) return string.Empty;

        var names = ldapDisplayName.Split(',');
        if (names.Length != 2) return ldapDisplayName;

        var lastName = names[0].Trim();
        var firstName = names[1].Trim();

        return $"{firstName} {lastName}";
    }

    public static string GetFormattedNumber(string number)
    {
        if (string.IsNullOrEmpty(number)) return null;

        number = number.Trim().ToLower();

        number = number.Replace(" ", string.Empty);

        number = number.Replace(@"\t", string.Empty);

        number = number.StartsWith("1 ") ? number.Substring(1, number.Length - 1) : number;

        number = number.StartsWith("1-") ? number.Substring(1, number.Length - 1) : number;

        number = number.Replace("-", string.Empty);

        number = Regex.Replace(number, @"[/!#\\$~`!^,._&' *()=+|}{;><:?%]", string.Empty);

        number = number.Replace("extension", "x").Replace("ext", "x").Replace("xt", "x").Replace("ex", "x");

        var invalidwords = new[] {"direct", "phone", "cell", "fax", "phn", "cel", "ce", "fx", "ph", "c", "f", "m", "\r", "\n", "\t"};

        number = invalidwords.Aggregate(number, (current, word) => current.Contains(word) ? current.Replace(word, string.Empty) : current);

        if (!BigInteger.TryParse(number, out var value)) return number;

        if (value.ToString().Length == 11 && value.ToString().StartsWith("1"))
            number = number.Substring(1, number.Length - 1);

        if (value.ToString().Length > 10)
            number = number.Substring(0, 10);

        return number;
    }

    public static string ParseToDateTimeShort(string? dateTimeValue)
    {
        var value = ParseToDateTime(dateTimeValue);
        return value?.ToShortDateString() ?? null;
    }

    public static string ParseToDateTimeYearMonthDay(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }

    public static string ParseToDateTimeYearMonthDay(string? dateTimeValue)
    {
        var value = ParseToDateTime(dateTimeValue);
        return value == null ? string.Empty : ParseToDateTimeYearMonthDay(value.Value);
    }

    public static string MakeAcronym(string input)
    {
        var chars = input.Where(char.IsUpper).ToArray();
        return new string(chars);
    }

    public static string SanitizeWord(string input, bool retainCase)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        input = input.Trim();

        if (!retainCase) input = input.Trim().ToLower();

        input = input.Replace(" ", string.Empty);

        input = input.Replace("\"", string.Empty);

        input = input.Replace("-", string.Empty);

        input = Regex.Replace(input, @"[/!#\\$~`!^,._&' *()=+|}{;><:?%]", string.Empty);

        return input;
    }

    public static string FormatPhoneNumber(string number)
    {
        //try
        //{
        if (long.TryParse(number, out _)) return Convert.ToInt64(number).ToString("(###) ###-####");
        if (number.Length < 10) return number;

        var first10digits = Regex.Replace(number.Substring(0, 10), "[^0-9.]", "0");
        var after10digits = number.Substring(10);
        after10digits = after10digits.Replace("x", string.Empty);
        return Convert.ToInt64(first10digits).ToString("(###) ###-####") + " ext " + after10digits;
        //}
        //catch
        //{
        //    return string.Empty;
        //}
    }

    public static DateTime GetFirstMondayOfYear(int year)
    {
        var dateTime = new DateTime(year - 1, 12, 30);
        return dateTime.AddDays(8 - (int) dateTime.DayOfWeek);
    }

    public static DateTime GetFirstSaturdayOfYear(int year)
    {
        //var dateTime = new DateTime(year - 1, 12, 30);
        //return dateTime.AddDays(3 - (int)dateTime.DayOfWeek);

        //var dt = new DateTime(year, 1, 1);
        //for (var i = 0; i < 7; i++)
        //    if (dt.DayOfWeek == DayOfWeek.Saturday)
        //        break;

        //return dt.AddDays(1);

        var day = 0;
        while (new DateTime(year, 1, ++day).DayOfWeek != DayOfWeek.Saturday) ;

        return new DateTime(year, 1, day);
    }

    public static int GetWeekOfYear(DateTime invoiceDate)
    {
        var cultureInfo = new CultureInfo("en-US");
        var calendar = cultureInfo.Calendar;
        var calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
        //var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        var firstDayOfWeek = DayOfWeek.Monday;

        return calendar.GetWeekOfYear(invoiceDate, calendarWeekRule, firstDayOfWeek);
    }

    public static List<SearchDto> GetWeeksOfYear(int year)
    {
        var weeksList = new List<SearchDto>();

        var firstMonday = GetFirstMondayOfYear(year);
        firstMonday = firstMonday.AddDays(-2); //TODO: need to properly start with either Saturday or Monday based on year
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

        weeksList.Add(new SearchDto {Id = $"Week {weekNumber}", Data = date});
        return monday.AddDays(7);
    }
}
namespace ShabbosAlarm.Extensions;

internal static class JewishDateExtensions
{
    public static DateTime Today => DateTime.Today.ToJewishDate();

    public static DateTime ToJewishDate(this DateTime date)
    {
        var hc = new HebrewCalendar();
        return new(hc.GetYear(date), hc.GetMonth(date), hc.GetDayOfMonth(date), hc);
    }
}

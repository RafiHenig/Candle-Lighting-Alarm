namespace ShabbosAlarm.Extensions;

internal static class JewishDateExtensions
{
    public static DateTime JewishToday
    {
        get
        {
            var today = DateTime.Today;
            var hc = new HebrewCalendar();
            return new(hc.GetYear(today), hc.GetMonth(today), hc.GetDayOfMonth(today), hc);
        }
    }

    public static DateTime ToJewishDate(this DateTime date)
    {
        var hc = new HebrewCalendar();
        return new(hc.GetYear(date), hc.GetMonth(date), hc.GetDayOfMonth(date), hc);
    }
}

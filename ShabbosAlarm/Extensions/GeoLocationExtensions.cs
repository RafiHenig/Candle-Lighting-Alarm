namespace ShabbosAlarm.Extensions;

internal static class GeoLocationExtensions
{
    private const string LocationName = "Spring Valley, NY";
    private const double Latitude = 41.146783; //Spring Valley, NY
    private const double Longitude = -74.061188; //Spring Valley, NY
    private const double Elevation = 0; //optional elevation
    private const string TimeZone = "America/New_York";

    internal static GeoLocation SpringVally()
    {
        ITimeZone timeZone = new WindowsTimeZone(TimeZone);
        return new GeoLocation(LocationName, Latitude, Longitude, Elevation, timeZone);
    }
}

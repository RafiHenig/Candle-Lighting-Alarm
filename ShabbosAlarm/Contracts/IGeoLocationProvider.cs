namespace ShabbosAlarm.Contracts;

internal interface IGeoLocationProvider
{
    public string LocationName { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public double Elevation { get; }
    public string TimeZone { get; }
    public GeoLocation GeoLocation { get; }
}
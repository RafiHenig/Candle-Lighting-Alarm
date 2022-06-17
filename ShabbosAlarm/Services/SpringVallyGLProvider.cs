namespace ShabbosAlarm.Services;

internal class SpringVallyGLProvider : IGeoLocationProvider
{
    private readonly ILogger _logger;

    public string LocationName => "Spring Valley, NY";
    public double Latitude => 41.146783; //Spring Valley, NY
    public double Longitude => -74.061188; //Spring Valley, NY
    public double Elevation => 0; //optional elevation
    public string TimeZone => "America/New_York";

    public SpringVallyGLProvider(ILogger<SpringVallyGLProvider> logger) => _logger = logger;

    public GeoLocation GeoLocation
    {
        get
        {
            var timeZone = new WindowsTimeZone(TimeZone);
            var geoLocation = new GeoLocation(LocationName, Latitude, Longitude, Elevation, timeZone);

            _logger.LogInformation(geoLocation.ToString());

            return geoLocation;
        }
    }
}
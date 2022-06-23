namespace ShabbosAlarm.Jobs;

public class DailyCheckJob : IJob
{
    private readonly ILogger _logger;
    private readonly IPlayService _playService;
    private readonly ComplexZmanimCalendar _zmanimCalendar;
    private readonly JewishCalendar _jewishCalendar;
    private DateTime _lastPlayed;

    private bool HasPlayedYesterday => DateTime.Today == _lastPlayed.AddDays(1);
    private DateTime JewishDate => DateTime.Today.ToJewishDate();

    public DailyCheckJob(
        ILogger<DailyCheckJob> logger, 
        ComplexZmanimCalendar complexZmanimCalendar,
        JewishCalendar jewishCalendar,
        IPlayService playService
        )
    {
        _logger = logger;
        _zmanimCalendar = complexZmanimCalendar;
        _jewishCalendar = jewishCalendar;
        _playService = playService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Checking whether today is Erev YomTov or Shabbos...");

        if ((_jewishCalendar.IsErevYomTov(JewishDate, false) || _jewishCalendar.GetDayOfWeek(JewishDate) == DayOfWeek.Friday) && !HasPlayedYesterday)
        {
            _logger.LogInformation("Today is Erev YomTov or Shabbos");

            _lastPlayed = DateTime.Today;

            var candleLighting = DateTime.SpecifyKind(_zmanimCalendar.GetCandleLighting()!.Value, DateTimeKind.Local);

            _logger.LogInformation($"Candle lighting time: {candleLighting:hh:mm:ss}");
            _logger.LogInformation($"Playlist durition: {_playService.Durition:hh\\:mm\\:s}");
            _logger.LogInformation("Schedualing an alarm...");

            var playTime = candleLighting.Subtract(_playService.Durition);

            await SchedualePlayJob(context.Scheduler, playTime);
        }
        else _logger.LogInformation("Today is not Erev YomTov or Shabbos");
    }

    private async Task SchedualePlayJob(IScheduler scheduler, DateTime playTime)
    {
        await scheduler.ScheduleJob(
              JobBuilder.Create<PlayJob>().Build(),
              TriggerBuilder.Create().StartAt(playTime).Build()
        );

        _logger.LogInformation($"Schedualed an alarm for {playTime:hh:mm:ss}");
    }
}
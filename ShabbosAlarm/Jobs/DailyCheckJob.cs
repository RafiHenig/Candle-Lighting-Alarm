namespace ShabbosAlarm.Jobs;

public class DailyCheckJob : IJob
{
    private readonly ILogger _logger;
    private readonly ComplexZmanimCalendar _zmanimCalendar;
    private readonly JewishCalendar _jewishCalendar;
    private DateTime _lastPlayed;

    private bool HasPlayedYesterday => DateTime.Today == _lastPlayed.AddDays(1);
    private DateTime JewishDate => DateTime.Today.ToJewishDate();

    public DailyCheckJob(ILogger<DailyCheckJob> logger, ComplexZmanimCalendar complexZmanimCalendar, JewishCalendar jewishCalendar)
    {
        _logger = logger;
        _zmanimCalendar = complexZmanimCalendar;
        _jewishCalendar = jewishCalendar;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Checking whether today is Erev YomTov or Shabbos...");

        if ((_jewishCalendar.IsErevYomTov(JewishDate, false) || _jewishCalendar.GetDayOfWeek(JewishDate) == DayOfWeek.Friday) && !HasPlayedYesterday)
        {
            _logger.LogInformation("Today is Erev YomTov or Shabbos, schedualing an alarm...");

            _lastPlayed = DateTime.Today;

            var playTime = DateTime.SpecifyKind(_zmanimCalendar.GetCandleLighting()!.Value.AddMinutes(-20), DateTimeKind.Local);

            await context.Scheduler.ScheduleJob(
                JobBuilder.Create<PlayJob>().Build(),
                TriggerBuilder.Create().StartAt(playTime).Build()
            );

            _logger.LogInformation($"Schedualed an alarm for {playTime:hh:mm:ss}");
        }
        else _logger.LogInformation("Today is not Erev YomTov or Shabbos");
    }
}
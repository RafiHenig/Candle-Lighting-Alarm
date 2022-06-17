namespace ShabbosAlarm.Jobs;

public class DailyCheckJob : IJob
{
    private DateTime lastRun;
    private bool DidntRunYesterday => DateTime.Today == lastRun.AddDays(1);

    public async Task Execute(IJobExecutionContext context)
    {
        var zmanimCalendar = new ComplexZmanimCalendar(GeoLocationExtensions.SpringVally());
        var jewishCalendar = new JewishCalendar();
        var todayJewishDate = DateTime.Today.ToJewishDate();

        if (jewishCalendar.IsErevYomTov(todayJewishDate, false) || jewishCalendar.GetDayOfWeek(todayJewishDate) == DayOfWeek.Friday && DidntRunYesterday)
        {
            lastRun = DateTime.Today;

            var playTime = zmanimCalendar.GetCandleLighting()!.Value.AddMinutes(-20);

            await context.Scheduler.ScheduleJob(
                JobBuilder.Create<PlayJob>().Build(),
                TriggerBuilder.Create().WithSimpleSchedule(x => x.WithRepeatCount(0)).StartAt(playTime).Build()
            );
        }
    }
}
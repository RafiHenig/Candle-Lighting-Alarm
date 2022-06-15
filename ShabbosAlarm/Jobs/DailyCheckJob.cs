namespace ShabbosAlarm.Jobs;

public class DailyCheckJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        string locationName = "Lakewood, NJ";
        double latitude = 40.09596; //Lakewood, NJ
        double longitude = -74.22213; //Lakewood, NJ
        double elevation = 0; //optional elevation


        ITimeZone timeZone = new WindowsTimeZone("America/New_York");
        GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
        ComplexZmanimCalendar zc = new ComplexZmanimCalendar(location);
        JewishCalendar jc = new JewishCalendar();

        var todayJewishDate = JewishToday;


        if (jc.IsErevYomTov(todayJewishDate, false) || jc.GetDayOfWeek(todayJewishDate) == DayOfWeek.Friday)
        {
            var playTime = zc.GetCandleLighting()!.Value.AddMinutes(-20);

            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            IScheduler sched = await schedFact.GetScheduler();
            await sched.Start();

            IJobDetail job = JobBuilder.Create<PlayJob>().Build();

            ITrigger onlyOnce = TriggerBuilder.Create()
                        .WithSimpleSchedule(x => x.WithRepeatCount(0))
                        .StartAt(playTime)
                        .Build();

            await sched.ScheduleJob(job, onlyOnce);
        }
    }

    public DateTime JewishToday
    {
        get
        {
            DateTime Today = DateTime.Today;

            HebrewCalendar HebCal = new();
            int curYear = HebCal.GetYear(Today);    //current numeric hebrew year
            int curMonth = HebCal.GetMonth(Today);
            int curDay = HebCal.GetDayOfMonth(Today);



            //return new DateTime(5782, 10, 18, new HebrewCalendar());


            return new DateTime(curYear, curMonth, curDay, new HebrewCalendar());
        }

    }


}
ISchedulerFactory schedFact = new StdSchedulerFactory();

// get a scheduler, start the schedular before triggers or anything else
IScheduler sched = await schedFact.GetScheduler();
await sched.Start();

// create job
IJobDetail job = JobBuilder.Create<DailyCheckJob>().Build();

var trigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule(s => s.OnEveryDay()
																		  .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(7, 00))
																	      .EndingDailyAfterCount(1)
																    )
									.Build();

// Schedule the job using the job and trigger 
await sched.ScheduleJob(job, trigger);

Console.ReadLine();

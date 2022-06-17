CreateHostBuilder(args).Build().Run();

IHostBuilder CreateHostBuilder(string[] args)
{
    Console.OutputEncoding = Encoding.Unicode;

    return Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddLogging(x => x.ClearProviders()
                                             .AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                                             .AddConsole()
                                             .AddDebug()
                   );

                   services.AddSingleton<IGeoLocationProvider, SpringVallyGLProvider>();
                   services.AddSingleton<ComplexZmanimCalendar>(x => new(x.GetRequiredService<IGeoLocationProvider>().GeoLocation));
                   services.AddSingleton<JewishCalendar>();
                   services.AddTransient<DailyCheckJob>();
                   services.AddTransient<PlayJob>();

                   services.AddQuartz(q =>
                   {
                       q.UseMicrosoftDependencyInjectionJobFactory();
                       q.UseInMemoryStore();
                       q.ScheduleJob<DailyCheckJob>(trigger => trigger.WithDailyTimeIntervalSchedule(x => x.OnEveryDay()
                                                                                                           .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(7, 00))
                                                                                                           .EndingDailyAfterCount(1)  // .WithIntervalInHours(24)
                                                                                                           .WithMisfireHandlingInstructionFireAndProceed()
                                                                                                
                                                                                                     )
                       );
                   });

                   services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
               });
}


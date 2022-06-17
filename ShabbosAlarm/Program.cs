CreateHostBuilder(args).Build().Run();


IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
              
                   services.AddLogging(builder =>
                   {
                       builder.ClearProviders();
              
                       builder.AddConfiguration(hostContext.Configuration.GetSection("Logging"))
                              .AddConsole()
                              .AddDebug();
                   });
              
                   services.AddTransient<DailyCheckJob>();
                   services.AddTransient<PlayJob>();
              
                   services.AddQuartz(q =>
                   {
                       // as of 3.3.2 this also injects scoped services (like EF DbContext) without problems
                       q.UseMicrosoftDependencyInjectionJobFactory();
              
                       // or for scoped service support like EF Core DbContext
                       // q.UseMicrosoftDependencyInjectionScopedJobFactory();
              
                       // these are the defaults
                       q.UseSimpleTypeLoader();
                       q.UseInMemoryStore();
                       q.UseDefaultThreadPool(tp => tp.MaxConcurrency = 10);

                       q.ScheduleJob<DailyCheckJob>(trigger => trigger
                          .WithIdentity("Combined Configuration Trigger")
                           .StartNow()
                          .WithDailyTimeIntervalSchedule(x => x.OnEveryDay()
                                                               .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(6, 56))
                                                               .EndingDailyAfterCount(1)
                                                               .WithMisfireHandlingInstructionFireAndProceed()
                          )
                          .WithDescription("my awesome trigger configured for a job with single call")
                      );
              
                   });
              
                   // Quartz.Extensions.Hosting allows you to fire background service that handles scheduler lifecycle
                   services.AddQuartzHostedService(options =>
                   {
                       // when shutting down we want jobs to complete gracefully
                       options.WaitForJobsToComplete = true;
                   });
               });
}


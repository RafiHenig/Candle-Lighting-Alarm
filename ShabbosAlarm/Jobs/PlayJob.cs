namespace ShabbosAlarm.Jobs;

public class PlayJob : IJob
{
    private readonly ILogger _logger;

    public PlayJob(ILogger<PlayJob> logger) => _logger = logger;

    public Task Execute(IJobExecutionContext context)
    {
        new SoundPlayer(@"Assets/goodshabbat.wav").Play();

        _logger.LogInformation("Playing...");

        return Task.CompletedTask;
    }
}

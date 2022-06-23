namespace ShabbosAlarm.Jobs;

public class PlayJob : IJob
{
    private readonly ILogger _logger;
    private readonly IPlayService _playService;

    public PlayJob(ILogger<PlayJob> logger, IPlayService playService) => (_logger,_playService) = (logger,playService);

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Playing soon...");
        _playService.Play();
        return Task.CompletedTask;
    }
}
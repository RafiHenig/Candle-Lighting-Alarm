namespace ShabbosAlarm.Jobs;

public class PlayService : IPlayService
{
    private SoundPlayer[] _playList;
    private readonly ILogger _logger;
    private readonly string[] _fileList = { @"Assets\song.wav", @"Assets\alarm.wav" };

    public TimeSpan Durition { get; }

    public PlayService(ILogger<PlayJob> logger)
    {
        _logger = logger;
        _playList = _fileList.Select(CreateSoundPlayer).ToArray();
        Durition = _fileList.Select(GetWavDurition).Aggregate((x, y) => x + y);
    }

    public void Play()
    {
        foreach (var song in _playList)
        {
            _logger.LogInformation($"Now playing {song.Tag}");
            song.PlaySync();
        }
    }

    private SoundPlayer CreateSoundPlayer(string path)
    {
        var soundPlayer = new SoundPlayer(path);
        soundPlayer.Tag = path;
        return soundPlayer;
    }

    private TimeSpan GetWavDurition(string path) => new WaveFileReader(path).TotalTime;
}

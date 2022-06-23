namespace ShabbosAlarm.Contracts;

public interface IPlayService
{
    public TimeSpan Durition { get; }

    public void Play();
}
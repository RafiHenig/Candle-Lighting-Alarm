namespace ShabbosAlarm.Jobs;

public class PlayJob : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		var media = new SoundPlayer(@"Assets/goodshabbat.wav");
		media.Play();
	}
}

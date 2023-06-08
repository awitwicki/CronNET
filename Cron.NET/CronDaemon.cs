using System.Timers;

namespace Cron.NET;

public class CronDaemon : ICronDaemon
{
    internal readonly System.Timers.Timer Timer = new(30000);
    private readonly List<ICronJob> _cronJobs = new();
    internal DateTime Last = DateTime.Now;

    public CronDaemon()
    {
        Timer.AutoReset = true;
        Timer.Elapsed += _timerElapsed!;
    }

    public CronDaemon AddJob(string schedule, Action action)
    {
        var cj = new CronJob(schedule, action);
        _cronJobs.Add(cj);
        return this;
    }

    public void Start()
    {
        Timer.Start();
    }

    public void Stop()
    {
        Timer.Stop();

        foreach (var job in _cronJobs)
            job.Abort();
    }

    private void _timerElapsed(object sender, ElapsedEventArgs e)
    {
        if (DateTime.Now.Minute != Last.Minute)
        {
            Last = DateTime.Now;

            foreach (var job in _cronJobs)
                job.Execute(DateTime.Now);
        }
    }
}

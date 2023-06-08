namespace Cron.NET;

internal interface ICronDaemon
{
    CronDaemon AddJob(string schedule, Action action);
    void Start();
    void Stop();
}

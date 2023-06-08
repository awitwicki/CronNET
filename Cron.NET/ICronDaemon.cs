namespace Cron.NET;

internal interface ICronDaemon
{
    void AddJob(string schedule, Action action);
    void Start();
    void Stop();
}

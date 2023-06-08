namespace Cron.NET;

internal interface ICronJob
{
    void Execute(DateTime dateTime);
    void Abort();
}

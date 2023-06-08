namespace Cron.NET;

internal interface ICronSchedule
{
    bool IsValid(string expression);
    bool IsTime(DateTime dateTime);
}

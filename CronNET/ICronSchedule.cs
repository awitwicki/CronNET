namespace CronNET
{
    internal interface ICronSchedule
    {
        bool IsValid(string expression);
        bool IsTime(DateTime dateTime);
    }
}

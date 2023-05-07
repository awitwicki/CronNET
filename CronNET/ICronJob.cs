namespace CronNET
{
    internal interface ICronJob
    {
        void Execute(DateTime dateTime);
        void Abort();
    }
}

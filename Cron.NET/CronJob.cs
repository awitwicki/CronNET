namespace Cron.NET;

public class CronJob : ICronJob
{
    private readonly ICronSchedule _cronSchedule;
    private readonly Action _action;
    private Task? _task;
    private CancellationTokenSource? Cts { get; set; }

    private void InitTask()
    {
        Cts = new CancellationTokenSource();

        _task = new Task(() =>
        {
            try
            {
                // Specify this thread's Abort() as the cancel delegate
                using (Cts.Token.Register(() => {}))
                {
                    _action();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Tasks cancelled");
            }
            finally
            {
                Cts.Dispose();
            }
        }, Cts.Token);
    }

    public CronJob(string schedule, Action action)
    {
        _cronSchedule = new CronSchedule(schedule);
        _action = action;
        InitTask();
    }

    private readonly object _lock = new();

    public void Execute(DateTime dateTime)
    {
        lock (_lock)
        {
            if (!_cronSchedule.IsTime(dateTime))
                return;

            if (_task?.Status == TaskStatus.Running)
                return;
            
            InitTask();
            
            _task?.Start();
        }
    }

    public void Abort()
    {
        if (_task?.Status == TaskStatus.Running)
        {
            Cts?.Cancel();
        }
    }
}

namespace CronNET
{
    public class CronJob : ICronJob
    {
        private readonly ICronSchedule _cronSchedule = new CronSchedule();
        private readonly Action _action;
        private readonly Task _task;
        private CancellationTokenSource Cts{ get; set; }

        public CronJob(string schedule, Action action)
        {
            _cronSchedule = new CronSchedule(schedule);
            _action = action;

            Cts = new CancellationTokenSource();

            _task = new Task(() =>
            {
                try
                {
                    // Specify this thread's Abort() as the cancel delegate
                    using (Cts.Token.Register(() => { 
                        return;
                    }))
                    {
                        _action();
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("\nTasks cancelled: timed out.\n");
                }
                finally
                {
                    Cts.Dispose();
                }
            }, Cts.Token);
        }

        private object _lock = new object();
        public void Execute(DateTime dateTime)
        {
            lock (_lock)
            {
                if (!_cronSchedule.IsTime(dateTime))
                    return;

                if (_task.Status == TaskStatus.Running)
                    return;

                _task.Start();
            }
        }

        public void Abort()
        {
            Cts.Cancel();
        }
    }
}

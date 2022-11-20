using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronNET
{
    public class CronJob : ICronJob
    {
        private readonly ICronSchedule _cron_schedule = new CronSchedule();
        private readonly Action _action;
        private readonly Task _task;
        private CancellationTokenSource _cts{ get; set; }

        public CronJob(string schedule, Action action)
        {
            _cron_schedule = new CronSchedule(schedule);
            _action = action;

            _cts = new CancellationTokenSource();

            _task = new Task(() =>
            {
                try
                {
                    // Specify this thread's Abort() as the cancel delegate
                    using (_cts.Token.Register(() => { 
                        //Console.WriteLine("canceled");
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
                    _cts.Dispose();
                }
            }, _cts.Token);
        }

        private object _lock = new object();
        public void Execute(DateTime dateTime)
        {
            lock (_lock)
            {
                if (!_cron_schedule.IsTime(dateTime))
                    return;

                if (_task.Status == TaskStatus.Running)
                    return;

                _task.Start();
            }
        }

        public void Abort()
        {
            _cts.Cancel();
        }
    }
}

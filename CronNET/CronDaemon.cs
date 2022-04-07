using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CronNET
{
    public class CronDaemon : ICronDaemon
    {
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(30000);
        private readonly List<ICronJob> _cronJobs = new List<ICronJob>();
        private DateTime _last = DateTime.Now;

        public CronDaemon()
        {
            _timer.AutoReset = true;
            _timer.Elapsed += _timerElapsed;
        }

        public void AddJob(string schedule, ThreadStart action)
        {
            var cj = new CronJob(schedule, action);
            _cronJobs.Add(cj);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();

            foreach (CronJob job in _cronJobs)
                job.Abort();
        }

        private void _timerElapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Minute != _last.Minute)
            {
                _last = DateTime.Now;
                foreach (ICronJob job in _cronJobs)
                    job.Execute(DateTime.Now);
            }
        }
    }
}

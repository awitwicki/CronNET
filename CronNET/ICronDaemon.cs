using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronNET
{
    internal interface ICronDaemon
    {
        void AddJob(string schedule, Action action);
        void Start();
        void Stop();
    }
}

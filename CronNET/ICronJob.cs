using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronNET
{
    internal interface ICronJob
    {
        void Execute(DateTime dateTime);
        void Abort();
    }
}

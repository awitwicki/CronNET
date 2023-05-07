using System.Threading;
using Xunit;

namespace CronNET.Tests;

public class CronScheduleJobTests
{
    [Fact]
    public static void AbortJob()
    {
        var d = new CronDaemon();
        d.AddJob("* * * * *", () =>
        {
            // Do something
            Thread.Sleep(5000);

            // Throw test error
            Assert.True(false);
        });

        d.Start();

        Thread.Sleep(1000);

        d.Stop();

        Thread.Sleep(70 * 1000);
    }

    [Fact]
    public static void AbortWorkingJob()
    {
        var d = new CronDaemon();
        d.AddJob("* * * * *", () =>
        {
            // Do something
            Thread.Sleep(40 * 1000);

            // Throw test error
            Assert.True(false);
        });

        d.Start();

        Thread.Sleep(40 * 1000);

        d.Stop();

        Thread.Sleep(40 * 1000);
    }
}
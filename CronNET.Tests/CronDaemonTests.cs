using System;
using System.Threading;
using Cron.NET;
using Xunit;

namespace CronNET.Tests;

public class CronDaemonTests
{
    [Fact]
    public static void Start_WithValidInput_ShouldChangeVariable()
    {
        // Arrange
        var processed = false;

        var d = new CronDaemon();
        d.AddJob("* * * * *", () =>
        {
            processed = true;
        });
    
        // Change last time to make it run immediately
        d.Last = DateTime.UtcNow.AddMinutes(-1);
        d.Timer.Interval = 10;
        
        // Act
        d.Start();
        Thread.Sleep(100);
        
        // Assert
        Assert.True(processed);
    }
    
    [Fact]
    public static void Stop_WithValidInput_ShouldNotChangeVariable()
    {
        // Arrange
        var processed = false;

        var d = new CronDaemon();
        d.AddJob("* * * * *", () =>
        {
            Thread.Sleep(1000);
            processed = true;
        });
        
        // Change last time to make it run immediately
        d.Last = DateTime.UtcNow.AddMinutes(-1);
        d.Timer.Interval = 10;
        
        // Act
        d.Start();
        Thread.Sleep(100);
        d.Stop();
        
        // Assert
        Assert.False(processed);
    }
}

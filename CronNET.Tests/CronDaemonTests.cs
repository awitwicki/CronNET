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
    public static void Start_WithValidInput_ShouldChangeVariableAfewTimes()
    {
        // Arrange
        var counter = 0;

        var d = new CronDaemon();
        d.AddJob("* * * * *", () =>
        {
            counter++;
        });
    
        d.Timer.Interval = 10;
        
        // Act
        d.Start();
        
        // Rewind
        for (var i = 0; i < 5; i++)
        {
            Thread.Sleep(50);
            // Change last time to make it run immediately
            d.Last = DateTime.UtcNow.AddMinutes(-1);
            Thread.Sleep(50);
        }
        
        // Assert
        Assert.Equal(5, counter);
    }
    
    [Fact]
    public static void Start_WithValidInput_ShouldChangeManyVariables()
    {
        // Arrange
        var counter1 = 0;
        var counter2 = 0;

        var d = new CronDaemon();
        d.AddJob("* * * * *", () =>
        {
            counter1 += 1;
        });
        d.AddJob("* * * * *", () =>
        {
            counter2 += 2;
        });
    
        d.Timer.Interval = 10;
        
        // Act
        d.Start();
        
        // Rewind
        for (var i = 0; i < 5; i++)
        {
            Thread.Sleep(50);
            // Change last time to make it run immediately
            d.Last = DateTime.UtcNow.AddMinutes(-1);
            Thread.Sleep(50);
        }
        
        // Assert
        Assert.Equal(5, counter1);
        Assert.Equal(10, counter2);
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
    
    [Fact]
    public static void Stop_WithValidInput_ShouldNotChangeVariableOnNextExecutions()
    {
        // Arrange
        var counter = 0;

        var d = new CronDaemon();
        d.AddJob("* * * * *", () =>
        {
            // Nothing
        });
        d.AddJob("* * * * *", () =>
        {
            Thread.Sleep(100);
            counter++;
        });
    
        d.Timer.Interval = 10;
        
        // Act
        d.Start();
        Thread.Sleep(100);
        d.Stop();
        
        // Rewind
        for (var i = 0; i < 5; i++)
        {
            Thread.Sleep(50);
            // Change last time to make it run immediately
            d.Last = DateTime.UtcNow.AddMinutes(-1);
            Thread.Sleep(50);
        }
        
        // Assert
        Assert.Equal(0, counter);
    }
}

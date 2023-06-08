using System;
using Cron.NET;
using Xunit;

namespace CronNET.Tests;

public class CronScheduleIsTimeValidatorTests
{
    [Fact]
    public void IsTime_WithValidInput_ShouldReturnTrue()
    {
        var cronSchedule = new CronSchedule("0 0 12 10 *");
        Assert.True(cronSchedule.IsTime(new DateTime(2022, 10, 12, 00, 00, 00)));
    }

    [Fact]
    public void IsTime_WithValidInput_ShouldReturnFalse()
    {
        var cronSchedule = new CronSchedule("0 0 12 10 *");
        Assert.False(cronSchedule.IsTime(new DateTime(2022, 10, 12, 01, 00, 00)));
    }

    [Theory]
    [InlineData("0 * * * *", "2022-10-12 8:00:00")]
    [InlineData("0-10 * * * *", "2022-10-12 00:00:00")]
    [InlineData("0-10 * * * *", "2022-10-12 08:00:00")]
    [InlineData("*/2 * * * *", "2022-10-12 08:00:00")]
    [InlineData("*/2 * * * *", "2022-10-12 08:02:00")]
    public void IsTimeMinute_WithValidInput_ShouldReturnTrue(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("0 * * * *", "2022-10-12 8:01:00")]
    [InlineData("0-10 * * * *", "2022-10-12 00:13:00")]
    [InlineData("0-10 * * * *", "2022-10-12 08:37:00")]
    public void IsTimeMinute_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.False(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* 0 * * *", "2022-10-12 00:00:00")]
    [InlineData("* 0,12 * * *", "2022-10-12 00:00:00")]
    [InlineData("* 0,12 * * *", "2022-10-12 12:00:00")]
    public void IsTimeHour_WithValidInput_ShouldReturnTrue(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* 0 * * *", "2022-10-12 04:00:00")]
    [InlineData("* 0,12 * * *", "2022-10-12 2:00:00")]
    [InlineData("* 0,12 * * *", "2022-10-12 14:00:00")]
    public void IsTimeHour_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.False(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * 1 * *", "2022-01-01 00:00:00")]
    [InlineData("* * 0,12 * *", "2022-01-12 00:00:00")]
    [InlineData("* * 0,12 * *", "2022-02-12 00:00:00")]
    [InlineData("* * 0,12 * *", "2022-07-12 00:00:00")]
    public void IsTimeDayOfMonth_WithValidInput_ShouldReturnTrue(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * 1 * *", "13-11-2022 00:00:00")]
    [InlineData("* * 0,6 * *", "07-01-2022 00:00:00")]
    [InlineData("* * 0,6 * *", "08-02-2022 00:00:00")]
    [InlineData("* * 0,6 * *", "25-07-2022 00:00:00")]
    public void IsTimeDayOfMonth_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.False(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * * 1 *", "01-01-2022 00:00:00")]
    [InlineData("* * * 12 *", "01-12-2022 00:00:00")]
    [InlineData("* * * */3 *", "01-03-2022 00:00:00")]
    [InlineData("* * * */3 *", "01-06-2022 00:00:00")]
    [InlineData("* * * */3 *", "01-09-2022 00:00:00")]
    [InlineData("* * * */3 *", "01-12-2022 00:00:00")]
    public void IsTimeMonth_WithValidInput_ShouldReturnTrue(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * * 1 *", "01-02-2022 00:00:00")]
    [InlineData("* * * 12 *", "01-11-2022 00:00:00")]
    [InlineData("* * * */3 *", "01-02-2022 00:00:00")]
    [InlineData("* * * */3 *", "01-07-2022 00:00:00")]
    [InlineData("* * * */3 *", "01-11-2022 00:00:00")]
    public void IsTimeMonth_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.False(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * * * 0", "10-04-2022 00:00:00")]
    [InlineData("* * * * */2", "10-04-2022 00:00:00")]
    public void IsTimeDayOfWeek_WithValidInput_ShouldReturnTrue(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * * * 0", "11-04-2022 00:00:00")]
    [InlineData("* * * * */2", "04-04-2022 00:00:00")]
    public void IsTimeDayOfWeek_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.Parse(expectedDateStr);

        var cronSchedule = new CronSchedule(input);

        Assert.False(cronSchedule.IsTime(expectedDate));
    }
}

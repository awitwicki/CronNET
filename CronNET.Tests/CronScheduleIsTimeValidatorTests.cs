using System;
using System.Globalization;
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
    [InlineData("0 * * * *", "2022-10-12 08:00:00")]
    [InlineData("0-10 * * * *", "2022-10-12 00:00:00")]
    [InlineData("0-10 * * * *", "2022-10-12 08:00:00")]
    [InlineData("*/2 * * * *", "2022-10-12 08:00:00")]
    [InlineData("*/2 * * * *", "2022-10-12 08:02:00")]
    public void IsTimeMinute_WithValidInput_ShouldReturnTrue(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("0 * * * *", "2022-10-12 08:01:00")]
    [InlineData("0-10 * * * *", "2022-10-12 00:13:00")]
    [InlineData("0-10 * * * *", "2022-10-12 08:37:00")]
    public void IsTimeMinute_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var cronSchedule = new CronSchedule(input);

        Assert.False(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* 0 * * *", "2022-10-12 00:00:00")]
    [InlineData("* 0,12 * * *", "2022-10-12 00:00:00")]
    [InlineData("* 0,12 * * *", "2022-10-12 12:00:00")]
    public void IsTimeHour_WithValidInput_ShouldReturnTrue(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* 0 * * *", "2022-10-12 04:00:00")]
    [InlineData("* 0,12 * * *", "2022-10-12 02:00:00")]
    [InlineData("* 0,12 * * *", "2022-10-12 14:00:00")]
    public void IsTimeHour_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

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
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * 1 * *", "2022-11-13 00:00:00")]
    [InlineData("* * 0,6 * *", "2022-01-07 00:00:00")]
    [InlineData("* * 0,6 * *", "2022-02-08 00:00:00")]
    [InlineData("* * 0,6 * *", "2022-07-25 00:00:00")]
    public void IsTimeDayOfMonth_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var cronSchedule = new CronSchedule(input);

        Assert.False(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * * 1 *", "2022-01-01 00:00:00")]
    [InlineData("* * * 12 *", "2022-12-01 00:00:00")]
    [InlineData("* * * */3 *", "2022-03-01 00:00:00")]
    [InlineData("* * * */3 *", "2022-06-01 00:00:00")]
    [InlineData("* * * */3 *", "2022-09-01 00:00:00")]
    [InlineData("* * * */3 *", "2022-12-01 00:00:00")]
    public void IsTimeMonth_WithValidInput_ShouldReturnTrue(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * * 1 *", "2022-02-01 00:00:00")]
    [InlineData("* * * 12 *", "2022-11-01 00:00:00")]
    [InlineData("* * * */3 *", "2022-02-01 00:00:00")]
    [InlineData("* * * */3 *", "2022-07-01 00:00:00")]
    [InlineData("* * * */3 *", "2022-11-01 00:00:00")]
    public void IsTimeMonth_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var cronSchedule = new CronSchedule(input);

        Assert.False(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * * * 0", "2022-04-10 00:00:00")]
    [InlineData("* * * * */2", "2022-04-10 00:00:00")]
    public void IsTimeDayOfWeek_WithValidInput_ShouldReturnTrue(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var cronSchedule = new CronSchedule(input);

        Assert.True(cronSchedule.IsTime(expectedDate));
    }

    [Theory]
    [InlineData("* * * * 0", "2022-04-11 00:00:00")]
    [InlineData("* * * * */2", "2022-04-04 00:00:00")]
    public void IsTimeDayOfWeek_WithValidInput_ShouldReturnFalse(string input, string expectedDateStr)
    {
        var expectedDate = DateTime.ParseExact(expectedDateStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var cronSchedule = new CronSchedule(input);

        Assert.False(cronSchedule.IsTime(expectedDate));
    }
}

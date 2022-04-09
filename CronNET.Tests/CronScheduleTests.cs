using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace CronNET.Tests
{
    public class CronScheduleTests
    {
        [Fact]
        public void IsValidTest()
        {
            CronSchedule cronSchedule = new CronSchedule();
            Assert.True(cronSchedule.IsValid("*/2"));
            Assert.True(cronSchedule.IsValid("* * * * *"));
            Assert.True(cronSchedule.IsValid("0 * * * *"));
            Assert.True(cronSchedule.IsValid("0,1,2 * * * *"));
            Assert.True(cronSchedule.IsValid("*/2 * * * *"));
            Assert.True(cronSchedule.IsValid("1-4 * * * *"));
            Assert.True(cronSchedule.IsValid("1-55/3 * * * *"));
            Assert.True(cronSchedule.IsValid("1,10,20 * * * *"));
            Assert.True(cronSchedule.IsValid("* 1,10,20 * * *"));
        }

        [Fact]
        public static void DividedArrayTest()
        {
            var cron_schedule = new CronSchedule("*/2");
            List<int> results = cron_schedule.minutes.GetRange(0, 5);//("*/2", 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Fact]
        public static void RangeArrayTest()
        {
            var cron_schedule = new CronSchedule("1-10");
            List<int> results = cron_schedule.minutes.GetRange(0, 10);//();
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var cs = new CronSchedule("1-10/3 20-45/4 * * *");
            results = cs.minutes;
            Assert.Equal(results.ToArray(), new int[] { 3, 6, 9 });
        }

        [Fact]
        public void WildArrayTest()
        {
            var cron_schedule = new CronSchedule("*");
            List<int> results = cron_schedule.minutes.GetRange(0, 10);//("*", 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }

        [Fact]
        public void ListArrayTest()
        {
            var cron_schedule = new CronSchedule("1,2,3,4,5,6,7,8,9,10");
            List<int> results = cron_schedule.minutes;
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [Fact]
        public void GenerateValuesDividedTest()
        {
            var cron_schedule = new CronSchedule("*/2");
            List<int> results = cron_schedule.minutes.GetRange(0, 5);//(, 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Fact]
        public void GenerateValuesRangeTest()
        {
            var cron_schedule = new CronSchedule("1-10");
            List<int> results = cron_schedule.minutes.GetRange(0, 10);//(, 0, 10);
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [Fact]
        public void GenerateMinutesTest()
        {
            var cron_schedule = new CronSchedule("1,2,3 * * * *");
            Assert.Equal(cron_schedule.minutes.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void GenerateHoursTest()
        {
            var cron_schedule = new CronSchedule("* 1,2,3 * * *");
            Assert.Equal(cron_schedule.hours.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void GenerateDaysOfMonthTest()
        {
            var cron_schedule = new CronSchedule("* * 1,2,3 * *");
            Assert.Equal(cron_schedule.days_of_month.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void GenerateMonthsTest()
        {
            var cron_schedule = new CronSchedule("* * * 1,2,3 *");
            Assert.Equal(cron_schedule.months.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void GenerateDaysOfWeeks()
        {
            var cron_schedule = new CronSchedule("* * * * 1,2,3 ");
            Assert.Equal(cron_schedule.days_of_week.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void IsTimeMinuteTest()
        {
            var cron_schedule = new CronSchedule("0 * * * *");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("8:00 am")));
            Assert.False(cron_schedule.IsTime(DateTime.Parse("8:01 am")));

            cron_schedule = new CronSchedule("0-10 * * * *");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("8:00 am")));
            Assert.True(cron_schedule.IsTime(DateTime.Parse("8:03 am")));

            cron_schedule = new CronSchedule("*/2 * * * *");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("8:00 am")));
            Assert.True(cron_schedule.IsTime(DateTime.Parse("8:02 am")));
            Assert.False(cron_schedule.IsTime(DateTime.Parse("8:03 am")));
        }

        [Fact]
        public void IsTimeHourTest()
        {
            var cron_schedule = new CronSchedule("* 0 * * *");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("12:00 am")));

            cron_schedule = new CronSchedule("* 0,12 * * *");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("12:00 am")));
            Assert.True(cron_schedule.IsTime(DateTime.Parse("12:00 pm")));
        }

        [Fact]
        public void IsTimeDayOfMonthTest()
        {
            var cron_schedule = new CronSchedule("* * 1 * *");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("2010/08/01")));
        }

        [Fact]
        public void IsTimeMonthTest()
        {
            var cron_schedule = new CronSchedule("* * * 1 *");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("1/1/2008")));

            cron_schedule = new CronSchedule("* * * 12 *");
            Assert.False(cron_schedule.IsTime(DateTime.Parse("1/1/2008")));

            cron_schedule = new CronSchedule("* * * */3 *");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("1/3/2008")));
            Assert.True(cron_schedule.IsTime(DateTime.Parse("1/6/2008")));
        }

        [Fact]
        public void IsTimeDayOfWeekTest()
        {
            var cron_schedule = new CronSchedule("* * * * 0");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("12/10/2008")));
            Assert.False(cron_schedule.IsTime(DateTime.Parse("13/10/2008")));

            cron_schedule = new CronSchedule("* * * * */2");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("14/10/2008")));
        }

        [Fact]
        public void IsTimeTest()
        {
            var cron_schedule = new CronSchedule("0 0 12 10 *");
            Assert.True(cron_schedule.IsTime(DateTime.Parse("12:00:00 am 12/10/2008")));
            Assert.False(cron_schedule.IsTime(DateTime.Parse("12:01:00 am 12/10/2008")));
        }

        [Fact]
        public static void IntegrationTest()
        {
            CronDaemon d = new CronDaemon();
            d.AddJob("*/1 * * * *", () => { Console.WriteLine(DateTime.Now.ToString()); });
            d.Start();
            //Thread.Sleep(60 * 1000);
        }
    }
}
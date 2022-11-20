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
            List<int> results = cron_schedule.minutes.GetRange(0, 5);
            Assert.Equal(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Fact]
        public static void RangeArrayTest()
        {
            var cron_schedule = new CronSchedule("1-10");
            List<int> results = cron_schedule.minutes.GetRange(0, 10);
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var cs = new CronSchedule("1-10/3 20-45/4 * * *");
            results = cs.minutes;
            Assert.Equal(results.ToArray(), new int[] { 3, 6, 9 });
        }

        [Fact]
        public void WildArrayTest()
        {
            var cron_schedule = new CronSchedule("*");
            List<int> results = cron_schedule.minutes.GetRange(0, 10);
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
            List<int> results = cron_schedule.minutes.GetRange(0, 5);
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
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 10, 12, 08, 00, 00)));
            Assert.False(cron_schedule.IsTime(new DateTime(2022, 10, 12, 08, 01, 00)));

            cron_schedule = new CronSchedule("0-10 * * * *");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 10, 12, 00, 00, 00)));
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 10, 12, 00, 08, 00)));

            cron_schedule = new CronSchedule("*/2 * * * *");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 10, 12, 08, 00, 00)));
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 10, 12, 08, 02, 00)));
            Assert.False(cron_schedule.IsTime(new DateTime(2022, 10, 12, 08, 03, 00)));
        }

        [Fact]
        public void IsTimeHourTest()
        {
            var cron_schedule = new CronSchedule("* 0 * * *");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 10, 12, 00, 00, 00)));

            cron_schedule = new CronSchedule("* 0,12 * * *");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 10, 12, 00, 00, 00)));
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 10, 12, 12, 00, 00)));
        }

        [Fact]
        public void IsTimeDayOfMonthTest()
        {
            var cron_schedule = new CronSchedule("* * 1 * *");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 01, 01)));
        }

        [Fact]
        public void IsTimeMonthTest()
        {
            var cron_schedule = new CronSchedule("* * * 1 *");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 01, 01)));

            cron_schedule = new CronSchedule("* * * 12 *");
            Assert.False(cron_schedule.IsTime(new DateTime(2022, 01, 01)));

            cron_schedule = new CronSchedule("* * * */3 *");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 03, 01)));
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 06, 01)));
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 09, 01)));
        }

        [Fact]
        public void IsTimeDayOfWeekTest()
        {
            var cron_schedule = new CronSchedule("* * * * 0");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 04, 10)));
            Assert.False(cron_schedule.IsTime(new DateTime(2022, 04, 11)));

            cron_schedule = new CronSchedule("* * * * */2");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 04, 10)));
        }

        [Fact]
        public void IsTimeTest()
        {
            var cron_schedule = new CronSchedule("0 0 12 10 *");
            Assert.True(cron_schedule.IsTime(new DateTime(2022, 10, 12, 00, 00, 00)));
            Assert.False(cron_schedule.IsTime(new DateTime(2022, 10, 12, 01, 00, 00)));
        }

        [Fact]
        public static void AbortJob()
        {
            CronDaemon d = new CronDaemon();
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
            CronDaemon d = new CronDaemon();
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
}
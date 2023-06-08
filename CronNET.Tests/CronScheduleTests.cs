using Cron.NET;
using Xunit;

namespace CronNET.Tests
{
    public class CronScheduleTests
    {
        [Theory]
        [InlineData("*/2")]
        [InlineData("* * * * *")]
        [InlineData("0 * * * *")]
        [InlineData("0,1,2 * * * *")]
        [InlineData("*/2 * * * *")]
        [InlineData("1-4 * * * *")]
        [InlineData("1-55/3 * * * *")]
        [InlineData("1,10,20 * * * *")]
        [InlineData("* 1,10,20 * * *")]
        public void IsValid_WithValidInput_ShouldReturnTrue(string input)
        {
            var cronSchedule = new CronSchedule();
            Assert.True(cronSchedule.IsValid(input));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null!)]
        [InlineData("lorem ipsum")]
        public void IsValid_WithInvalidInput_ShouldReturnFalse(string input)
        {
            var cronSchedule = new CronSchedule();
            Assert.False(cronSchedule.IsValid(input));
        }

        [Fact]
        public static void CronSchedule_WithValidInputWithDividers_ShouldParseProperValue()
        {
            var cronSchedule = new CronSchedule("*/2");
            var results = cronSchedule.Minutes.GetRange(0, 5);
            
            Assert.Equal(results.ToArray(), new int[] { 0, 2, 4, 6, 8 });
        }

        [Theory]
        [InlineData("1-10", new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [InlineData("1-10/3 20-45/4 * * *", new int[] { 3, 6, 9 })]
        public static void CronSchedule_WithValidInputWithRange_ShouldParseProperValue(string input, int[] expected)
        {
            var cronSchedule = new CronSchedule(input);
            var results = cronSchedule.Minutes;
            
            Assert.Equal(results.ToArray(), expected);
        }

        [Fact]
        public void CronSchedule_WithValidInputWithWild_ShouldParseProperValue()
        {
            var cronSchedule = new CronSchedule("*");
            
            var results = cronSchedule.Minutes.GetRange(0, 10);
            
            Assert.Equal(results.ToArray(), new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }

        [Fact]
        public void CronSchedule_WithValidInputListArray_ShouldParseProperValue()
        {
            var cronSchedule = new CronSchedule("1,2,3,4,5,6,7,8,9,10");
            
            var results = cronSchedule.Minutes;
            
            Assert.Equal(results.ToArray(), new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }

        [Fact]
        public void CronSchedule_WithValidInputMinutesList_ShouldParseProperValue()
        {
            var cronSchedule = new CronSchedule("1,2,3 * * * *");
            Assert.Equal(cronSchedule.Minutes.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void CronSchedule_WithValidInputHoursList_ShouldParseProperValue()
        {
            var cronSchedule = new CronSchedule("* 1,2,3 * * *");
            Assert.Equal(cronSchedule.Hours.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void CronSchedule_WithValidInputDaysOfMonthList_ShouldParseProperValue()
        {
            var cronSchedule = new CronSchedule("* * 1,2,3 * *");
            Assert.Equal(cronSchedule.DaysOfMonth.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void CronSchedule_WithValidInputMonthList_ShouldParseProperValue()
        {
            var cronSchedule = new CronSchedule("* * * 1,2,3 *");
            Assert.Equal(cronSchedule.Months.ToArray(), new int[] { 1, 2, 3 });
        }

        [Fact]
        public void CronSchedule_WithValidInputDaysOfWeeksList_ShouldParseProperValue()
        {
            var cronSchedule = new CronSchedule("* * * * 1,2,3 ");
            Assert.Equal(cronSchedule.DaysOfWeek.ToArray(), new int[] { 1, 2, 3 });
        }
    }
}
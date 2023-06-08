using System.Text.RegularExpressions;

namespace Cron.NET;

public class CronSchedule : ICronSchedule
{
    private static readonly Regex DividedRegex = new(@"(\*/\d+)");
    private static readonly Regex RangeRegex = new(@"(\d+\-\d+)\/?(\d+)?");
    private static readonly Regex WildRegex = new(@"(\*)");
    private static readonly Regex ListRegex = new(@"(((\d+,)*\d+)+)");

    private static readonly Regex ValidationRegex =
        new(DividedRegex + "|" + RangeRegex + "|" + WildRegex + "|" + ListRegex);

    private readonly string _expression = string.Empty;
    internal List<int> Minutes = new();
    internal List<int> Hours = new();
    internal List<int> DaysOfMonth = new();
    internal List<int> Months = new();
    internal List<int> DaysOfWeek = new();

    public CronSchedule()
    {
    }

    public CronSchedule(string expressions)
    {
        _expression = expressions;
        _generate();
    }

    private bool IsValid()
    {
        return IsValid(_expression);
    }

    public bool IsValid(string expression)
    {
        if (expression == null!)
            return false;

        var matches = ValidationRegex.Matches(expression);
        return matches.Count > 0;
    }

    public bool IsTime(DateTime dateTime)
    {
        return Minutes.Contains(dateTime.Minute) &&
               Hours.Contains(dateTime.Hour) &&
               DaysOfMonth.Contains(dateTime.Day) &&
               Months.Contains(dateTime.Month) &&
               DaysOfWeek.Contains((int)dateTime.DayOfWeek);
    }

    private void _generate()
    {
        if (!IsValid())
            return;

        var matches = ValidationRegex.Matches(_expression);

        _generateMinutes(matches[0].ToString());

        if (matches.Count > 1)
            _generateHours(matches[1].ToString());
        else
            _generateHours("*");

        if (matches.Count > 2)
            _generateDaysOfMonth(matches[2].ToString());
        else
            _generateDaysOfMonth("*");

        if (matches.Count > 3)
            _generateMonths(matches[3].ToString());
        else
            _generateMonths("*");

        if (matches.Count > 4)
            _generateDaysOfWeeks(matches[4].ToString());
        else
            _generateDaysOfWeeks("*");
    }

    private void _generateMinutes(string match)
    {
        Minutes = _generateValues(match, 0, 60);
    }

    private void _generateHours(string match)
    {
        Hours = _generateValues(match, 0, 24);
    }

    private void _generateDaysOfMonth(string match)
    {
        DaysOfMonth = _generateValues(match, 1, 32);
    }

    private void _generateMonths(string match)
    {
        Months = _generateValues(match, 1, 13);
    }

    private void _generateDaysOfWeeks(string match)
    {
        DaysOfWeek = _generateValues(match, 0, 7);
    }

    private List<int> _generateValues(string configuration, int start, int max)
    {
        if (DividedRegex.IsMatch(configuration)) return _dividedArray(configuration, start, max);
        if (RangeRegex.IsMatch(configuration)) return _rangeArray(configuration);
        if (WildRegex.IsMatch(configuration)) return _wildArray(configuration, start, max);
        if (ListRegex.IsMatch(configuration)) return _listArray(configuration);

        return new List<int>();
    }

    private List<int> _dividedArray(string configuration, int start, int max)
    {
        if (!DividedRegex.IsMatch(configuration))
            return new List<int>();

        var ret = new List<int>();
        var split = configuration.Split("/".ToCharArray());
        var divisor = int.Parse(split[1]);

        for (var i = start; i < max; ++i)
            if (i % divisor == 0)
                ret.Add(i);

        return ret;
    }

    private List<int> _rangeArray(string configuration)
    {
        if (!RangeRegex.IsMatch(configuration))
            return new List<int>();

        var ret = new List<int>();
        var split = configuration.Split("-".ToCharArray());
        var start = int.Parse(split[0]);
        var end = 0;

        if (split[1].Contains("/"))
        {
            split = split[1].Split("/".ToCharArray());
            end = int.Parse(split[0]);
            var divisor = int.Parse(split[1]);

            for (var i = start; i < end; ++i)
                if (i % divisor == 0)
                    ret.Add(i);
            return ret;
        }
        else
            end = int.Parse(split[1]);

        for (var i = start; i <= end; ++i)
            ret.Add(i);

        return ret;
    }

    private List<int> _wildArray(string configuration, int start, int max)
    {
        if (!WildRegex.IsMatch(configuration))
            return new List<int>();

        var ret = new List<int>();

        for (var i = start; i < max; ++i)
            ret.Add(i);

        return ret;
    }

    private List<int> _listArray(string configuration)
    {
        if (!ListRegex.IsMatch(configuration))
            return new List<int>();

        var ret = new List<int>();

        var split = configuration.Split(",".ToCharArray());

        foreach (var s in split)
            ret.Add(int.Parse(s));

        return ret;
    }
}

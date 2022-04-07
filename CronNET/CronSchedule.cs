using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CronNET
{
    public class CronSchedule : ICronSchedule
    {
        #region Readonly Class Members

        readonly static Regex _dividedRegex = new Regex(@"(\*/\d+)");
        readonly static Regex _rangeRegex = new Regex(@"(\d+\-\d+)\/?(\d+)?");
        readonly static Regex _wildRegex = new Regex(@"(\*)");
        readonly static Regex _listLegex = new Regex(@"(((\d+,)*\d+)+)");
        readonly static Regex _validationRegex = new Regex(_dividedRegex + "|" + _rangeRegex + "|" + _wildRegex + "|" + _listLegex);

        #endregion

        #region Private Instance Members

        private readonly string _expression;
        public List<int> minutes;
        public List<int> hours;
        public List<int> days_of_month;
        public List<int> months;
        public List<int> days_of_week;

        #endregion

        #region Public Constructors

        public CronSchedule()
        {
        }

        public CronSchedule(string expressions)
        {
            this._expression = expressions;
            _generate();
        }

        #endregion

        #region Public Methods

        private bool IsValid()
        {
            return IsValid(this._expression);
        }

        public bool IsValid(string expression)
        {
            MatchCollection matches = _validationRegex.Matches(expression);
            return matches.Count > 0;
        }

        public bool IsTime(DateTime dateTime)
        {
            return minutes.Contains(dateTime.Minute) &&
                   hours.Contains(dateTime.Hour) &&
                   days_of_month.Contains(dateTime.Day) &&
                   months.Contains(dateTime.Month) &&
                   days_of_week.Contains((int)dateTime.DayOfWeek);
        }

        private void _generate()
        {
            if (!IsValid()) return;

            MatchCollection matches = _validationRegex.Matches(this._expression);

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
            this.minutes = _generateValues(match, 0, 60);
        }

        private void _generateHours(string match)
        {
            this.hours = _generateValues(match, 0, 24);
        }

        private void _generateDaysOfMonth(string match)
        {
            this.days_of_month = _generateValues(match, 1, 32);
        }

        private void _generateMonths(string match)
        {
            this.months = _generateValues(match, 1, 13);
        }

        private void _generateDaysOfWeeks(string match)
        {
            this.days_of_week = _generateValues(match, 0, 7);
        }

        private List<int> _generateValues(string configuration, int start, int max)
        {
            if (_dividedRegex.IsMatch(configuration)) return _dividedArray(configuration, start, max);
            if (_rangeRegex.IsMatch(configuration)) return _rangeArray(configuration);
            if (_wildRegex.IsMatch(configuration)) return _wildArray(configuration, start, max);
            if (_listLegex.IsMatch(configuration)) return _listArray(configuration);

            return new List<int>();
        }

        private List<int> _dividedArray(string configuration, int start, int max)
        {
            if (!_dividedRegex.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();
            string[] split = configuration.Split("/".ToCharArray());
            int divisor = int.Parse(split[1]);

            for (int i = start; i < max; ++i)
                if (i % divisor == 0)
                    ret.Add(i);

            return ret;
        }

        private List<int> _rangeArray(string configuration)
        {
            if (!_rangeRegex.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();
            string[] split = configuration.Split("-".ToCharArray());
            int start = int.Parse(split[0]);
            int end = 0;
            if (split[1].Contains("/"))
            {
                split = split[1].Split("/".ToCharArray());
                end = int.Parse(split[0]);
                int divisor = int.Parse(split[1]);

                for (int i = start; i < end; ++i)
                    if (i % divisor == 0)
                        ret.Add(i);
                return ret;
            }
            else
                end = int.Parse(split[1]);

            for (int i = start; i <= end; ++i)
                ret.Add(i);

            return ret;
        }

        private List<int> _wildArray(string configuration, int start, int max)
        {
            if (!_wildRegex.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();

            for (int i = start; i < max; ++i)
                ret.Add(i);

            return ret;
        }

        private List<int> _listArray(string configuration)
        {
            if (!_listLegex.IsMatch(configuration))
                return new List<int>();

            List<int> ret = new List<int>();

            string[] split = configuration.Split(",".ToCharArray());

            foreach (string s in split)
                ret.Add(int.Parse(s));

            return ret;
        }

        #endregion
    }
}

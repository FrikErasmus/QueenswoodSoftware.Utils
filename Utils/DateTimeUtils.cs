namespace Library.Utils
{
    /// <summary>
    /// Provides static helpers for converting, comparing, and formatting dates and times.
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> value to a Unix timestamp in seconds.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <returns>The number of seconds since the Unix epoch (UTC).</returns>
        public static long ToUnixTimestamp(DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime.ToUniversalTime() - epoch).TotalSeconds;
        }

        /// <summary>
        /// Converts a Unix timestamp in seconds to a local <see cref="DateTime"/>.
        /// </summary>
        /// <param name="timestamp">The Unix timestamp in seconds.</param>
        /// <returns>The corresponding local date and time.</returns>
        public static DateTime FromUnixTimestamp(long timestamp)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(timestamp).ToLocalTime();
        }

        /// <summary>
        /// Returns the later of two nullable dates, treating null as absent.
        /// </summary>
        /// <param name="date1">The first date to compare.</param>
        /// <param name="date2">The second date to compare.</param>
        /// <returns>The maximum date, or null when neither date is valid.</returns>
        public static DateTime? GetMaxDate(DateTime? date1, DateTime? date2)
        {
            DateTime? returnDate;
            returnDate = Convert.ToDateTime("1900-01-01 00:00:01");
            try
            {
                if (date1 == null)
                {
                    date1 = returnDate;
                }
                if (date2 == null)
                {
                    date2 = returnDate;
                }

                if (date1 > date2)
                {
                    returnDate = date1;
                }
                else
                {
                    returnDate = date2;
                }
            }
            catch (Exception ex)
            {
                //
            }
            if (returnDate == Convert.ToDateTime("1900-01-01 00:00:01"))
            {
                returnDate = null;
            }
            return returnDate;
        }

        /// <summary>
        /// Returns the Unix epoch seconds for the start of the current hour.
        /// </summary>
        /// <returns>The epoch value as a string using the current culture.</returns>
        public static string ReturnEpoch()
        {
            var t = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0) - new DateTime(1970, 1, 1, 0, 0, 0);
            return t.TotalSeconds.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the start of the day (midnight) for the specified date and time.
        /// </summary>
        /// <param name="dateTime">The date and time value.</param>
        /// <returns>A <see cref="DateTime"/> at 00:00:00 on the same calendar day.</returns>
        public static DateTime GetStartOfDay(DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the last tick of the specified calendar day.
        /// </summary>
        /// <param name="dateTime">The date and time value.</param>
        /// <returns>A <see cref="DateTime"/> representing 23:59:59.9999999 on the same calendar day.</returns>
        public static DateTime GetEndOfDay(DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Formats a <see cref="DateTime"/> using the specified format string.
        /// </summary>
        /// <param name="dateTime">The date and time to format.</param>
        /// <param name="format">The format string passed to <see cref="DateTime.ToString(string)"/>.</param>
        /// <returns>The formatted date and time string.</returns>
        public static string FormatDate(DateTime dateTime, string format)
        {
            return dateTime.ToString(format);
        }

        /// <summary>
        /// Calculates age in whole years from a birth date to the current date.
        /// </summary>
        /// <param name="birthDate">The date of birth.</param>
        /// <returns>The age in years.</returns>
        public static int GetAge(DateTime birthDate)
        {
            var now = DateTime.Now;
            var age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || now.Month == birthDate.Month && now.Day < birthDate.Day)
            {
                age--;
            }
            return age;
        }

        /// <summary>
        /// Gets the next occurrence of the specified weekday on or after the start date.
        /// </summary>
        /// <param name="start">The starting date.</param>
        /// <param name="day">The target day of the week.</param>
        /// <returns>The next matching weekday.</returns>
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        /// <summary>
        /// Gets a weekday occurrence a number of weeks after the next matching day from the start date.
        /// </summary>
        /// <param name="start">The starting date.</param>
        /// <param name="day">The target day of the week.</param>
        /// <param name="weeks">The number of additional weeks to add.</param>
        /// <returns>The calculated date.</returns>
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day, int weeks)
        {
            var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd + weeks * 7);
        }

        /// <summary>
        /// Gets a weekday occurrence with additional weeks and a time offset from the start date.
        /// </summary>
        /// <param name="start">The starting date.</param>
        /// <param name="day">The target day of the week.</param>
        /// <param name="weeks">The number of additional weeks to add.</param>
        /// <param name="hours">Hours to add to the result.</param>
        /// <param name="minutes">Minutes to add to the result.</param>
        /// <param name="seconds">Seconds to add to the result.</param>
        /// <returns>The calculated date and time.</returns>
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day, int weeks, int hours, int minutes, int seconds)
        {
            var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd + weeks * 7).AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
        }
    }
}

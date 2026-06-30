namespace Library.Helpers
{
    /// <summary>
    /// Provides abbreviated month name and value pairs for UI selection lists.
    /// </summary>
    public class Months
    {
        /// <summary>
        /// Returns the twelve calendar months as name/value pairs.
        /// </summary>
        /// <returns>List of month abbreviations and numeric month values.</returns>
        public List<NameValue> MonthsInYear()
        {
            return
            [
                new() { Name = "Jan", Value = "1" },
                new() { Name = "Feb", Value = "2" },
                new() { Name = "Mar", Value = "3" },
                new() { Name = "Apr", Value = "4" },
                new() { Name = "May", Value = "5" },
                new() { Name = "Jun", Value = "6" },
                new() { Name = "Jul", Value = "7" },
                new() { Name = "Aug", Value = "8" },
                new() { Name = "Sept", Value = "9" },
                new() { Name = "Oct", Value = "10" },
                new() { Name = "Nov", Value = "11" },
                new() { Name = "Dec", Value = "12" }
            ];
        }
    }

    /// <summary>
    /// Calendar and ISO 8601 week calculations for reporting and scheduling.
    /// </summary>
    public class YearWeekNumbers
    {
        /// <summary>
        /// Gets the culture-specific week number for a date.
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns>Week of year according to the current culture calendar rules.</returns>
        public int GetWeekNumber(DateTime date)
        {
            var cultureInfo = CultureInfo.CurrentCulture;
            var calendar = cultureInfo.Calendar;
            return calendar.GetWeekOfYear(date, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
        }

        /// <summary>
        /// Calculates Monday-through-Sunday bounds for the week containing the selected date.
        /// </summary>
        /// <param name="selectedDate">Any date within the desired week.</param>
        /// <returns>Start-of-week (Monday) and end-of-week (Sunday) dates at midnight.</returns>
        public (DateTime StartOfWeek, DateTime EndOfWeek) GetWeekRange(DateTime selectedDate)
        {
            // Get the current day of the week
            var currentDay = selectedDate.DayOfWeek;

            // Calculate the start of the week (Monday)
            var daysToMonday = (currentDay == DayOfWeek.Sunday) ? -6 : DayOfWeek.Monday - currentDay;
            var startOfWeek = selectedDate.AddDays(daysToMonday).Date;

            // Calculate the end of the week (Sunday)
            var endOfWeek = startOfWeek.AddDays(6);

            return (startOfWeek, endOfWeek);
        }


        /// <summary>
        /// Builds ISO week records for each year in an inclusive range.
        /// </summary>
        /// <param name="startYear">First calendar year to include.</param>
        /// <param name="endYear">Last calendar year to include.</param>
        /// <returns>List of <see cref="WeekInfo"/> entries with ISO week numbers and date ranges.</returns>
        public List<WeekInfo> GenerateWeeks(int startYear, int endYear)
        {
            var weeks = new List<WeekInfo>();

            for (var year = startYear; year <= endYear; year++)
            {
                var firstDayOfYear = new DateTime(year, 1, 1);
                var lastDayOfYear = new DateTime(year, 12, 31);

                // Start at the first week
                var startOfWeek = GetStartOfIsoWeek(firstDayOfYear);

                while (startOfWeek <= lastDayOfYear)
                {
                    var endOfWeek = startOfWeek.AddDays(6);
                    var weekNumber = GetIso8601WeekOfYear(startOfWeek);

                    weeks.Add(new WeekInfo
                    {
                        Year = year,
                        WeekNumber = weekNumber,
                        StartDate = startOfWeek,
                        EndDate = endOfWeek
                    });

                    // Move to the next week
                    startOfWeek = startOfWeek.AddDays(7);
                }
            }

            return weeks;
        }

        /// <summary>
        /// Returns the Monday that starts the ISO week containing the given date.
        /// </summary>
        /// <param name="date">Reference date within the week.</param>
        /// <returns>Date adjusted to the Monday of the ISO week.</returns>
        private static DateTime GetStartOfIsoWeek(DateTime date)
        {
            // Adjust to Monday of the ISO week
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            var offset = (day == DayOfWeek.Sunday ? -6 : DayOfWeek.Monday - day);
            return date.AddDays(offset);
        }

        /// <summary>
        /// Computes the ISO 8601 week number for a date using the FirstFourDayWeek rule and Monday as the first day of the week.
        /// </summary>
        /// <param name="time">Date within the week to number.</param>
        /// <returns>ISO 8601 week number.</returns>
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            // ISO 8601 weeks start on Monday
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                // Adjust the date to the nearest Thursday
                time = time.AddDays(3 - (int)day);
            }

            // Return the ISO 8601 week number
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                time,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }
    }

    /// <summary>
    /// Describes a calendar year, ISO week number, and inclusive start/end dates for that week.
    /// </summary>
    public class WeekInfo
    {
        /// <summary>Calendar year for the week.</summary>
        public int Year { get; set; }

        /// <summary>ISO week number within the year.</summary>
        public int WeekNumber { get; set; }

        /// <summary>First day of the week (inclusive).</summary>
        public DateTime StartDate { get; set; }

        /// <summary>Last day of the week (inclusive).</summary>
        public DateTime EndDate { get; set; }
    }
}

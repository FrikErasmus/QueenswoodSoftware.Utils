namespace Library.Helpers
{
    /// <summary>
    /// Time-of-day greeting text for UI display.
    /// </summary>
    public class Greeting
    {
        /// <summary>
        /// sets the greeting depending on the time
        /// </summary>
        /// <returns>greeting depending on the time</returns>
        public string TimeOfDay()
        {
            var time = DateTime.Now.TimeOfDay;

            if (time > new TimeSpan(00, 00, 00)
             && time < new TimeSpan(12, 00, 00))
            {
                return "Good morning";
            }
            if (time > new TimeSpan(12, 00, 00)
            && time < new TimeSpan(18, 00, 00))
            {
                return "Good afternoon";
            }
            return "Good evening";
        }
    }
}
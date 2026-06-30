namespace Library.Helpers
{
    /// <summary>
    /// Unix epoch helpers for hour-based authentication tokens.
    /// </summary>
    public class Epoch
    {
        /// <summary>
        /// Returns current hour in seconds.
        /// </summary>
        /// <returns></returns>
        public string ReturnEpoch()
        {
            var t = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, 0, 0) - new DateTime(1970, 1, 1, 0, 0, 0);
            return t.TotalSeconds.ToString(CultureInfo.CurrentCulture);
        }
    }
}
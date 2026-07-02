namespace QueenswoodSoftware.Utils.Helpers
{
    /// <summary>
    /// Logger File Destination
    /// </summary>
    public enum LoggerFile
    {
        Log,
        Error,
        Warning
    }

    public interface ILogger
    {
        void WriteLog(string message, string path, LoggerFile destination, Exception ex);
    }

    internal class Logger : ILogger
    {
        public void WriteLog(string message, string path, LoggerFile destination, Exception ex)
        {
            var t = new Task(() => { LogData(message, path, destination, ex); });
            t.Start();
            t.Wait();
        }

        /// <summary>
        /// LogData to File
        /// </summary>
        /// <param name="message"></param>
        /// <param name="path"></param>
        /// <param name="destination"></param>
        /// <param name="ex"></param>
        private static void LogData(string message, string path, LoggerFile destination, Exception ex)
        {
            CreateWindowsEvent(message, destination, ex);

            //Test if LogData Directory Exists
            if (!new DirectoryInfo(path + "\\" + destination + "\\").Exists)
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\" + destination + "\\");
            }

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\{destination}\\{DateTime.Now:yyyy-MM-dd}.csv")
                .CreateLogger();

            switch (destination)
            {
                case LoggerFile.Log:
                    Log.Logger.Information($"{message}");
                    break;

                case LoggerFile.Error:
                    Log.Logger.Error($"{message}. Error: {ex}");
                    break;
            }
            Log.CloseAndFlush();
        }

        private static void CreateWindowsEvent(string message, LoggerFile destination, Exception ex)
        {
            var title = Assembly.GetEntryAssembly().GetName().Name;

            try
            {
                if (!EventLog.SourceExists(title))
                {
                    EventLog.CreateEventSource(source: title, logName: "Application");
                }

                using var eventLog = new EventLog("Application");
                eventLog.Source = title;
                switch (destination)
                {
                    case LoggerFile.Log:

                        eventLog.WriteEntry(message, EventLogEntryType.Information, 1000, 0);
                        break;

                    case LoggerFile.Error:
                        eventLog.WriteEntry(message, EventLogEntryType.Error, 1000, 0);

                        break;
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error($"{message}. Error: {e}");
            }
        }
    }
}
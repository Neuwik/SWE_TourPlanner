using System;
using System.Diagnostics;

namespace SWE_TourPlanner_WPF.BusinessLayer.Logging
{
    public static class LoggerFactory
    {
        public static readonly string LoggerConfigPath = "Configs/log4net.config";

        public static ILoggerWrapper GetLogger()
        {
            StackTrace stackTrace = new(1, false); //Captures 1 frame, false for not collecting information about the file
            var type = stackTrace.GetFrame(1).GetMethod().DeclaringType;
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(appDir, LoggerConfigPath);
            return Log4NetWrapper.CreateLogger(filePath, type.FullName);
        }
    }
}

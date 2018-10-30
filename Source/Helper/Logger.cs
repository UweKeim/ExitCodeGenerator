namespace ExitCodeGenerator.Helper
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    internal static class Logger
    {
        public static void LogError(string text)
        {
            LogError((object)text);
        }

        public static void LogError(object text)
        {
            var c = Console.ForegroundColor;
            if (_logToConsole) Console.ForegroundColor = ConsoleColor.Red;

            if (_logToConsole) Console.Error.WriteLine(text);
            doWriteToFile(@"ERROR", text?.ToString(), new object[] { string.Empty });

            if (_logToConsole) Console.ForegroundColor = c;
        }

        public static void LogInfo(string text)
        {
            LogInfo((object)text);
        }

        public static void LogInfo(object text)
        {
            var c = Console.ForegroundColor;
            if (_logToConsole) Console.ForegroundColor = ConsoleColor.Blue;

            if (_logToConsole) Console.Out.WriteLine(text);
            doWriteToFile(@"INFO", text?.ToString(), new object[] { string.Empty });

            if (_logToConsole) Console.ForegroundColor = c;
        }

        public static void Log()
        {
            Log(string.Empty);
        }

        public static void Log(string text)
        {
            Log((object)text);
        }

        public static void Log(object text)
        {
            if (_logToConsole) Console.WriteLine(text);
            doWriteToFile(@"INFO", text?.ToString(), new object[] { string.Empty });
        }

        private static readonly object TypeLock = new object();
        private static bool _isFirstCall = true;
        private static bool _logToFile = true;
        private static bool _logToConsole = true;

        private static string LogFilePath => Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, @".log");

        private static void doWriteToFile(string type, string text, object[] args)
        {
            if (!_logToFile) return;

            // Mehrfach probieren, weil anderer PROZESS blockieren kann.
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    lock (TypeLock)
                    {
                        var directory = Path.GetDirectoryName(LogFilePath);
                        if (directory != null) Directory.CreateDirectory(directory);

                        if (_isFirstCall)
                        {
                            if (File.Exists(LogFilePath)) File.Delete(LogFilePath);
                            _isFirstCall = false;
                        }

                        File.AppendAllText(
                            LogFilePath,
                            string.Format(
                                @"{0} [{1}] ({2}-{3}) {4}",
                                DateTime.Now,
                                type,
                                Process.GetCurrentProcess().Id,
                                Thread.CurrentThread.ManagedThreadId,
                                string.Format(text, args)) + Environment.NewLine);

                        return;
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(50 * (i + 1) * 5);
                }
            }
        }

        public static void NotifyKeepLogFile()
        {
            _isFirstCall = false;
        }

        public static void NotifyDoNotLogToFile()
        {
            _logToFile = false;
        }

        public static void NotifyDoNotLogToConsole()
        {
            _logToConsole = false;
        }
    }
}
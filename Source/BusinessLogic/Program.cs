namespace ExitCodeGenerator.BusinessLogic
{
    using Helper;
    using System;
    using System.Threading;

    internal static class Program
    {
        private static int Main(string[] args)
        {
            // Das hier mus vor dem ersten Log-Aufruf geschehen.
            var keepLogFileRaw = read(@"--keeplogfiles", args);
            if (!bool.TryParse(keepLogFileRaw, out var keepLogFile)) keepLogFile = false;

            var wantLogRaw = read(@"--log", args);
            if (!bool.TryParse(wantLogRaw, out var wantLog)) wantLog = false;

            var wantQuietRaw = read(@"--quiet", args);
            if (!bool.TryParse(wantQuietRaw, out var wantQuiet)) wantQuiet = false;

            if (keepLogFile) Logger.NotifyKeepLogFile();
            if (!wantLog) Logger.NotifyDoNotLogToFile();
            if (wantQuiet) Logger.NotifyDoNotLogToConsole();

            // --

            Logger.Log();
            Logger.Log(@"Tool to sleep for a given amount of milliseconds and then return a given exit code.");
            Logger.Log(@"Intended as a mockup for testing external application calls.");
            Logger.Log();
            Logger.Log(@"Developed by https://www.zeta-producer.com");
            Logger.Log();
            Logger.Log(@"Syntax:");
            Logger.Log(
                @"    exitcode-generator.exe [--exitcode=<code>] [--sleep=<milliseconds>] [--log=<true|false>] [--keeplogfiles=<true|false>] [--quiet=<true|false>]");
            Logger.Log();
            Logger.Log(@"--exitcode=<code>:");
            Logger.Log(@"    Optional. Specify an integer exit code, positive or negative or zero to");
            Logger.Log(@"    return from the application. If none is specified, zero is returned.");
            Logger.Log();
            Logger.Log(@"--sleep=<milliseconds>:");
            Logger.Log(@"    Optional. Specify an positive amout of milliseconds to sleep before exiting.");
            Logger.Log(@"    If none is specified, the application returns immediately.");
            Logger.Log();
            Logger.Log(@"--log=<true|false>:");
            Logger.Log(@"    Optional. Specify whether to log to a file at all.");
            Logger.Log(@"    If not specified, the default value is false.");
            Logger.Log();
            Logger.Log(@"--keeplogfile=<true|false>:");
            Logger.Log(@"    Optional. Specify whether to clear any existing log file upon program start.");
            Logger.Log(@"    If not specified, the default value is true. Useful for calling the program");
            Logger.Log(@"    multiple times and having one large cumulated log file.");
            Logger.Log();
            Logger.Log(@"--quiet=<true|false>:");
            Logger.Log(@"    Optional. Specify whether to output anything to the console at all.");
            Logger.Log(@"    If not specified, the default value is false, meaning it does output text.");
            Logger.Log();
            Logger.Log();

            // --

            try
            {
                var rawExitCode = read(@"--exitcode", args) ?? string.Empty;
                var exitCode = int.TryParse(rawExitCode, out var ec) ? ec : 0;

                var rawSleep = read(@"--sleep", args) ?? string.Empty;
                var sleep = int.TryParse(rawSleep, out var sl) ? sl : 0;

                // --

                Logger.Log(@"Understood/calculated the following parameters:");
                Logger.Log();
                Logger.Log($@"    Want log       = '{wantLog}'");
                Logger.Log($@"    Keep log files = '{keepLogFile}'");
                Logger.Log($@"    Quiet          = '{wantQuiet}'");
                Logger.Log($@"    Exit code      = {exitCode}");
                Logger.Log($@"    Sleep          = {sleep} milliseconds");
                Logger.Log();

                // --
                // Die eigentliche Business-Logik.

                Thread.Sleep(sleep);

                Logger.Log(@"Finished.");

                return exitCode;
            }
            catch (Exception x)
            {
                Logger.LogError(x);

                Logger.Log(@"Finished with errors.");
                return -1;
            }
        }

        private static string read(string key, string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.StartsWith(key))
                {
                    var value = arg.Substring(key.Length);
                    value = trim(value);

                    return value;
                }
            }

            return null;
        }

        private static string trim(string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.Trim('=', '\'', '"', ' ', '\t', '\r', '\n');
        }
    }
}
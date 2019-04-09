using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace TLIB
{
    public static class Log
    {
        public static string LogFile { get; set; }
        public static int InMemoryLogMaxCount { get; set; } = Int32.MaxValue;
        public static ObservableCollection<string> InMemoryLog { get; } = new ObservableCollection<string>();

        public static bool IsFileLogEnabled => LogFile != null;
        public static bool IsInMemoryLogEnabled { get; set; }
        public static bool IsConsoleLogEnabled { get; set; }

        public static event EventHandler<string> DisplayMessageRequested;
        public static LogMode Mode = LogMode.Moderat;
        public enum LogType
        {
            None,
            Info,
            Error,
            Question
        }
        public static void Write(string msg, Exception ex = null, LogType logType = LogType.Info, bool InterruptUser = false, [CallerLineNumber] int Number = 0, [CallerMemberName] string Caller = "")
        {
            if (Mode == LogMode.Moderat)
            {
                msg = DateTime.Now + " " + msg;
            }
            msg = logType + " " + msg;
            if (Mode == LogMode.Verbose)
            {
                msg += DateTime.Now + " " + msg + " (" + Caller + ":" + Number + ")";
            }
            if (ex != null)
            {
                msg += Environment.NewLine
                + "\t" + ex.Message
                + Environment.NewLine
                + "\t" + ex.StackTrace;
            }
            if (IsInMemoryLogEnabled)
            {
                try
                {
                    InMemoryLog.Add(msg);
                    if (InMemoryLog.Count > InMemoryLogMaxCount)
                    {
                        InMemoryLog.RemoveAt(0);
                    }
                }
                catch (Exception)
                {
                }
            }
            if (IsConsoleLogEnabled)
            {
                try
                {
                    Console.WriteLine(msg);
                }
                catch (Exception)
                {
                }
            }
            if (IsFileLogEnabled)
            {
                try
                {
                    File.AppendAllText(LogFile, msg + Environment.NewLine);
                }
                catch (Exception)
                {
                }
            }
            if (InterruptUser)
            {
                DisplayMessageRequested?.Invoke(msg, msg);
            }
        }
    }
    public enum LogMode
    {
        Minimal,
        Moderat,
        Verbose
    }
}

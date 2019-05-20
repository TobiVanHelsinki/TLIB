using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace TLIB
{
    public enum LogMode
    {
        Minimal,
        Moderat,
        Verbose
    }
    public enum LogType
    {
        Info,
        Warning,
        Error,
        Question
    }
    public delegate void LogEventHandler(LogMessage logmessage);
    public struct LogMessage
    {
        public LogType LogType;
        public string Message;
        public DateTime ArrivedAt;
        public string Caller;
        public Exception ThrownException;
        public string CombinedMessage;

        public LogMessage(LogType logType, string message, DateTime arrivedAt, string caller, Exception thrownException, string combinedMessage)
        {
            LogType = logType;
            Message = message;
            ArrivedAt = arrivedAt;
            Caller = caller;
            ThrownException = thrownException;
            CombinedMessage = combinedMessage;
        }
    }
    public static class Log
    {
        public static string LogFile { get; set; }
        public static int InMemoryLogMaxCount { get; set; } = Int32.MaxValue;
        public static List<string> InMemoryLog { get; } = new List<string>();

        public static bool IsFileLogEnabled => LogFile != null;
        public static bool IsInMemoryLogEnabled { get; set; }
        public static bool IsConsoleLogEnabled { get; set; }

        public static event LogEventHandler DisplayMessageRequested;
        public static event LogEventHandler NewLogArrived;
        public static LogMode Mode = LogMode.Moderat;
        public static void Write(string msg, Exception ex = null, LogType logType = LogType.Info, bool InterruptUser = false, [CallerLineNumber] int Number = 0, [CallerMemberName] string Caller = "")
        {
            var ArrivedAt = DateTime.Now;
            var CombinedMessage = logType + " \"" + msg + "\"";
            if (Mode == LogMode.Moderat)
            {
                CombinedMessage = DateTime.Now + " " + CombinedMessage;
            }
            if (Mode == LogMode.Verbose)
            {
                CombinedMessage += ArrivedAt + " " + CombinedMessage + " (" + Caller + ":" + Number + ")";
            }
            if (ex != null)
            {
                AddDetails(ex, ref CombinedMessage);
            }
            AddToLog(CombinedMessage);
            var logentry = new LogMessage(logType, msg, ArrivedAt, Caller + ":" + Number, ex, CombinedMessage);
            if (ex != null)
            {
                logentry.LogType = LogType.Error;
            }
            NewLogArrived?.Invoke(logentry);
            if (InterruptUser)
            {
                DisplayMessageRequested?.Invoke(logentry);
            }
        }

        private static void AddDetails(Exception ex, ref string CombinedMessage)
        {
            CombinedMessage += Environment.NewLine
            + "\t\"" + ex.Message + "\""
            + Environment.NewLine
            + "\t" + ex.StackTrace;
            if (ex.InnerException is Exception x2)
            {
                AddDetails(x2, ref CombinedMessage);
            }
        }

        static void AddToLog(string msg)
        {
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
        }
    }
}

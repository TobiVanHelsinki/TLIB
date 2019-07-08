using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace TLIB
{
    /// <summary>
    /// Detaildegree of an logmessage
    /// </summary>
    public enum LogMode
    {
        /// <summary>
        /// just the message
        /// </summary>
        Minimal,
        /// <summary>
        /// add datetime
        /// </summary>
        Moderat,
        /// <summary>
        /// all details
        /// </summary>
        Verbose
    }
    /// <summary>
    /// type of the log may determine color or how it is handeld
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// standard
        /// </summary>
        Info,
        /// <summary>
        /// possible unwanted behavior noticed
        /// </summary>
        Warning,
        /// <summary>
        /// unwanted behavior noticed
        /// </summary>
        Error,
        /// <summary>
        /// for the future, displays a question to the user and provide an answere
        /// </summary>
        Question
    }

    /// <summary>
    /// The used event handler for new arrived logs
    /// </summary>
    /// <param name="logmessage"></param>
    public delegate void LogEventHandler(LogMessage logmessage);
/// <summary>
/// the wrapper for an log
/// </summary>
    public struct LogMessage
    {
        /// <summary>
        /// What kind of log is this
        /// </summary>
        public LogType LogType { get; set; }
        /// <summary>
        /// What does it say
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// When did it arrived
        /// </summary>
        public DateTime ArrivedAt { get; set; }
        /// <summary>
        /// Who send this log
        /// </summary>
        public string Caller { get; set; }
        /// <summary>
        /// Had it have an exception
        /// </summary>
        public Exception ThrownException { get; set; }
        /// <summary>
        /// A string containing the message and for example the arrivetime or the priority. depends at the LogType
        /// </summary>
        public string CombinedMessage { get; set; }

        /// <summary>
        /// Create a log message
        /// </summary>
        /// <param name="logType">What kind of log is this</param>
        /// <param name="message">What does it say</param>
        /// <param name="arrivedAt">When did it arrived</param>
        /// <param name="caller">Who send this log</param>
        /// <param name="thrownException">Had it have an exception</param>
        /// <param name="combinedMessage">A short string for this instance</param>
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

    /// <summary>
    /// Provides a basic logsystem. 
    /// You can log to file or just hold the log messages in memory.
    /// Use the "In*Enabled" Properties to choose, how to log.
    /// Specify a LogFile
    /// </summary>
    public static class Log
    {
        static string _LogFile;
        /// <summary>
        /// The path to the file, the logs are written in. Note: You have to have access to this file. System.IO.File.AppendAllText is used for file operations.
        /// If you set a filepath not null, the IsFileLogEnabled is automaticly set.
        /// </summary>
        public static string LogFile { get => _LogFile; set { _LogFile = value; IsFileLogEnabled = value != null; } }
        /// <summary>
        /// How many items shall be stored in memory?
        /// </summary>
        public static int InMemoryLogMaxCount { get; set; } = Int32.MaxValue;
        /// <summary>
        /// save place for the logs. if full, the earliest messages are removed. You can edit the list as you whish.
        /// </summary>
        public static List<string> InMemoryLog { get; } = new List<string>();

        static bool _IsFileLogEnabled;
        /// <summary>
        /// Enables the file Log. Just possible if a Filepath is specified
        /// </summary>
        public static bool IsFileLogEnabled
        {
            get { return _IsFileLogEnabled; }
            set { _IsFileLogEnabled = LogFile != null ? value : false; }
        }
        /// <summary>
        /// Enables the in memory Log.
        /// </summary>
        public static bool IsInMemoryLogEnabled { get; set; }
        /// <summary>
        /// Enables std console output
        /// </summary>
        public static bool IsConsoleLogEnabled { get; set; }

        /// <summary>
        /// Occures, when a log arrived, that needs to notify the user. You may display a MessageBox, a PopUp or ignore it.
        /// </summary>
        public static event LogEventHandler DisplayMessageRequested;
        /// <summary>
        /// Occures, when a new log arrived
        /// </summary>
        public static event LogEventHandler NewLogArrived;
        /// <summary>
        /// How detailled shall the message be
        /// </summary>
        public static LogMode Mode = LogMode.Moderat;

        /// <summary>
        /// Adds a new log
        /// </summary>
        /// <param name="msg">Your message</param>
        /// <param name="ex">optional, an exception</param>
        /// <param name="logType">optional, a special logtype</param>
        /// <param name="InterruptUser">optional, request, that a user get's rigth away notified</param>
        /// <param name="Number">automatic, the line number from wich the call came</param>
        /// <param name="Caller">automatic, the membername from wich the call came</param>
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

        static void AddDetails(Exception ex, ref string CombinedMessage)
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

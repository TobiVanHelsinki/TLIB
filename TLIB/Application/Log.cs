using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TLIB
{
    /// <summary>
    /// The used event handler for new arrived logs
    /// </summary>
    /// <param name="logmessage"></param>
    public delegate void LogEventHandler(LogMessage logmessage);

    /// <summary>
    /// The used event handler for new arrived choices
    /// </summary>
    /// <param name="title">A short but strong text that describes your intend</param>
    /// <param name="text">The Question you have, the feedback you want, etc.</param>
    /// <param name="choice">an object where the selected answere should be places</param>
    /// <param name="choices">an array of Choices</param>
    public delegate void ChoiceEventHandler(string title, string text, ResultCallback choice, params string[] choices);

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
        /// Occures, when a Question arrives. The handler then should give a user a choice and send the answere back to the .
        /// </summary>
        public static event ChoiceEventHandler DisplayQuestionRequested;
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
        public static void Write(string msg, Exception ex, LogType logType = LogType.Info, bool InterruptUser = false, [CallerLineNumber] int Number = 0, [CallerMemberName] string Caller = "")
        {
            var ArrivedAt = DateTime.Now;
            var CombinedMessage = logType + " \"" + msg + "\"";
            if (Mode == LogMode.Moderat)
            {
                CombinedMessage = ArrivedAt + " " + CombinedMessage;
            }
            else if (Mode == LogMode.Verbose)
            {
                CombinedMessage = ArrivedAt + " " + CombinedMessage + " (" + Caller + ":" + Number + ")";
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

        /// <summary>
        /// Adds a new log
        /// </summary>
        /// <param name="msg">Your message</param>
        /// <param name="logType">optional, a special logtype</param>
        /// <param name="InterruptUser">optional, request, that a user get's rigth away notified</param>
        /// <param name="number">automatic, the line number from wich the call came</param>
        /// <param name="caller">automatic, the membername from wich the call came</param>
        public static void Write(string msg, LogType logType, bool InterruptUser = false, [CallerLineNumber] int number = 0, [CallerMemberName] string caller = "")
        {
            Write(msg, null, logType, InterruptUser, number, caller);
        }

        /// <summary>
        /// Adds a new log
        /// </summary>
        /// <param name="msg">Your message</param>
        /// <param name="InterruptUser">optional, request, that a user get's rigth away notified</param>
        /// <param name="number">automatic, the line number from wich the call came</param>
        /// <param name="caller">automatic, the membername from wich the call came</param>
        public static void Write(string msg, bool InterruptUser = false, [CallerLineNumber] int number = 0, [CallerMemberName] string caller = "")
        {
            Write(msg, null, LogType.Info, InterruptUser, number, caller);
        }

        ///// <summary>
        ///// Adds a new log
        ///// </summary>
        ///// <param name="msg">Your message</param>
        ///// <param name="number">automatic, the line number from wich the call came</param>
        ///// <param name="caller">automatic, the membername from wich the call came</param>
        //public static void Write(string msg, [CallerLineNumber] int number = 0, [CallerMemberName] string caller = "")
        //{
        //    Write(msg, null, LogType.Info, false, number, caller);
        //}

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

        /// <summary>
        /// Use this method to obtain feedback from a user. With the thrid parameter you can pass the actions you want to have executed when the corrosponding choice is selected
        /// </summary>
        /// <param name="title">A short but strong text that describes your intend</param>
        /// <param name="text">The Question you have, the feedback you want, etc.</param>
        /// <param name="options">and array of Choice-Result-Tuples</param>
        public static void DisplayChoice(string title, string text, params (string, Action)[] options)
        {
            var choice = new ResultCallback(
                (x) =>
                {
                    string answere;
                    if (x < options.Count() && x >= 0)
                    {
                        options[x].Item2?.Invoke();
                        answere = options[x].Item1;
                    }
                    else
                    {
                        answere = "Nothing";
                    }
                    Write(title + "\"  was asked: \"" + text + "\". The Answere is: \"" + answere, LogType.Question, false, 0, "");
                }
                );
            DisplayQuestionRequested?.Invoke(title, text, choice, options.Select(x => x.Item1).ToArray());
        }
    }
}

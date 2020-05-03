//Author: Tobi van Helsinki

using System;

namespace TLIB
{
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
        /// A string containing the message and for example the arrivetime or the priority. depends
        /// at the LogType
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

        /// <summary>
        /// This custom ToString Mehtod returns the CombinedMessage, to maintain compatibility with
        /// prior Versions
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CombinedMessage;
        }
    }
}
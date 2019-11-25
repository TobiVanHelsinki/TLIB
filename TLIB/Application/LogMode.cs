//Author: Tobi van Helsinki

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
        Plain,

        /// <summary>
        /// add LogMode
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
}
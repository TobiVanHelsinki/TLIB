///Author: Tobi van Helsinki

namespace TLIB
{
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
        /// Displays a question to the user and provide an answere, to use with the Choose Methode
        /// </summary>
        Question,

        /// <summary>
        /// The success is a log type with special font or color
        /// </summary>
        Success
    }
}
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
        /// for the future, displays a question to the user and provide an answere
        /// </summary>
        Question
    }
}

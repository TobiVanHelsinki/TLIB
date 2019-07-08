using System;

namespace TLIB
{
    /// <summary>
    /// An Exception that indicates, that an behavior is acceptable. e.g.: 
    /// * a user closed a file chooser dialog without choosing a file
    /// * 
    /// </summary>
    public class IsOKException : Exception
    {
        /// <summary>
        /// Default Exception ctor
        /// </summary>
        public IsOKException()
        {
        }

        /// <summary>
        /// Default Exception ctor
        /// </summary>
        public IsOKException(string message) : base(message)
        {
        }

        /// <summary>
        /// Default Exception ctor
        /// </summary>
        public IsOKException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

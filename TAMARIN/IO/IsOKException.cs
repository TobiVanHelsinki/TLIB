using System;

namespace TLIB
{
    [Serializable]
    public class IsOKException : Exception
    {
        public IsOKException()
        {
        }

        public IsOKException(string message) : base(message)
        {
        }

        public IsOKException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

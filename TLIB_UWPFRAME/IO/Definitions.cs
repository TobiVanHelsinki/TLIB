using System;
using System.Collections.Generic;
using System.Text;

namespace TLIB_UWPFRAME.IO
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

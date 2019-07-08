using System;

namespace TAPPLICATION.Model
{
    public class SettingNotPresentException : Exception
    {
        public SettingNotPresentException()
        {
        }

        public SettingNotPresentException(string message) : base(message)
        {
        }

        public SettingNotPresentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

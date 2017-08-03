using System;

namespace TLIB.Model
{
    public class Notification
    {
        public string strMessage;
        public bool bIsRead;
        public Exception ThrownException;
        public DateTime DateTime = DateTime.Now;

        public Notification(string istrMessage, Exception iExeption = null)
        {
            strMessage = istrMessage;
            ThrownException = iExeption;
        }
    }
}

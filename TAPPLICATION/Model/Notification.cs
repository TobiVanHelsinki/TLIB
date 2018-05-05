using System;

namespace TAPPLICATION.Model
{
    public class Notification
    {
        public string Message;
        public bool IsRead;
        public bool IsLight;
        public Exception ThrownException;
        public DateTime OccuredAt = DateTime.Now;

        public Notification(string mess)
        {
            Message = mess;
        }
        public Notification(string mess, Exception ex) : this(mess)
        {
            ThrownException = ex;
        }
    }
}

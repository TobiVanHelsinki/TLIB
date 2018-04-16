using System;

namespace TAPPLICATION.Model
{
    public class Notification
    {
        public string Message;
        public bool IsRead;
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
        public Notification(string format, params string[] args) : this(string.Format(format, args))
        {
        }
        public Notification(string format, Exception ex, params string[] args) : this(string.Format(format, args), ex)
        {
        }
    }
}

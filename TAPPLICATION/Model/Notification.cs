using System;

namespace TAPPLICATION.Model
{
    public class Notification
    {
        public string Message = "";
        public bool IsRead = false;
        public bool IsLight = true;
        public int ShownTime = 6000;
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

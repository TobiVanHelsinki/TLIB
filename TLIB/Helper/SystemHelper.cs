using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLIB
{
    public static class SystemHelper
    {
        public static async Task SleepMilliSeconds(int ms)
        {
#if WINDOWS_UWP
            await Task.Delay(TimeSpan.FromMilliseconds(ms));
#elif WINDOWS_DESKTOP
            Thread.Sleep(ms);
#endif
        }

//        public static void WriteLine(string s)
//        {
//#if WINDOWS_UWP
//            System.Diagnostics.Debug.WriteLine(s);
//#elif WINDOWS_DESKTOP
//            Console.Write(s);
//#endif
//        }

        public static void WriteTime() => System.Diagnostics.Debug.Write(DateTime.Now);
        public static void WriteLineTime() => System.Diagnostics.Debug.WriteLine(DateTime.Now);

        public static void WriteLine(object s = null) => System.Diagnostics.Debug.WriteLine(s);
        public static void WriteLine(string f = null, params object[] args) => System.Diagnostics.Debug.WriteLine(f, args);
        public static void Write(object s = null) => System.Diagnostics.Debug.Write(s);
        public static void Write(string f = null, params object[] args) => System.Diagnostics.Debug.Write(String.Format(f, args));
    }
}
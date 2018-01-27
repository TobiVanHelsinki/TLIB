using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLIB.CrossPlatformHelper
{
    internal static class Threading
    {
        public static async Task SleepMilliSeconds(int ms)
        {
#if WINDOWS_UWP
            await Task.Delay(TimeSpan.FromMilliseconds(ms));
#elif WINDOWS_DESKTOP
            Thread.Sleep(ms);
#endif

        }
    }
    internal static class PrintOut
    {
        public static void WriteLine( string s)
        {
#if WINDOWS_UWP
            System.Diagnostics.Debug.WriteLine(s);
#elif WINDOWS_DESKTOP
            Console.Write(s);
#endif

        }
    }
}

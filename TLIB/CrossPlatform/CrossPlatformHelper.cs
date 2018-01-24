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
#if UWP
            await Task.Delay(TimeSpan.FromMilliseconds(ms));
#else
            Thread.Sleep(ms);
#endif

        }
    }
    internal static class PrintOut
    {
        public static void WriteLine( string s)
        {
#if UWP
            System.Diagnostics.Debug.WriteLine(s);
#else
            Console.Write(s);
#endif

        }
    }
}

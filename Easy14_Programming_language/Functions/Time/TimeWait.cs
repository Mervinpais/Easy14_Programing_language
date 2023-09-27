using System;
using System.Threading;

namespace Easy14_Programming_Language
{
    public static class TimeWait
    {
        public static void Interperate(string line)
        {
            if (!ItemChecks.IsInt(line)) { return; }

            try { Thread.Sleep(Convert.ToInt32(line) * 1000); }
            catch (Exception e) { ErrorReportor.ConsoleLineReporter.Error(e.Message); }
        }
    }
}
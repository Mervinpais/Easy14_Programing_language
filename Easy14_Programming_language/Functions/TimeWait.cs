using System;
using System.Threading;

namespace Easy14_Programming_Language
{
    public static class TimeWait
    {
        public static void Interperate(string line)
        {
            if (ItemChecks.IsString(line)) { return; }

            try { Thread.Sleep(Convert.ToInt32(line) * 1000); }
            catch { ErrorReportor.ConsoleLineReporter.Error("Input provide is not a valid integer"); } //only possible probloem i think 
        }
    }
}
using System;
using System.Threading;

namespace Easy14_Programming_Language
{
    public static class TimeWait
    {
        public static void Interperate(string line)
        {
            line = line.Substring("Wait".Length);

            if (line.EndsWith(";"))
            {
                line = line.Substring(0, line.Length - 1);    //SYNTAX CHECK 1
            }
            else
            {
                return;
            }

            if (line.StartsWith("(") && line.EndsWith(")"))
            {
                line = line.Substring(1, line.Length - 2);    //SYNTAX CHECK 2
            }
            else
            {
                return;
            }

            if (line.StartsWith("\"") && line.EndsWith("\""))
            {
                line = line.Substring(1, line.Length - 2);
                Console.WriteLine(line);
            }

            try
            {
                Thread.Sleep(Convert.ToInt32(line) * 1000);
            }
            catch
            {
                ErrorReportor.ConsoleLineReporter.Error("Input provide is not a valid integer"); //only possible probloem i think
            }
        }
    }
}
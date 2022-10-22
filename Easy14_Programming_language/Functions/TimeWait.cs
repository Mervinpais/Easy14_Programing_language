using System;
using System.Threading;

namespace Easy14_Programming_Language
{
    public static class TimeWait
    {
        public static void Interperate(string code_part, int line_count)
        {
            string textToPrint;
            code_part = code_part.TrimStart();
            if (code_part.StartsWith("wait"))
            {
                code_part = code_part[4..];
            }
            else
            {
                CSharpErrorReporter.ConsoleLineReporter.Error("Cant Interperate Line due to missing keyword \"wait\" ");
                return;
            }

            if (code_part.StartsWith("(") && code_part.EndsWith(");"))
            {
                code_part = code_part[1..^2];
            }

            textToPrint = code_part;
            try
            {
                var n = textToPrint;
                decimal n_decimal = Convert.ToDecimal(n);
                int amountTowait = Convert.ToInt32(n_decimal * 1000);
                Thread.Sleep(amountTowait);
            }
            catch
            {
                CSharpErrorReporter.ConsoleLineReporter.Error($"Cant implicitly Convert from (string/bool) to int/decimal in 'wait'\n  Error was located on Line {line_count}");
            }
        }
    }
}
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
            code_part = code_part[5..];

            code_part = code_part[..^1];
            if (code_part.EndsWith(");"))
            {
                code_part = code_part[..^1];
            }

            textToPrint = code_part;
            try
            {
                var n = textToPrint;
                decimal n_decimal = Convert.ToDecimal(n.Substring(0, n.Length - 1));
                int amountTowait = Convert.ToInt32(n_decimal * 1000);
                Thread.Sleep(amountTowait);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR; Cant implicitly Convert from (string/bool) to int/decimal in 'wait'\n  Error was located on Line {line_count}");
            }
        }
    }
}
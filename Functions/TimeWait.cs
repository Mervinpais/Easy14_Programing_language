using System;
using System.Threading;

namespace Easy14_Programming_Language
{
    class TimeWait
    {
        public void interperate(string code_part, int line_count)
        {
            string code_part_unedited;
            string textToPrint;

            code_part_unedited = code_part;
            code_part = code_part.TrimStart();
            code_part = code_part.Substring(5);
            if (code_part.EndsWith(")"))
                code_part = code_part.Substring(0, code_part.Length - 1);
            if (code_part.EndsWith(");"))
                code_part = code_part.Substring(0, code_part.Length - 2);
            textToPrint = code_part;
            if (textToPrint.StartsWith("random.range("))
            {
                string text = textToPrint;
                text = text.Replace("random.range(", "");
                text = text.Replace(")", "");
                int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                Random rnd = new Random();
                int rnd_number = rnd.Next(number1 * 1000, number2 * 1000);
                Thread.Sleep(rnd_number);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("waited for " + Math.Round(Convert.ToDecimal(rnd_number / 1000)) + " seconds");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                try
                {
                    var n = textToPrint;
                    decimal n_decimal = Convert.ToDecimal(n);
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
}
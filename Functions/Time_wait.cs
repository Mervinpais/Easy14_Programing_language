using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;
using System.Linq;
using System.Management;

namespace Easy14_Coding_Language
{
    class Time_wait
    {
        public void interperate(string code_part, int line_count)
        {
            string code_part_unedited;
            string textToPrint;

            code_part_unedited = code_part;
            code_part = code_part.TrimStart();
            code_part = code_part.Substring(5);
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
                    int n_decimal = Convert.ToInt32(n);
                    Thread.Sleep(n_decimal * 500);
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; Cant implicitly Convert from (string/bool/decimal) to int in 'wait'\n  Error was located on Line {line_count}");
                }
            }
        }
    }
}
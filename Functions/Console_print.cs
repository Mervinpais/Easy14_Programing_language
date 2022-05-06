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
    class Console_print
    {
        public void interperate(string code_part , string[] textArray, string fileloc)
        {
            string code_part_unedited;
            string textToPrint;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();

            if (code_part.StartsWith($"print("))
            {
                bool foundUsing = false;
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                foreach (string x in someLINEs)
                {
                    if (x.StartsWith("using"))
                    {
                        if (x == "using Console;")
                        {
                            foundUsing = true;
                            break;
                        }
                    }
                    if (x == code_part)
                    {
                        break;
                    }
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'print' without its reference  (Use Console.print(\"*Text To Print*\") to fix this error :)");
                }
            }
            else if (code_part.StartsWith($"Console.print(")) { }

            if (code_part_unedited.StartsWith($"Console.print("))
                code_part = code_part.Substring(14);
            else if (code_part_unedited.StartsWith($"print("))
                code_part = code_part.Substring(6);

            code_part = code_part.Substring(0, code_part.Length - 2);
            textToPrint = code_part;
            if (textToPrint == "Time.Now")
            {
                Console.WriteLine(DateTime.Now);
            }
            else if (textToPrint.StartsWith("random.range("))
            {
                string text = textToPrint;
                text = text.Replace("random.range(", "").Replace(")", "");
                int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                Random rnd = new Random();
                Console.WriteLine(rnd.Next(number1, number2));
            }
            else if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith('"'.ToString()))
            {
                Console.WriteLine(textToPrint.Replace('"'.ToString(), ""));
            }
            else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
            {
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                foreach (string file in files)
                {
                    if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == textToPrint.Replace(".txt", ""))
                    {
                        var contentInFile = File.ReadAllText(file);
                        Console.WriteLine(contentInFile.ToString());
                        break;
                    }
                }
            }
        }
    }
}
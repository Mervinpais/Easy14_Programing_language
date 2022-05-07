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
    class Console_input
    {
        public void interperate(string code_part, string[] textArray, string fileloc)
        {
            string code_part_unedited;
            string textToPrint;
            
            bool foundUsing = false;
            if (code_part.StartsWith($"input("))
            {
                string[] someLines = null;
                if (textArray == null && fileloc != null) someLines = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLines = textArray;

                foreach (string x in someLines)
                {
                    if (x == code_part)
                    {
                        break;
                    }
                    if (x.StartsWith("using"))
                    {
                        if (x == "using Console;")
                        {
                            foundUsing = true;
                            break;
                        }
                    }
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'input' without its reference  (Use Console.input(\"*Text*\") to fix this error :)");
                    return;
                }
            }
            else if (code_part.StartsWith($"Console.input(")) { }

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();
            code_part = code_part.Substring(14);
            code_part = code_part.Substring(0, code_part.Length - 2);
            textToPrint = code_part;
            if (foundUsing == false)
            {
                if (textToPrint == "Time.Now")
                {
                    Console.WriteLine(DateTime.Now);
                }
                else if (textToPrint.StartsWith("random.range("))
                {
                    string text = textToPrint;
                    text = text.Replace("random.range(", "");
                    text = text.Replace(")", "");
                    int number1 = Convert.ToInt32(
                        text.Substring(0, text.IndexOf(",")).Replace(",", "")
                    );
                    int number2 = Convert.ToInt32(
                        text.Substring(text.IndexOf(",")).Replace(",", "")
                    );
                    Random rnd = new Random();
                    Console.WriteLine(rnd.Next(number1, number2));
                }
                else if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith('"'.ToString()))
                {
                    Console.WriteLine(textToPrint);
                }
                else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                {
                    string[] files = Directory.GetFiles(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                            + @$"\EASY14_Variables_TEMP"
                    );
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
                Console.Write(">");
                Console.ReadLine();
            }
        }
    }
}
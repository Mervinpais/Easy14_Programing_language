using System;
using System.Data;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class ConsolePrint
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static void Interperate(string code_part, string[] textArray = null, string fileloc = null, int lineNumber = -1)
        {
            code_part = code_part.TrimStart();
            string[] lines = { };
            if (textArray is not null && fileloc is null)
            {
                lines = textArray;
            }
            else if (textArray is null && fileloc is not null)
            {
                lines = File.ReadAllLines(fileloc);
            }

            bool foundUsing = false;
            if (code_part.StartsWith("print("))
            {
                foreach (string line in lines)
                {
                    string line_Trimmed = line.TrimStart().TrimEnd();
                    if (line_Trimmed == "using Console;" || line_Trimmed == "for Console get print;") { foundUsing = true; break; }
                    else if (line_Trimmed == code_part) { break; }
                }

                if (!foundUsing)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'print' without its reference  (Use Console.print(\"*Text To Print*\") to fix this error :)");
                    Console.ResetColor(); return;
                }
            }

            if (code_part.StartsWith("Console.print"))
            {
                code_part = code_part.Substring("Console.print".Length);
            }
            else if (code_part.StartsWith("print"))
            {
                code_part = code_part.Substring("print".Length);
            }

            //check if code_part contains +, -, *, / or %
            if (code_part.Contains("+")
                || code_part.Contains("-")
                || code_part.Contains("*")
                || code_part.Contains("/")
                || code_part.Contains("%"))
            {
                double result = Convert.ToDouble(new DataTable().Compute(code_part, null));
                Console.WriteLine(Convert.ToDouble(new DataTable().Compute(code_part, null)));
            }

            if (code_part.StartsWith("(")) code_part = code_part.Substring(1);
            if (code_part.EndsWith(");")) code_part = code_part.Substring(0, code_part.Length - 2);

            if (code_part.StartsWith("\""))
            {
                if (code_part.EndsWith("\""))
                {
                    Console.WriteLine(code_part.Substring(1, code_part.Length - 2));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The String '{code_part}' is not closed properly");
                    Console.ResetColor(); return;
                }
            }
            else if (code_part.EndsWith("\""))
            {
                if (code_part.StartsWith("\""))
                {
                    Console.WriteLine(code_part.Substring(1, code_part.Length - 2));
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The String '{code_part}' is not opened properly");
                    Console.ResetColor(); return;
                }
            }
            else if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\\EASY14_Variables_TEMP"))
            {
                if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\\EASY14_Variables_TEMP").Length != 0)
                {
                    string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP");
                    foreach (string file in files)
                    {
                        if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == code_part)
                        {
                            var contentInFile = File.ReadAllText(file).ToString();
                            Console.WriteLine(contentInFile);
                            break;
                        }
                    }
                }
            }


        }
    }
}
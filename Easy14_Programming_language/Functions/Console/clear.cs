using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class ConsoleClear
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static void Interperate(string code_part, string[] textArray, string fileloc)
        {
            string endOfStatementCode = ")";
            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons"))
                    endOfStatementCode.Equals(line.EndsWith("true") ? endOfStatementCode = ");" : endOfStatementCode = ")");
                break;
            }

            string code_part_unedited;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();
            bool foundUsing = false;
            if (code_part.StartsWith($"print(") && code_part.EndsWith(endOfStatementCode == ")" ? ")" : ");"))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to Console.clear()");
                    Console.ResetColor();
                    return;
                }
                foreach (string x in someLINEs)
                {
                    if (x.TrimStart().TrimEnd() == "using Console;" || x.TrimStart().TrimEnd() == "from Console get clear;")
                    {
                        foundUsing = true;
                        break;
                    }
                    if (x.TrimStart().TrimEnd() == code_part)
                    {
                        break;
                    }
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'clear' without its reference  (Use Console.clear()) to fix this error :)");
                    Console.ResetColor();
                    return;
                }
            }
            else if (code_part.StartsWith($"Console.clear(") && code_part.EndsWith(endOfStatementCode == ")" ? ")" : ");")) { }

            Console.Clear();

            //}
        }
    }
}
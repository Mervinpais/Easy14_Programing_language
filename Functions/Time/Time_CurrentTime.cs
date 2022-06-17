using System;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    class Time_CurrentTime
    {
        public string interperate(string code_part, string[] textArray, string fileloc)
        {
            string endOfStatementCode = ")";
            string[] configFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "") + "\\Application Code\\options.ini");
            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons"))
                    endOfStatementCode.Equals(line.EndsWith("true") ? endOfStatementCode = ");" : endOfStatementCode = ")");
                break;
            }

            string code_part_unedited = code_part;
            bool foundUsing = false;
            if (code_part.StartsWith("CurrentTime("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the CurrentTime function.");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return null;
                }
                
                foreach (string x in someLINEs)
                {
                    if (x.TrimStart().TrimEnd() == "using Time;")
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
                    Console.WriteLine($"ERROR; The Using 'Time' wasnt referenced to use 'CurrentTime' without its reference  (Use Time.CurrentTime() to fix this error :)");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return "<ERROR_USED_FUNCTION_WHICH_NEEDED_A_USING_NAMESPACE_BEFORE>";
                }
            }
            else if (code_part.StartsWith($"Time.CurrentTime(")) { }

            if (code_part_unedited.StartsWith($"Time.CurrentTime("))
                code_part = code_part.Substring(17);
            else if (code_part_unedited.StartsWith($"CurrentTime("))
                code_part = code_part.Substring(12);

            if (endOfStatementCode == ");")
                code_part = code_part.Substring(0, code_part.Length - 2);
            else
                code_part = code_part.Substring(0, code_part.Length - 1);
            
            DateTime date = DateTime.Now;
            string date_str = Convert.ToString(date);
            return date_str;

        }
    }
}
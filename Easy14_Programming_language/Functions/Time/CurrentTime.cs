using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class Time_CurrentTime
    {
        public static string Interperate(string code_part, string[] textArray)
        {
            string endOfStatementCode = ")";
            string[] configFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Application Code\\options.ini");
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
                string[] someLINEs = textArray;
                if (textArray == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the DeleteFile function.");
                    Console.ResetColor();
                    return "";
                }

                foreach (string usingStatements in someLINEs)
                {
                    if (usingStatements.TrimStart().TrimEnd() == "using Time;")
                    {
                        foundUsing = true;
                        break;
                    }
                    if (usingStatements.TrimStart().TrimEnd() == code_part)
                    {
                        break;
                    }
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Time' wasnt referenced to use 'CurrentTime' without its reference  (Use Time.CurrentTime() to fix this error :)");
                    Console.ResetColor();
                    return "<ERROR_USED_FUNCTION_WHICH_NEEDED_A_USING_NAMESPACE_BEFORE>";
                }
            }
            else if (code_part.StartsWith($"Time.CurrentTime(")) { }

            if (code_part_unedited.StartsWith($"Time.CurrentTime("))
                code_part = code_part.Substring(17);
            else if (code_part_unedited.StartsWith($"CurrentTime("))
                code_part = code_part.Substring(12);

            code_part = code_part.Substring(0, code_part.Length - 1);
            if (code_part.EndsWith(")"))
            {
                code_part = code_part.Substring(0, code_part.Length - 1);
            }

            DateTime date = DateTime.Now;
            string date_str = Convert.ToString(date);
            return date_str;

        }
    }
}
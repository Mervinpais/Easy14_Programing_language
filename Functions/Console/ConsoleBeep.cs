using System;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    class ConsoleBeep
    {
        public string interperate(string code_part, string[] textArray, string fileloc)
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
            if (code_part.StartsWith("Beep("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the Console.beep function.");
                    Console.ResetColor();
                    return "";
                }

                foreach (string x in someLINEs)
                {
                    if (x.TrimStart().TrimEnd() == "using Console;")
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
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'Beep' without its reference  (Use Console.Beep() to fix this error :)");
                    Console.ResetColor();
                    return "<ERROR_USED_FUNCTION_WHICH_NEEDED_A_USING_NAMESPACE_BEFORE>";
                }
            }
            else if (code_part.StartsWith($"Console.Beep(")) { }

            if (code_part_unedited.StartsWith($"Console.Beep("))
                code_part = code_part.Substring(13);
            else if (code_part_unedited.StartsWith($"Beep("))
                code_part = code_part.Substring(5);

            if (endOfStatementCode == ");")
                code_part = code_part.Substring(0, code_part.Length - 2);
            else
                code_part = code_part.Substring(0, code_part.Length - 1);
            
            string[] codePart_Array = code_part.Split(',');
            int codePartInt = Convert.ToInt32(codePart_Array[0]);
            int codePartInt2 = Convert.ToInt32(codePart_Array[1]);
            if (codePartInt > 32766 || codePartInt < 37)
            {
                Console.Beep();
            }
            else {
                Console.Beep(codePartInt, codePartInt2 * 1000);
            }
            return "";
        }
    }
}
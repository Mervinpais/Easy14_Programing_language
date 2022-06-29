using System;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    class ConsoleBeep
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));
        
        public void Interperate(string code_part, string[] textArray, string fileloc)
        {
            string endOfStatementCode = ")";
            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons"))
                    endOfStatementCode.Equals(line.EndsWith("true") ? endOfStatementCode = ");" : endOfStatementCode = ")");
                break;
            }

            string code_part_unedited = code_part;
            bool foundUsing = false;
            if (code_part.StartsWith("beep("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the Console.beep function.");
                    Console.ResetColor();
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
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'beep' without its reference  (Use Console.Beep() to fix this error :)");
                    Console.ResetColor();
                }
            }
            else if (code_part.StartsWith($"Console.beep(")) { }

            if (code_part_unedited.StartsWith($"Console.beep("))
                code_part = code_part.Substring(13);
            else if (code_part_unedited.StartsWith($"beep("))
                code_part = code_part.Substring(5);

            code_part = code_part.Substring(0, code_part.Length - 1);
            if (code_part.EndsWith(")"))
            {
                code_part = code_part.Substring(0, code_part.Length - 1);
            }
            
            if (code_part.Contains(","))
            {

                string[] codePart_Array = code_part.Split(',');
                double codePartInt = Convert.ToDouble(codePart_Array[0]);
                double codePartInt2 = Convert.ToDouble(codePart_Array[1]);
                if (codePartInt > 32766 || codePartInt < 37)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning: The frequency of the beep is out of range. Default has been set");
                    Console.ResetColor();
                    Console.Beep();
                }
                else
                {
                    Console.Beep(Convert.ToInt32(codePartInt), Convert.ToInt32(codePartInt2 * 1000));
                }
            }
            else
            {
                Console.Beep();
            }
            return;
        }
    }
}
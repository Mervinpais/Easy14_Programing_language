using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class ConsoleInput
    {
        public static string Interperate(string line = null)
        {
            line = line.Substring("Input".Length);

            if (line.EndsWith(";"))
            {
                line = line.Substring(0, line.Length - 1);    //SYNTAX CHECK 1
            }
            else
            {
                return "";
            }

            if (line.StartsWith("(") && line.EndsWith(")"))
            {
                line = line.Substring(1, line.Length - 2);    //SYNTAX CHECK 2
            }
            else
            {
                return "";
            }


            if (line == "")
            {
                Console.Write(">");
                string returnedInput = Console.ReadLine();
                return returnedInput;
            }
            else if (line.StartsWith("\"") && line.EndsWith("\""))
            {
                line = line.Substring(1, line.Length - 2);
                Console.WriteLine(line);
                Console.Write(">");
                string returnedInput = Console.ReadLine();
                return returnedInput;
            }
            else
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string variable_dir = dir + "\\EASY14_Variables_TEMP";
                if (!Directory.Exists(variable_dir))
                {
                    ErrorReportor.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                    return "";
                }

                var files = Directory.GetFiles(variable_dir);
                if (!(files.Length > 0))
                {
                    ErrorReportor.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                    return "";
                }

                var variable = variable_dir + "\\" + line;
                if (File.Exists(variable))
                {
                    Console.WriteLine(File.ReadAllText(variable));
                    Console.Write(">");
                    string returnedInput = Console.ReadLine();
                    return returnedInput;
                }
                else
                {
                    ErrorReportor.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                    return "";
                }
            }
        }
    }
}
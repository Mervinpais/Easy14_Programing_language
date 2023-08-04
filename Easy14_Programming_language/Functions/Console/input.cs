using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class ConsoleInput
    {
        public static string Interperate(string line = null)
        {
            if (line == "")
            {
                ConsolePrint.Interperate("\"> \"", false);
                string returnedInput = "";
                returnedInput = Console.ReadLine();
                return returnedInput;
            }
            else if (ItemChecks.IsString(line))
            {
                line = line.Substring(1, line.Length - 2);
                ConsolePrint.Interperate(line);
                ConsolePrint.Interperate("\"> \"", false);
                string returnedInput = "";
                returnedInput = Console.ReadLine();
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
                    ConsolePrint.Interperate(File.ReadAllText(variable));
                    ConsolePrint.Interperate("\"> \"", false);
                    string returnedInput = "";
                    returnedInput = Console.ReadLine();
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
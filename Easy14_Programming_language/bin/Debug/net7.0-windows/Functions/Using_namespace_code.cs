using System;
using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class UsingNamspaceFunction
    {
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);

        public static void UsingMethod(string line, bool disableLibraries, int lineCount)
        {
            if (line == "using _easy14_;")
            {
                ErrorReportor.ConsoleLineReporter.Warning("You already are :)");
                return;
            }

            string currentDir = Directory.GetCurrentDirectory();

            string theSupposedNamspace = strWorkPath.Replace("\\bin\\Debug\\net7.0-windows", "") + "\\Functions\\" + line.Replace("using ", "").Replace(";", "");

            /* Checking if the using exists. */
            bool doesUsingExist = Directory.Exists(theSupposedNamspace);
            if (doesUsingExist)
            {
                /* just */
                return;
            }
            else
            {
                ErrorReportor.ConsoleLineReporter.Error($"The Using {line.Replace("using ", "").Replace(";", "")} Mentioned on line {lineCount} is not found!");
                return;
            }
        }

        public static void FromMethod(string line, bool disableLibraries, int lineCount)
        {
            if (line == "from Easy14 get easy14;")
            {
                ErrorReportor.ConsoleLineReporter.Message("You already are using Easy14 :)");
                return;
            }

            // Extract the namespace and class names from the input line
            string[] parts = line.Split(new string[] { "from ", " get " }, StringSplitOptions.RemoveEmptyEntries);
            string theSupposedNamespace = parts[0].Trim();
            string theSupposedClass = parts[1].TrimEnd(';') + ".cs";

            // Check if the directory and file exist
            string fullPath = Path.Combine(strWorkPath, "Functions", theSupposedNamespace, theSupposedClass);
            if (File.Exists(fullPath)) return;
            else
            {
                string errorMessage = File.Exists(Path.GetDirectoryName(fullPath))
                    ? $"The Using class \"{theSupposedClass}\" Mentioned on line {lineCount}, (Line: {line}) is not found!"
                    : $"The Using \"{theSupposedNamespace}\" Mentioned on line {lineCount}, (Line: {line}) is not found!";

                ErrorReportor.ConsoleLineReporter.Error(errorMessage);
            }
        }
    }
}

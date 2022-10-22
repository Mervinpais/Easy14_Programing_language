using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class Using_namespace_code
    {
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);

        public static void usingFunction_interp(string line, bool disableLibraries, int lineCount)
        {
            if (disableLibraries)
            {
                CSharpErrorReporter.ConsoleLineReporter.Warning("You have disabled libraries in options.ini, if you want to use libraries, please change the true to false at line 10 in options.ini");
                return;
            }

            if (line == "using _easy14_;")
            {
                CSharpErrorReporter.ConsoleLineReporter.Warning("You already are :)");
                return;
            }

            string currentDir = Directory.GetCurrentDirectory();

            string theSupposedNamspace = strWorkPath.Replace("\\bin\\Debug\\net6.0-windows", "") + "\\Functions\\" + line.Replace("using ", "").Replace(";", "");

            /* Checking if the using exists. */
            bool doesUsingExist = Directory.Exists(theSupposedNamspace);
            if (doesUsingExist)
            {
                /* just */
                return;
            }
            else
            {
                CSharpErrorReporter.ConsoleLineReporter.Error($"The Using {line.Replace("using ", "").Replace(";", "")} Mentioned on line {lineCount} is not found!");
                return;
            }
        }

        public static void fromFunction_interp(string line, bool disableLibraries, int lineCount)
        {
            if (disableLibraries)
            {
                CSharpErrorReporter.ConsoleLineReporter.Warning("You have disabled libraries in options.ini, if you want to use libraries, please change the true to false at line 10 in options.ini");
                return;
            }
            if (line == "from _easy14_ get _use_;")
            {
                CSharpErrorReporter.ConsoleLineReporter.Warning("You already are :)");
                return;
            }

            string theSupposedNamespace = strWorkPath.Replace("\\bin\\Debug\\net6.0-windows", "") + "\\Functions\\";
            theSupposedNamespace = theSupposedNamespace + line.Substring(line.IndexOf("from"), line.Length - (line.IndexOf("get") - 2)).Substring(4).TrimStart().TrimEnd();
            theSupposedNamespace = theSupposedNamespace.TrimStart().TrimEnd();
            string theSupposedClass = theSupposedNamespace;
            theSupposedClass = theSupposedClass + "\\" + line.Substring(line.IndexOf("get") + 3).TrimStart();
            theSupposedClass = theSupposedClass.Substring(0, theSupposedClass.Length - 1) + ".cs";

            /* Checking if the using exists. */
            bool doesUsingExist = Directory.Exists(theSupposedNamespace);
            if (doesUsingExist)
            {
                bool doesClassExist = File.Exists(theSupposedClass);
                if (doesClassExist)
                {
                    return;
                }
                else
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error($"The Using class \"{theSupposedClass}\" Mentioned on line {lineCount}, (Line: {line}) is not found!");
                    return;
                }
            }
            else
            {
                CSharpErrorReporter.ConsoleLineReporter.Error($"The Using \"{theSupposedNamespace}\" Mentioned on line {lineCount}, (Line: {line}) is not found!");
                return;
            }
        }
    }
}

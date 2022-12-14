using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class ConsolePrint
    {
        public static void Interperate(string line)
        {
            if (line.StartsWith("\"") && line.EndsWith("\""))
            {
                line = line.Substring(1, line.Length - 1);
                Console.WriteLine(line);
            }
            else
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string variable_dir = dir + "\\EASY14_Variables_TEMP";
                if (!Directory.Exists(variable_dir))
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                    return;
                }

                var files = Directory.GetFiles(variable_dir);
                if (!(files.Length > 0))
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                    return;
                }

                var variable = variable_dir + "\\" + line;
                if (File.Exists(variable))
                {
                    Console.WriteLine(File.ReadAllText(variable));
                }
                else
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                }
            }

            //Anything else...
            /*
            Program prog = new Program();
            Console.WriteLine(prog.CompileCode_fromOtherFiles(textArray: new string[] { statement_line }));
            return; */
        }
    }
}
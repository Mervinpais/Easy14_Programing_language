using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class ConsoleExec
    {
        public static void Interperate(string line)
        {
            if (line == null) { return; }
            if (line.StartsWith("\"") && line.StartsWith("\""))
            {
                Program prog = new Program();
                prog.CompileCode_fromOtherFiles(line);
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
                    Program prog = new Program();
                    prog.CompileCode_fromOtherFiles(line);
                }
                else
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                    return;
                }
            }
        }
    }
}
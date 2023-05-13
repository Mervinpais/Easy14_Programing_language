using System;
using System.Data;

namespace Easy14_Programming_Language
{
    public static class ConsolePrint
    {
        public static void Interperate(string line, bool newLine = true)
        {
            if (line.StartsWith("Print("))
            {
                if (!line.EndsWith(";"))
                {
                    ErrorReportor.ConsoleLineReporter.Error(line + " is missing a semicolon.");
                }
                line = line.Substring(6, line.Length - 8);
            }
            else
            {
                line = "\"" + line + "\"";
            }
            if (line.StartsWith("\"") && line.EndsWith("\""))
            {
                line = line.Substring(1, line.Length - 2);

                if (newLine == true) Console.WriteLine(line);
                else Console.Write(line);

                return;
            }
            else if (VariableCode.variableList.ContainsKey(line))
            {
                Console.WriteLine(VariableCode.variableList[line]);
            }
            else if ((!line.StartsWith("\"") || !line.EndsWith("\"")) && !(!line.StartsWith("\"") && !line.EndsWith("\"")))
            {
                if ((line.IndexOf("\"") % 2 != 0))
                {
                    ErrorReportor.ConsoleLineReporter.Error("Mismatch of quotes with line \'" + line + "\', make sure there is no inquality between the double quotes.");
                }
            }
            else
            {
                try
                {
                    Console.WriteLine(new DataTable().Compute(line, ""));
                }
                catch
                {

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
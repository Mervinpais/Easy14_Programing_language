using System;
using System.Data;

namespace Easy14_Programming_Language
{
    public static class ConsolePrint
    {
        public static void Interperate(string line, bool newLine = true)
        {
            if (ItemChecks.IsString(line))
            {
                line = line.Substring(1, line.Length - 2);

                if (newLine == true) Console.WriteLine(line);
                else Console.Write(line);

                return;
            }
            else if (line.EndsWith(';'))
            {
                Program prog = new Program();
                Console.WriteLine(prog.ExternalComplieCode(line));
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
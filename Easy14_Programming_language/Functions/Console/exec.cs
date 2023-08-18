using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class ConsoleExec
    {
        public static void Interperate(string line)
        {
            if (line == null) { return; }
            if (ItemChecks.IsString(line))
            {
                Program prog = new Program();
                prog.ExternalComplieCode(line);
            }
            else
            {
                if (VariableCode.variableList.TryGetValue(line, out _) == true)
                {
                    VariableCode.variableList.TryGetValue(line, out object val);
                    Program prog = new Program();
                    prog.ExternalComplieCode(textArray: new string[] { val.ToString() });
                }
                else
                {
                    ErrorReportor.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                    return;
                }
            }
        }
    }
}
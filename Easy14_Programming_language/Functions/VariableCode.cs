using Easy14_Programming_Language.Application_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.LinkLabel;

namespace Easy14_Programming_Language
{
    public static class VariableCode
    {
        public static Dictionary<string, string> variables = new Dictionary<string, string>();

        // Method to define a new method
        public static void DefineVariable(string variableName, string variableContents)
        {
            if (variableContents.StartsWith("() => "))
            {
                variableContents = variableContents.Substring(6).Trim();
                variableContents = Program.CompileCode(codeToExecute: new string[] { variableContents + ";" }).ToString();
            }
            variables[variableName] = variableContents;
        }

        public static string ReturnString(string variableName)
        {
            if (variables.ContainsKey(variableName))
            {
                return variables[variableName];
            }
            else
            {
                Console.WriteLine($"Method '{variableName}' not found.");
            }
            return null;
        }


        public static bool VariableExists(string methodName)
        {
            return variables.ContainsKey(methodName);
        }
    }
}

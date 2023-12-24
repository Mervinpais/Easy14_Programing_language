using Easy14_Programming_Language.Application_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.LinkLabel;

namespace Easy14_Programming_Language
{
    public static class VariableCode
    {
        public enum VariableType
        {
            @unknown,
            str,
            @int,
            @bool,
            cmd
        }

        public class Variable
        {
            public string Name { get; set; }
            public VariableType Type { get; set; }
            public string Contents { get; set; }
        }

        public static List<Variable> variables = new List<Variable>();

        // Method to define a new method
        public static void DefineVariable(string variableName, string variableContents)
        {
            if (variableContents.StartsWith("() => "))
            {
                variableContents = variableContents.Substring(6).Trim();
                variableContents = Program.CompileCode(codeToExecute: new string[] { variableContents + ";" }).ToString();
            }

            // Add a new Variable to the list
            variables.Add(new Variable { Name = variableName, Type = VariableType.unknown, Contents = variableContents });
        }

        public static string ReturnString(string variableName)
        {
            var variable = variables.FirstOrDefault(v => v.Name == variableName);

            if (variable != null)
            {
                return variable.Contents;
            }
            else
            {
                Console.WriteLine($"Variable '{variableName}' not found.");
                return null;
            }
        }

        public static bool VariableExists(string variableName)
        {
            return variables.Any(v => v.Name == variableName);
        }
    }
}

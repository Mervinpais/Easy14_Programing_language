using Easy14_Programming_Language.Application_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.LinkLabel;

namespace Easy14_Programming_Language
{
    public static class MethodHandler
    {
        // Define a dictionary to store method names and their contents
        private static readonly Dictionary<string, List<string>> methods = new Dictionary<string, List<string>>();

        // Method to define a new method
        public static void DefineMethod(string methodName, List<string> methodContents)
        {
            methods[methodName] = methodContents;
        }

        // Method to execute a method by name
        public static void ExecuteMethod(string methodName)
        {
            if (methods.ContainsKey(methodName))
            {
                Program.CompileCode(methods[methodName].ToArray());
            }
            else
            {
                Console.WriteLine($"Method '{methodName}' not found.");
            }
        }

        // Method to check if a method exists by name
        public static bool MethodExists(string methodName)
        {
            return methods.ContainsKey(methodName);
        }
    }
}

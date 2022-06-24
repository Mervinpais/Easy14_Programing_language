using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Easy14_Programming_Language
{
    class Console_namespace_handler
    {
        public static void handle(string line, string textArray, string fileloc)
        {
            string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Functions");
            List<string> allNamespacesAvaiable_list = new List<string>(allNamespacesAvaiable_array);
            List<string> allNamespacesAvaiable_list_main = new List<string>();
            foreach (string item in allNamespacesAvaiable_list)
            {
                allNamespacesAvaiable_list_main.Add(item.Substring(item.LastIndexOf("\\") + 1));
            }
            allNamespacesAvaiable_array = allNamespacesAvaiable_list_main.ToArray();
            string theNamespaceOfTheLine = line.Split(".")[0];
            
            if (line.StartsWith("Console."))
            {
                string[] split = line.Split('.');
                if (split[1] == "print")
                {
                    string[] code =
                    {
                                "Easy14_Programming_Language.ConsolePrint myfunc = new Easy14_Programming_Language.ConsolePrint();",
                                $"myfunc.interperate({line}, {textArray}, {fileloc});"
                            };
                    CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                    return;
                }
                else if (split[1] == "input()")
                {
                    string[] code =
                    {
                                "Easy14_Programming_Language.ConsoleInput myfunc = new Easy14_Programming_Language.ConsoleInput();",
                                $"myfunc.interperate({line}, {textArray}, {fileloc});"
                            };
                    CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                    return;
                }
            }
        }
    }
}

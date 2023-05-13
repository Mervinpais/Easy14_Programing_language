using System;
using System.Collections.Generic;

namespace Easy14_Programming_Language.Functions.TestingApps
{
    public static class Easy14_Tester
    {
        public static void Load()
        {
            static void ConsoleTest()
            {
                Console.WriteLine("Loading test for Console Library!");
                Dictionary<int, string> cv = new Dictionary<int, string>
                {
                    { 0, "Console.Clear();" },
                    { 1, "Console.Beep();" },
                    { 2, "Console.Print(\"Printing Works!\");" },
                    { 3, "Console.Input(\"Enter anything\");" },
                    { 4, "Console.Exec(\"This should cause an error\");" }
                 }; //The CV variable means Command value


                foreach (KeyValuePair<int, string> kvp in cv)
                {
                    Program.CompileCode(null, new string[] { kvp.Value });
                }
            }
        }
    }
}

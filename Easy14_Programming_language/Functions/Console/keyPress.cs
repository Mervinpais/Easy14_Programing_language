using System;

namespace Easy14_Programming_Language
{
    public static class ConsoleKeyPress
    {
        public static object Interperate(string item = "")
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (item == "Key") return key.Key;
            else if (item == "Char") return key.KeyChar;
            else if (item == "Modifiers") return key.Modifiers;
            else return key;
        }
    }
}
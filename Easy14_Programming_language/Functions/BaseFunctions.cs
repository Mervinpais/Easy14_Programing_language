using System;

namespace Easy14_Programming_Language
{
    public static class BaseFunctions
    {
        public static class Sys
        {
            public static void Write(string text, bool newLine)
            {
                if (newLine)
                {
                    Console.WriteLine(text);
                }
                else
                {
                    Console.Write(text);
                }
            }
        }
    }
}

using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_WriteFile
    {
        public static void Interperate(string file, string content)
        {
            try
            {
                File.WriteAllText(file, content);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
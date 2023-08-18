using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_ReadFile
    {
        public static string Interperate(string file)
        {
            try
            {
                return File.ReadAllText(file);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }
    }
}
using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_MakeFile
    {
        public static void Interperate(string file)
        {
            try
            {
                File.WriteAllText(file, "");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
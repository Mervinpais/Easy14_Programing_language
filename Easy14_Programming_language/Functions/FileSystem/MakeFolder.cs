using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_MakeFolder
    {
        public static void Interperate(string dir)
        {
            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
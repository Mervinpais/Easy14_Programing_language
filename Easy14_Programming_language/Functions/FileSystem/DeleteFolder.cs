using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_DeleteFolder
    {
        public static void Interperate(string dir)
        {
            try
            {
                Directory.Delete(dir, true);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_DeleteFile
    {
        public static void Interperate(string file)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
    }
}
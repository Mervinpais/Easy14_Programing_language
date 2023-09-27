using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_MoveFile
    {
        public static void Interperate(string filePath, string newPath)
        {
            try
            {
                File.Move(filePath, newPath);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
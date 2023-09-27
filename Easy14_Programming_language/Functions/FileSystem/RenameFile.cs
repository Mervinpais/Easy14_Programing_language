using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_RenameFile
    {
        public static void Interperate(string fileName, string newName)
        {
            try
            {
                File.Move(fileName, newName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
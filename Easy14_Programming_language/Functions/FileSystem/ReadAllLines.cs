using System;
using System.IO;

namespace Easy14_Programming_Language
{
    class FileSystem_ReadAllLines
    {
        public string[] Interperate(string file)
        {
            try
            {
                return File.ReadAllLines(file);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }
    }
}
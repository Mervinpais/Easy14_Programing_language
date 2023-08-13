using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_MakeFolder
    {
        public static void Interperate(string code_part, string[] textArray)
        {
            string textToPrint = code_part;
            if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith('"'.ToString()))
            {
                var contentInFile = textToPrint.Replace('"'.ToString(), "");
                try
                {
                    Directory.CreateDirectory(contentInFile);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Create A Folder at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP").Length != 0)
            {
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP");
                /* Checking if the user has used a variable in the MakeFolder function, if they have it
                will get the variable and use it as the folder location. */
                try
                {
                    string file = GetVariable.findVar(textToPrint);
                    var contentInFile = file;
                    Directory.CreateDirectory(contentInFile);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Create A Folder at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
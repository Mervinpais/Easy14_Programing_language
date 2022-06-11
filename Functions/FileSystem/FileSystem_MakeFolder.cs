using System;
using System.IO;

namespace Easy14_Coding_Language
{
    class FileSystem_MakeFolder
    {
        public void interperate(string code_part, string fileloc, string[] textArray, int line_count)
        {
            string code_part_unedited;
            string textToPrint;

            code_part_unedited = code_part;
            code_part = code_part.TrimStart();

            if (code_part.StartsWith($"MakeFolder("))
            {
                bool foundUsing = false;
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                foreach (string x in someLINEs)
                {
                    if (x.StartsWith("using"))
                    {
                        if (x == "using FileSystem;")
                        {
                            foundUsing = true;
                            break;
                        }
                    }
                    if (x == code_part)
                    {
                        break;
                    }
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'FileSystem' wasnt referenced to use 'MakeFolder' without its reference  (Use FileSystem.MakeFile(\"*File Location*\") to fix this error :)");
                }
            }
            else if (code_part.StartsWith($"FileSystem.MakeFolder(") || code_part.StartsWith($"Filesystem.MakeFolder(")) { }
            string line_ = code_part;
            line_ = line_.TrimStart();

            if (code_part.StartsWith($"FileSystem.MakeFolder(") || code_part.StartsWith($"Filesystem.MakeFolder("))
                line_ = line_.Substring(22);
            else if (code_part.StartsWith($"MakeFolder("))
                line_ = line_.Substring(11);

            line_ = line_.Substring(0, line_.Length - 2);
            textToPrint = line_;
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
                try
                {
                    GetVariable getVariable = new GetVariable();
                    string file = getVariable.findVar(textToPrint);
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
using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_DeleteFolder
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static void Interperate(string code_part, string[] textArray)
        {
            string code_part_unedited;
            string textToPrint;

            code_part_unedited = code_part;
            code_part = code_part.TrimStart();

            if (code_part.StartsWith($"DeleteFolder("))
            {
                bool foundUsing = false;
                string[] someLINEs = textArray;
                if (textArray == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the DeleteFile function.");
                    Console.ResetColor();
                    return;
                }

                foreach (string usingStatements in someLINEs)
                {
                    if (usingStatements.StartsWith("using"))
                    {
                        if (usingStatements == "using FileSystem;")
                        {
                            foundUsing = true;
                            break;
                        }
                    }
                    if (usingStatements == code_part)
                    {
                        break;
                    }
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'FileSystem' wasnt referenced to use 'DeleteFolder' without its reference  (Use Console.print(\"*Text To Print*\") to fix this error :)");
                }
            }
            else if (code_part.StartsWith($"FileSystem.DeleteFolder(") || code_part.StartsWith($"Filesystem.DeleteFolder(")) { }
            string line_ = code_part;
            line_ = line_.TrimStart();

            if (code_part.StartsWith($"FileSystem.DeleteFolder("))
                line_ = line_.Substring(24);
            else if (code_part.StartsWith($"DeleteFolder("))
                line_ = line_.Substring(13);

            line_ = line_.Substring(0, line_.Length - 2);
            textToPrint = line_;
            if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith('"'.ToString()))
            {
                var contentInFile = textToPrint.Replace('"'.ToString(), "");
                /* Checking if the folder exists and if it does it will delete it. */
                if (Directory.Exists(contentInFile))
                {
                    Directory.Delete(contentInFile, true);
                }
                else
                {
                    ExceptionSender.SendException("0xF00002");
                }
            }
            /* Checking if there are any files in the folder `EASY14_Variables_TEMP` and if there are
            it will get the variable and delete the folder at the location of the variable. */
            else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP").Length != 0)
            {
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP");
                try
                {
                    string file = GetVariable.findVar(textToPrint);
                    var contentInFile = file;
                    Directory.Delete(contentInFile, true);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Delete A Folder at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
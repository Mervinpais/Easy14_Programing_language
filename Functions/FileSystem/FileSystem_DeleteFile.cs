using System;
using System.IO;

namespace Easy14_Coding_Language
{
    class FileSystem_DeleteFile
    {
        /// <summary>
        /// It takes a string, a string, a string array, and an int, and returns nothing.
        /// </summary>
        /// <param name="code_part">The part of the code that is being interperated</param>
        /// <param name="fileloc">The location of the file</param>
        /// <param name="textArray">The array of strings that contains the code.</param>
        /// <param name="line_count">The line number of the code_part</param>
        public void interperate(string code_part, string fileloc, string[] textArray, int line_count)
        {
            string code_part_unedited;
            string textToPrint;

            code_part_unedited = code_part;
            code_part = code_part.TrimStart();

            /* Checking if the user has referenced the FileSystem library. */
            if (code_part.StartsWith($"DeleteFile("))
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
                    Console.WriteLine($"ERROR; The Using 'FileSystem' wasnt referenced to use 'DeleteFile' without its reference  (Use Console.print(\"*Text To Print*\") to fix this error :)");
                }
            }
            else if (code_part.StartsWith($"FileSystem.DeleteFile(") || code_part.StartsWith($"Filesystem.DeleteFile(")) { }
            string line_ = code_part;
            line_ = line_.TrimStart();

            if (code_part.StartsWith($"FileSystem.DeleteFile("))
                line_ = line_.Substring(22);
            else if (code_part.StartsWith($"DeleteFile("))
                line_ = line_.Substring(11);

            line_ = line_.Substring(0, line_.Length - 2);
            textToPrint = line_;

            if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith('"'.ToString()))
            {
                /* Deleting the file at the location of the string. */
                var contentInFile = textToPrint.Replace('"'.ToString(), "");
                try
                {
                    File.Delete(contentInFile);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Delete A File at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            /* Checking if the user has used a variable in the DeleteFile function. If they have, it
            will check if the variable is a string, and if it is, it will delete the file at the
            location of the string. */
            else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP").Length != 0)
            {
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP");
                try
                {
                    GetVariable getVariable = new GetVariable();
                    string file = getVariable.findVar(textToPrint);
                    var contentInFile = file;
                    File.Delete(contentInFile);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Delete A File at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
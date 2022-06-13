using System;
using System.IO;

namespace Easy14_Coding_Language
{
    class FileSystem_MakeFile
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

            if (code_part.StartsWith($"MakeFile("))
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
                /* Checking if the user has referenced the FileSystem library, if they haven't, it will
                throw an error. */
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'FileSystem' wasnt referenced to use 'MakeFile' without its reference  (Use FileSystem.MakeFile(\"*File Location*\") to fix this error :)");
                }
            }
            else if (code_part.StartsWith($"FileSystem.MakeFile(") || code_part.StartsWith($"Filesystem.MakeFile(")) { }

            if (code_part.StartsWith($"FileSystem.MakeFile(") || code_part.StartsWith($"Filesystem.MakeFile("))
                code_part = code_part.Substring(20);
            else if (code_part.StartsWith($"MakeFile("))
                code_part = code_part.Substring(9);

            code_part = code_part.Substring(0, code_part.Length - 2);
            textToPrint = code_part;
            if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith('"'.ToString()))
            {
                var contentInFile = textToPrint.Replace('"'.ToString(), "");
                try
                {
                    /* Creating a file at the specified location. */
                    using (FileStream fs = File.Create(contentInFile))
                    {
                        fs.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Create A File at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            /* Checking if there is a file in the EASY14_Variables_TEMP folder, if there is, it will
            get the variable from the file and use it as the file location. */
            else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP").Length != 0)
            {
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP");

                /* Checking if there is a file in the EASY14_Variables_TEMP folder, if there is, it
                will get the variable from the file and use it as the file location. */
                try
                {
                    GetVariable getVariable = new GetVariable();
                    string file = getVariable.findVar(textToPrint);
                    var contentInFile = file;
                    using (FileStream fs = File.Create(contentInFile))
                    {
                        fs.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Create A File at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
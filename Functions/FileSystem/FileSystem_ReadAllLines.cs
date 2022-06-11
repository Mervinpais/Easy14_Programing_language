using System;
using System.IO;

namespace Easy14_Coding_Language
{
    class FileSystem_ReadAllLines
    {
        public string[] interperate(string code_part, string fileloc, string[] textArray, int line_count)
        {
            string code_part_unedited = code_part;
            string textToPrint;
            string[] thingToReturn = null;

            code_part = code_part.TrimStart();

            if (code_part.StartsWith($"ReadAllLines("))
            {
                bool foundUsing = false;
                string[] someLINEs = null;

                if (textArray == null && fileloc != null)
                    someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null)
                    someLINEs = textArray;

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
                    if (x == code_part) break;
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'FileSystem' wasnt referenced to use 'ReadAllLines' without its reference  (Use FileSystem.ReadAllLines(\"*File Location*\") to fix this error :)");
                }
            }
            else if (code_part.StartsWith($"FileSystem.ReadAllLines(")) { }
            string line_ = code_part;
            line_ = line_.TrimStart();

            if (code_part.StartsWith($"FileSystem.ReadAllLines("))
                line_ = line_.Substring(24);
            else if (code_part.StartsWith($"ReadAllLines("))
                line_ = line_.Substring(13);

            line_ = line_.Substring(0, line_.Length - 2);
            textToPrint = line_;
            if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith('"'.ToString()))
            {
                var contentInFile = textToPrint.Replace('"'.ToString(), "");
                try
                {
                    Console.WriteLine(File.ReadAllText(contentInFile));
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Read File at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
            {
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                foreach (string file in files)
                {
                    if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == textToPrint.Replace(".txt", ""))
                    {
                        try
                        {
                            string[] contentInFile = File.ReadAllLines(file);
                            thingToReturn = contentInFile;
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR; Can't Find File at the specified location");
                            Console.WriteLine("Extra Info is below;\n\n" + e);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                }
            }
            return thingToReturn;
        }
    }
}
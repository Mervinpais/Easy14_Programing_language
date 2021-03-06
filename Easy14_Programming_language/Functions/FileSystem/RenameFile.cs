using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_RenameFile
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static void Interperate(string code_part, string fileloc, string[] textArray, int line_count)
        {
            string code_part_unedited;
            string textToPrint;

            code_part_unedited = code_part;
            code_part = code_part.TrimStart();

            if (code_part.StartsWith($"RenameFile("))
            {
                bool foundUsing = false;
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the RenameFile function.");
                    Console.ResetColor();
                    return;
                }

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
                    Console.WriteLine($"ERROR; The Using 'FileSystem' wasnt referenced to use 'RenameFile' without its reference  (Use FileSystem.ReadFile(\"*File Location*\") to fix this error :)");
                }
            }
            else if (code_part.StartsWith($"FileSystem.RenameFile(")) { }
            string line_ = code_part;
            line_ = line_.TrimStart();

            if (code_part.StartsWith($"FileSystem.RenameFile("))
                line_ = line_.Substring(22);
            else if (code_part.StartsWith($"RenameFile("))
                line_ = line_.Substring(11);

            line_ = line_.Substring(0, line_.Length - 2);
            textToPrint = line_;
            string[] thePathAndContent = textToPrint.Split(',');
            string oldpath = thePathAndContent[0];
            string newpath = thePathAndContent[1];
            if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith('"'.ToString()))
            {
                try
                {
                    oldpath = oldpath.Replace('"'.ToString(), "");
                    newpath = newpath.Replace('"'.ToString(), "");
                    newpath = newpath.TrimStart();
                    File.Move(oldpath, newpath);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Rename File at the specified location");
                    Console.WriteLine("Extra Info is below;     (Note: Please Enter the FULL FILE PATH when renaming a file for both the old name and new name)\n\n" + e);
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
                            var contentInFile = file;
                            Console.WriteLine(File.ReadAllText(contentInFile));
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR; Can't Rename File at the specified location");
                            Console.WriteLine("Extra Info is below;\n\n" + e);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    }
                }
            }
        }
    }
}
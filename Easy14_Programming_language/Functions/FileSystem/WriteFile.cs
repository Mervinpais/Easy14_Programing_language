using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class FileSystem_WriteFile
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

            if (code_part.StartsWith($"WriteFile("))
            {
                bool foundUsing = false;
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the WriteFile function.");
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
                    Console.WriteLine($"ERROR; The Using 'FileSystem' wasnt referenced to use 'WriteFile' without its reference  (Use FileSystem.ReadFile(\"*FilePath*\") to fix this error :)");
                }
            }
            else if (code_part.StartsWith($"FileSystem.WriteFile(")) { }
            string line_ = code_part;
            line_ = line_.TrimStart();

            if (code_part.StartsWith($"FileSystem.WriteFile("))
                line_ = line_.Substring(21);
            else if (code_part.StartsWith($"WriteFile("))
                line_ = line_.Substring(10);

            line_ = line_.Substring(0, line_.Length - 2);
            textToPrint = line_;
            string[] thePathAndContent = textToPrint.Split(',');
            string path = thePathAndContent[0];
            string contentInFile = thePathAndContent[1];
            if (path.StartsWith('"'.ToString()) && path.EndsWith('"'.ToString()))
            {
                try
                {
                    path = path.Replace('"'.ToString(), "");
                    contentInFile = contentInFile.Replace('"'.ToString(), "");
                    contentInFile = contentInFile.TrimStart();
                    File.WriteAllText(path, contentInFile);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Write To File at the specified location");
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
                            File.WriteAllText(path, contentInFile);
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR; Can't Write To File at the specified location");
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
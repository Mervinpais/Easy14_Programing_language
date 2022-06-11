using System;
using System.IO;

namespace Easy14_Coding_Language
{
    class FileSystem_DeleteFolder
    {
        /*void DeleteFolder(string folderPath) {
            try
            {
                if (Directory.GetFiles(folderPath).Length > 0)
                {
                    foreach (string File_ in Directory.GetFiles(folderPath))
                    {
                        File.Delete(File_);
                    }

                    foreach (string File_ in Directory.GetDirectories(folderPath))
                    {
                        int lengthOfDirInDir = Directory.GetDirectories(File_).Length;
                        if (lengthOfDirInDir < -1)
                        {
                            Directory.Delete(File_);
                        }
                        else if (lengthOfDirInDir > -1)
                        {
                            DeleteFolder(File_);
                        }
                    }
                }
                Directory.Delete(folderPath);
            }
            catch (DirectoryNotFoundException dirNotFoundEx)
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }*/
        public void interperate(string code_part, string fileloc, string[] textArray, int line_count)
        {
            string code_part_unedited;
            string textToPrint;

            code_part_unedited = code_part;
            code_part = code_part.TrimStart();

            if (code_part.StartsWith($"DeleteFolder("))
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
                if (Directory.Exists(contentInFile))
                {
                    Directory.Delete(contentInFile, true);
                }
                else
                {
                    ExceptionSender ExSend = new ExceptionSender();
                    ExSend.SendException(0xF00002);
                }
                /*DeleteFolder(contentInFile);
                try
                {
                    if (Directory.GetFiles(contentInFile).Length > 0)
                    {
                        foreach (string File_ in Directory.GetFiles(contentInFile))
                        {
                            try {
                                if (Directory.GetFiles(File_).Length > -1)
                                {
                                    DeleteFolder(File_);
                                }
                            }
                            catch {
                                File.Delete(File_);
                            }
                        }
                    }
                    Directory.Delete(contentInFile);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nERROR; Can't Delete A Folder at the specified location");
                    Console.WriteLine("Extra Info is below;\n\n" + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }*/
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
                            /*
                            if (Directory.GetFiles(contentInFile).Length > 0)
                            {
                                foreach (string File_ in Directory.GetFiles(contentInFile))
                                {
                                    File.Delete(File_);
                                }
                            }*/
                            Directory.Delete(contentInFile, true);
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nERROR; Can't Delete A Folder at the specified location");
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
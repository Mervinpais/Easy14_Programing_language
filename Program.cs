using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;
using System.Linq;
using System.Management;
using System.Diagnostics;

namespace Easy14_Coding_Language
{
    class Program
    {
        public static char quotes = '"';

        public static bool showCommands = false;
        public static bool previewTheFile = false;

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                foreach (string item in args)
                {
                    Console.Write(item + " ");
                }
            }
            Console.WriteLine("\n====Easy14====\n");
            Thread.Sleep(100);
            if (args.Length != 0)
            {
                if (args[0] == "-run")
                {
                    int item_count = 1;
                    string[] filePath = null;
                    foreach (var item in args)
                    {
                        if (item.EndsWith(".ese14"))
                        {
                            filePath = args[1..item_count];
                        }
                        item_count = item_count + 1;
                    }

                    if (filePath == null)
                    {
                        Console.WriteLine("\n ERROR; CAN NOT FIND FILE SPECIFIED \n");
                    }

                    if (filePath[filePath.Length - 1].EndsWith(".ese14"))
                    {
                        if (args.Length > 2)
                        {
                            if (args[filePath.Length] == "-show_cmds")
                            {
                                showCommands = true;
                            }
                            else if (args[filePath.Length] == "-preview_only")
                            {
                                previewTheFile = true;
                            }
                        }
                        if (!previewTheFile) { compileCode(String.Join(" ", filePath)); }
                        else
                        {
                            try
                            {
                                Console.WriteLine(String.Join(Environment.NewLine, File.ReadAllLines(args[1])));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\n An Error Occured While Reading File To Display in Preview, Below Is the Full Error Exception Message\n");
                                Console.WriteLine(e);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n Uh Oh, this file isnt an actual .ese14 file!, please change the file extention to .ese14 to use thiss file \n");
                    }
                }
                if (args[0] == "-help")
                {
                    Console.WriteLine("Hello! Welcome to the help section of Easy14!");
                    Console.WriteLine(
                        "\n   -help = Show the list of arguments you can run with the Easy14 Framework(ish) app"
                    );
                    Console.WriteLine(
                        "\n   -run = Runs an easy14 file, the file extention must be .ese14 (ex; *easy14 app path* run *file.s14c*)"
                    );
                    Console.WriteLine("    |");
                    Console.WriteLine(
                        "     -show_cmds = shows what command runs while running a file"
                    );
                    Console.WriteLine("\n");
                    Console.WriteLine("     |More Args Commands Comming Soon|");
                }
            }
            else if (args.Length == 0)
            {
                Console.WriteLine("No Arguments to run Given :/");
                Console.WriteLine(
                    "if you need help with what args you can run, run the app with argument '-help' to get args help"
                );
            }
            Thread.Sleep(500);
            Console.WriteLine("\nPress Any Key to exit");
            Console.Read();
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
            {
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                Console.WriteLine("\nDeleting current project variables folder...");
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
            }
            Console.WriteLine("\n Exiting Console...");
            Environment.Exit(-1);
        }
        public void compileCode_fromOtherFiles(string fileloc = null, string[] textArray = null, int lineIDX = 0)
        {
            compileCode(fileloc, textArray, lineIDX);
        }
        public static void compileCode(string fileloc = null, string[] textArray = null, int lineIDX = 0)
        {
            ObjectQuery wql = null;
            ManagementObjectSearcher searcher = null;
            ManagementObjectCollection results = null;
            //Checks Variables /\  and Setting those values in a try/catch statement \/
            try
            {
                wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                searcher = new ManagementObjectSearcher(wql);
                results = searcher.Get();
            }
            catch
            {
                Debug.Write("ERROR: System is not a NT-SYSTEM/Windows-OS, Can't Retrive Memory :(");
            }

            foreach (ManagementObject result in results)
            {
                int freeMemory = Convert.ToInt32(result["FreePhysicalMemory"]) / 1024;
                //Console.WriteLine("Free Memory {0} MB", freeMemory);
                if (freeMemory < 25)
                {
                    ExceptionSender ex_sender = new ExceptionSender();
                    ex_sender.SendException(0x0003);
                }
            }

            int line_count = 1;
            string[] lines = null;
            try {
                if (textArray == null && fileloc != null) {
                    lines = File.ReadAllLines(fileloc);
                }
                else if (textArray != null && fileloc == null) {
                    lines = textArray;
                }
            }
            catch
            {
                ExceptionSender ex_sender = new ExceptionSender();
                ex_sender.SendException(0x0003);
            }

            List<string> lines_list = new List<string>(lines);
            if (lineIDX != 0)
            {
                lines_list.RemoveRange(0, lineIDX);
            }

            List<string> lines_list_mod = new List<string>();
            foreach (string item_ in lines_list)
            {
                lines_list_mod.Add(item_.TrimStart());
            }

            lines = lines_list_mod.ToArray();
            foreach (string line in lines)
            {
                if (showCommands == true)
                {
                    Console.WriteLine(">>>" + line);
                }
                var backslash = "\\";
                if (line.StartsWith($"Console.print(") || line.StartsWith($"print(") && line.EndsWith($");"))
                {
                    Console_print con_print = new Console_print();
                    con_print.interperate(line, textArray, fileloc);
                    
                }
                else if (line.StartsWith($"Console.input(") || line.StartsWith($"input(") && line.EndsWith($");"))
                {
                    Console_input con_input = new Console_input();
                    con_input.interperate(line, textArray, fileloc);
                    
                }
                else if (line.StartsWith($"wait(") && line.EndsWith($");"))
                {
                    Time_wait wait = new Time_wait();
                    wait.interperate(line, line_count);
                }
                else if (line.StartsWith($"var") && line.EndsWith($";"))
                {
                    VARIABLE_var var = new VARIABLE_var();
                    var.interperate(line, lines, line_count);
                }
                else if (line.StartsWith($"if") && line.EndsWith("{"))
                {
                    If_loop if_loop = new If_loop();
                    if_loop.interperate(line, textArray, lines, fileloc);
                }
                else if (line.StartsWith($"while") && line.EndsWith("{"))
                {
                    //since we need the part that we need to loop until x == true, we first get and save the lines of the file/textArray
                    While_loop while_Loop = new While_loop();
                    while_Loop.interperate(line, textArray, lines, fileloc);
                }
                else if (line.StartsWith($"FileSystem.MakeFile(") || line.StartsWith($"MakeFile(") && line.EndsWith($");"))
                {
                    FileSystem_MakeFile fs_mkFile = new FileSystem_MakeFile();
                    fs_mkFile.interperate(line, fileloc, textArray, line_count);
                }
                else if (line.StartsWith($"FileSystem.MakeFolder(") || line.StartsWith($"MakeFolder(") && line.EndsWith($");"))
                {
                    FileSystem_MakeFolder fs_mkFolder = new FileSystem_MakeFolder();
                    fs_mkFolder.interperate(line, fileloc, textArray, line_count);
                }
                else if (line.StartsWith($"FileSystem.DeleteFile(") || line.StartsWith($"DeleteFile(") && line.EndsWith($");"))
                {
                    FileSystem_DeleteFile fs_delFile = new FileSystem_DeleteFile();
                    fs_delFile.interperate(line, fileloc, textArray, line_count);
                }
                else if (line.StartsWith($"FileSystem.DeleteFolder(") || line.StartsWith($"DeleteFolder(") && line.EndsWith($");"))
                {
                    FileSystem_DeleteFolder fs_delFolder = new FileSystem_DeleteFolder();
                    fs_delFolder.interperate(line, fileloc, textArray, line_count);
                }
                else if (line.StartsWith($"FileSystem.ReadFile(") || line.StartsWith($"ReadFile(") && line.EndsWith($");"))
                {
                    FileSystem_ReadFile fs_readFile = new FileSystem_ReadFile();
                    fs_readFile.interperate(line, fileloc, textArray, line_count);
                }
                else if (line.StartsWith($"FileSystem.RenameFile(") || line.StartsWith($"RenameFile(") && line.EndsWith($");"))
                {
                    FileSystem_RenameFile fs_renameFile = new FileSystem_RenameFile();
                    fs_renameFile.interperate(line, fileloc, textArray, line_count);
                }
                else if (line.StartsWith($"FileSystem.WriteFile(") || line.StartsWith($"WriteFile(") && line.EndsWith($");"))
                {
                    FileSystem_WriteFile fs_writeFile = new FileSystem_WriteFile();
                    fs_writeFile.interperate(line, fileloc, textArray, line_count);
                }
                else if (line.StartsWith($"Network.Ping(") || line.StartsWith($"Ping(") && line.EndsWith($");"))
                {
                    Network_Ping net_ping = new Network_Ping();
                    net_ping.interperate(line, fileloc, textArray, line_count);
                }
                else
                {
                    if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line) && line != "}" && line != "break" && line != "return" && !line.StartsWith("using") && !line.StartsWith("//"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ERROR; '{line}' is not a vaild code statement\n  Error was located on Line {line_count}");
                        break;
                    }
                }
                line_count++;
            }
        }
    }
}

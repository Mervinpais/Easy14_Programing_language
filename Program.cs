using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
                    Console.WriteLine("\n   -help = Show the list of arguments you can run with the Easy14 Language");
                    Console.WriteLine("\n   -run = Runs an easy14 file, the file extention must be .ese14 (ex; *easy14 app path* run *file.s14c*)");
                    Console.WriteLine("    |");
                    Console.WriteLine("     -show_cmds = shows what command runs while running a file");
                    Console.WriteLine("\n   -keywords = Shows all keywords that are statements in Easy14");
                    Console.WriteLine("\n");
                    Console.WriteLine("     |More Args Commands Comming Soon|");
                }
                if (args[0].ToLower() == "-keywords")
                {
                    Console.WriteLine("\n===== KEYWORDS =====");
                    Console.WriteLine("\n   === Main ===");
                    Console.WriteLine("\n   print, if, while, func, wait, var");
                    Console.WriteLine("\n   === ==== ===");
                }
            }
            else if (args.Length == 0)
            {
                Console.WriteLine("No Arguments to Given :/");
                Console.WriteLine(
                    "if you need help with what args you can run, run the app with argument '-help' to get args help"
                );
                while (true) {
                    Console.Write("\n>>>");
                    string command = Console.ReadLine();
                    if (command.ToLower() == "exit()" || command.ToLower() == "exit();")
                    {
                        return;
                    }
                    else if (command.ToLower() == "exit")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nPlease use \"exit()\" or \"exit();\" to close the interative console");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }
                    else if (command.Contains("\n"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR; Easy14 Interactive Cannot run multiline code \n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }
                    else {
                        if (!command.StartsWith("-")) {
                            string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\Functions");
                            List<string> allNamespacesAvaiable_list = new List<string>();
                            foreach (string namespace_ in allNamespacesAvaiable_array)
                            {
                                allNamespacesAvaiable_list.Add("using " + namespace_.Substring(namespace_.LastIndexOf("\\") + 1) + ";");
                            }

                            //thanks to the question https://stackoverflow.com/questions/59217/merging-two-arrays-in-net i managed to merge 2 arrays
                            string[] allNamespacesAvaiable = allNamespacesAvaiable_list.ToArray();
                            string[] command_array = {command};
                            //allNamespacesAvaiable = allNamespacesAvaiable.Replace("\r\n", Environment.NewLine);
                            //string[] commandAsArray = {string.Join("\r\n", allNamespacesAvaiable), command};
                            string[] commandAsArray = new string[allNamespacesAvaiable.Length + command_array.Length];
                            Array.Copy(allNamespacesAvaiable, commandAsArray, allNamespacesAvaiable.Length);
                            Array.Copy(command_array, 0, commandAsArray, allNamespacesAvaiable.Length, command_array.Length);

                            Program prog = new Program();
                            prog.compileCode_fromOtherFiles(null, commandAsArray, 0, false, "}");
                        }
                        else {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ERROR; Easy14 Interactive can't understand what the args you specified \n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            continue;
                        }
                    }
                }
            }
            Thread.Sleep(500);
            Console.WriteLine("\nPress Any Key to exit");
            Console.Read();
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
            {
                Console.WriteLine("\nDeleting current project variables folder...");
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                string var_MethodPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP";

                Directory.Delete(var_MethodPath, true);
            }
            Console.WriteLine("\n Exiting Console...");
            Environment.Exit(-1);
        }
        public void compileCode_fromOtherFiles(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {
            compileCode(fileLoc, textArray, lineIDX, isInAMethod, methodName);
        }
        public static void compileCode(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
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
            }
            catch
            {
                Debug.Write("ERROR: System is not a NT-SYSTEM/Windows-OS, Can't Retrive Memory :(");
            }

            int lineCount = 1;
            string[] lines = null;
            try {
                if (textArray == null && fileLoc != null) {
                    lines = File.ReadAllLines(fileLoc);
                }
                else if (textArray != null && fileLoc == null) {
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

                if (line.StartsWith($"using") && line.EndsWith($";"))
                {
                    //assume its a library :) 👍👍👍💯💯🔥🔥
                    //though it my not be and its just "fjejfiofiriojfeojiws" or some random sh!t like that
                    
                    string currentDir = Directory.GetCurrentDirectory();
                    string theSupposedNamspace = Directory.GetCurrentDirectory() + "\\Functions\\" + line.Replace("using ", "").Replace(";", "");
                    bool doesUsingExist = Directory.Exists(theSupposedNamspace);
                    if (doesUsingExist)
                    {
                        /* just */
                        continue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"ERROR; The Using {line.Replace("using ", "").Replace(";", "")} Mentioned on line {lineCount} is not found!");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                else if (line.StartsWith($"Console.print(") || line.StartsWith($"print(") && line.EndsWith($");"))
                {
                    ConsolePrint conPrint = new ConsolePrint();
                    conPrint.interperate(line, textArray, fileLoc);
                    
                }
                else if (line.StartsWith($"Console.input(") || line.StartsWith($"input(") && line.EndsWith($");"))
                {
                    ConsoleInput conInput = new ConsoleInput();
                    conInput.interperate(line, textArray, fileLoc);
                    
                }
                else if (line.StartsWith($"wait(") && line.EndsWith($");"))
                {
                    TimeWait wait = new TimeWait();
                    wait.interperate(line, lineCount);
                }
                else if (line.StartsWith($"var") && line.EndsWith($";"))
                {
                    VariableCode varCode = new VariableCode();
                    varCode.interperate(line, lines, lineCount);
                }
                else if (line.StartsWith($"if") && line.EndsWith("{"))
                {
                    If_Loop if_Loop = new If_Loop();
                    if_Loop.interperate(line, lines, textArray, fileLoc, lineCount, true, methodName);
                    return;
                }
                else if (line.StartsWith($"while") && line.EndsWith("{")) // || (lines[lineCount + 1] == "{")
                {
                    //since we need the part that we need to loop until x == true, we first get and save the lines of the file/textArray
                    WhileLoop whileLoop = new WhileLoop();
                    whileLoop.interperate(line, textArray, lines, fileLoc);
                    return;
                }
                else if (line.StartsWith("func ") && line.EndsWith(") {")) // || line.EndsWith("()" + (lines[lineCount + 1] == "{")
                {
                    MethodCode methodCode = new MethodCode();
                    methodCode.interperate(line, textArray, lines, fileLoc, true);
                    return;
                }
                else if (line.EndsWith("();"))
                {
                    MethodCode methodCode = new MethodCode();
                    methodCode.interperate(line, textArray, lines, fileLoc, false);
                    return;
                }
                else if (line.StartsWith($"FileSystem.MakeFile(") || line.StartsWith($"MakeFile(") && line.EndsWith($");"))
                {
                    FileSystem_MakeFile fs_mkFile = new FileSystem_MakeFile();
                    fs_mkFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.MakeFolder(") || line.StartsWith($"MakeFolder(") && line.EndsWith($");"))
                {
                    FileSystem_MakeFolder fs_mkFolder = new FileSystem_MakeFolder();
                    fs_mkFolder.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.DeleteFile(") || line.StartsWith($"DeleteFile(") && line.EndsWith($");"))
                {
                    FileSystem_DeleteFile fs_delFile = new FileSystem_DeleteFile();
                    fs_delFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.DeleteFolder(") || line.StartsWith($"DeleteFolder(") && line.EndsWith($");"))
                {
                    FileSystem_DeleteFolder fs_delFolder = new FileSystem_DeleteFolder();
                    fs_delFolder.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.ReadFile(") || line.StartsWith($"ReadFile(") && line.EndsWith($");"))
                {
                    FileSystem_ReadFile fs_readFile = new FileSystem_ReadFile();
                    fs_readFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.RenameFile(") || line.StartsWith($"RenameFile(") && line.EndsWith($");"))
                {
                    FileSystem_RenameFile fs_renameFile = new FileSystem_RenameFile();
                    fs_renameFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.WriteFile(") || line.StartsWith($"WriteFile(") && line.EndsWith($");"))
                {
                    FileSystem_WriteFile fs_writeFile = new FileSystem_WriteFile();
                    fs_writeFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"Network.Ping(") || line.StartsWith($"Ping(") && line.EndsWith($");"))
                {
                    NetworkPing netPing = new NetworkPing();
                    netPing.interperate(line, fileLoc, textArray, lineCount);
                }
                else
                {
                    if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line) && line != "}" && line != "break" && line != "return" && !line.StartsWith("using") && !line.StartsWith("//"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ERROR; '{line}' is not a vaild code statement\n  Error was located on Line {lineCount}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    }
                }
                lineCount++;
            }
        }
    }
}

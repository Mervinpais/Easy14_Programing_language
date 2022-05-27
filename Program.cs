using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Threading;
using System.Linq;
using System.Runtime.InteropServices;

namespace Easy14_Coding_Language
{
    class Program
    {
        public static bool showCommands = false;
        public static bool previewTheFile = false;
        static string[] configFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net5.0", "") + "\\Application Code\\options.ini");
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                foreach (string item in args)
                {
                    Console.Write(item + " ");
                }
            }

            Console.WriteLine("\n==== Easy14 Interative Console ====\n");
            //============================================================\\
            //There is a bug where if i entered a random statement and there was the var temp folder, it would think there was a variable and try checking it, then it just continues so we need to delete the variable temp folder, it like initializing variables except we just delete the leftover variables from the last session
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
            }

            //==================The Update Checker====================\\

            updateChecker uc = new updateChecker();
            uc.checkLatestVersion();

            //=========================================================\\
            foreach (string line in configFile)
            {
                if (line.StartsWith("delay")) {
                    int delay = Convert.ToInt32(line.Replace("delay = ", ""));
                    Thread.Sleep(delay * 1000);
                    break;
                }
            }

            if (args.Length != 0)
            {
                if (args[0] == "-run" || args[0] == "/run")
                {
                    int itemCount = 1;
                    string[] filePath = null;
                    foreach (var item in args)
                    {
                        if (item.EndsWith(".e14") || item.EndsWith(".ese14"))
                        { //Still deciding on a file extention :/
                            filePath = args[1..itemCount];
                        }
                        itemCount = itemCount + 1;
                    }
                    Console.WriteLine($"==== {DateTime.Now} | {filePath[filePath.Length - 1]} ====\n");

                    if (filePath == null)
                    {
                        Console.WriteLine("\n ERROR; CAN NOT FIND FILE SPECIFIED \n");
                    }

                    if (filePath[filePath.Length - 1].EndsWith(".ese14") || filePath[filePath.Length - 1].EndsWith(".e14"))
                    {
                        if (args.Length > 2)
                        {
                            if (args[filePath.Length] == "-show_cmds" || args[filePath.Length] == "/show_cmds")
                            {
                                showCommands = true;
                            }
                            else if (args[filePath.Length] == "-preview_only" || args[filePath.Length] == "/preview_only")
                            {
                                previewTheFile = true;
                            }
                        }

                        if (!previewTheFile)
                        {
                            compileCode(String.Join(" ", filePath));
                        }
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
                        Console.WriteLine("\n Uh Oh, this file isnt an actual .e14/.ese14 file!, please change the file extention to .e14 (preferred) or .ese14 to use this file \n");
                    }
                }
                if (args[0] == "-help" || args[0] == "/help")
                {
                    Console.WriteLine("Hello! Welcome to the help section of Easy14!");
                    Console.WriteLine("\n   -help | /help = Show the list of arguments you can run with the Easy14 Language");
                    Console.WriteLine("\n   -run | /run = Runs an easy14 file, the file extention must be .ese14 (ex; *easy14 app path* run *file.s14c*)");
                    Console.WriteLine("    |");
                    Console.WriteLine("     -show_cmds | /show_cmds = shows what command runs while running a file");
                    Console.WriteLine("\n   -keywords | /keywords = Shows all keywords that are statements in Easy14");
                    Console.WriteLine("\n   -intro | /intro = Introduction/Tutorial of Easy14");
                    Console.WriteLine("\n");
                    Console.WriteLine("     |More Commands Comming Soon|");
                }
                if (args[0].ToLower() == "-keywords" || args[0].ToLower() == "/keywords")
                {
                    Console.WriteLine("\n===== KEYWORDS =====");
                    Console.WriteLine("\n   === Main (6) ===");
                    Console.WriteLine("\n   print, if, while, func, wait, var");
                    Console.WriteLine("\n   === ==== ===");
                }
                if (args[0].ToLower() == "-intro" || args[0].ToLower() == "/intro")
                {
                    IntroductionCode introCode = new IntroductionCode();
                    introCode.IntroCode();
                }
                if (args[0].ToLower() == "-appinfo" || args[0].ToLower() == "/appinfo")
                {
                    AppInformation appInfo = new AppInformation();
                    appInfo.ShowInfo();
                }
            }
            else if (args.Length == 0)
            {
                Console.WriteLine("No Arguments to Given :/");
                Console.WriteLine(
                    "if you need help with what args you can run, run the app with argument '-help' to get args help"
                );
                while (true)
                {
                    Console.Write("\n>>>");
                    string command = Console.ReadLine();
                    if (command.ToLower() == "exit()" || command.ToLower() == "exit();")
                    {
                        return;
                    }
                    else if (command.ToLower() == "exit")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nPlease use \"exit()\" or \"exit();\" or Ctrl+C to close the interative console");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }
                    else if (command.ToLower() == "/help" || command.ToLower() == "-help")
                    {
                        Console.WriteLine("Hello! Welcome to the help section of Easy14!");
                        Console.WriteLine("\n   -help | /help = Show the list of arguments you can run with the Easy14 Language");
                        Console.WriteLine("\n   -run | /run = Runs an easy14 file, the file extention must be .ese14 (ex; *easy14 app path* run *file.s14c*)");
                        Console.WriteLine("    |");
                        Console.WriteLine("     -show_cmds | /show_cmds = shows what command runs while running a file");
                        Console.WriteLine("\n   -keywords | /keywords = Shows all keywords that are statements in Easy14");
                        Console.WriteLine("\n   -intro | /intro = Introduction/Tutorial of Easy14");
                        Console.WriteLine("\n");
                        Console.WriteLine("     |More Commands Comming Soon|");
                    }
                    else if (command.ToLower() == "/keywords" || command.ToLower() == "-keywords")
                    {
                        Console.WriteLine("\n===== KEYWORDS =====");
                        Console.WriteLine("\n   === Main (6) ===");
                        Console.WriteLine("\n   print, if, while, func, wait, var");
                        Console.WriteLine("\n   ================");
                        Console.WriteLine("\n====================");
                    }
                    else if (command.ToLower() == "/intro" || command.ToLower() == "-intro")
                    {
                        IntroductionCode introCode = new IntroductionCode();
                        introCode.IntroCode();
                    }
                    else if (command.ToLower() == "/appinfo" || command.ToLower() == "-appinfo")
                    {
                        AppInformation appInfo = new AppInformation();
                        appInfo.ShowInfo();
                    }
                    else if (command.Contains("\n")) //This is impossible since pasting a multiline will just paste one lines, then will automatically hit enter and continue on
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR; Easy14 Interactive Cannot run multiline code \n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        continue;
                    }
                    else
                    {
                        if (!command.StartsWith("-") && !command.StartsWith("/"))
                        {
                            string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net5.0", "") + "\\Functions");
                            List<string> allNamespacesAvaiable_list = new List<string>();
                            foreach (string namespace_ in allNamespacesAvaiable_array)
                            {
                                allNamespacesAvaiable_list.Add("using " + namespace_.Substring(namespace_.LastIndexOf("\\") + 1) + ";");
                            }

                            //thanks to the question https://stackoverflow.com/questions/59217/merging-two-arrays-in-net i managed to merge 2 arrays
                            string[] allNamespacesAvaiable = allNamespacesAvaiable_list.ToArray();
                            string[] command_array = { command };
                            //allNamespacesAvaiable = allNamespacesAvaiable.Replace("\r\n", Environment.NewLine);
                            //string[] commandAsArray = {string.Join("\r\n", allNamespacesAvaiable), command};
                            string[] commandAsArray = new string[allNamespacesAvaiable.Length + command_array.Length];
                            Array.Copy(allNamespacesAvaiable, commandAsArray, allNamespacesAvaiable.Length);
                            Array.Copy(command_array, 0, commandAsArray, allNamespacesAvaiable.Length, command_array.Length);

                            Program prog = new Program();
                            prog.compileCode_fromOtherFiles(null, commandAsArray, 0, false, "}");
                        }
                        else
                        {
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

            /*
                SETUP
            */
            /* Options.ini vars code */
            string endOfStatementCode = ");";
            bool disableLibraries = false;
            int windowHeight = Console.WindowHeight;
            int windowWidth = Console.WindowWidth;
            string windowState = "normal";

            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons"))
                    if (line.EndsWith("true")) endOfStatementCode = ");";
                    else endOfStatementCode = ")";
                if (line.StartsWith("disableLibraries"))
                    if (line.EndsWith("true")) disableLibraries = true;
                    else disableLibraries = false;
                if (line.StartsWith("windowHeight"))
                    if (!line.EndsWith("false"))
                        windowHeight = Convert.ToInt32(line.Replace("windowHeight = ", ""));
                if (line.StartsWith("windowWidth"))
                    if (!line.EndsWith("false"))
                        windowWidth = Convert.ToInt32(line.Replace("windowWidth = ", ""));
                if (line.StartsWith("windowState"))
                    if (line.EndsWith("max")) windowState = "maximized";
                    else if (line.EndsWith("min")) windowState = "minimized";
                    else if (line.EndsWith("hide")) windowState = "hidden";
                    else if (line.EndsWith("normal")) windowState = "normal";
                    else windowState = "normal";
            }

            //========== Only thing using System.Runtime.InteropServices =========//

            [DllImport("kernel32.dll", ExactSpelling = true)]

            static extern IntPtr GetConsoleWindow();
            IntPtr ThisConsole = GetConsoleWindow();
            
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
            const int HIDE = 0;
            const int MAXIMIZE = 3;
            const int MINIMIZE = 6;
            const int RESTORE = 9;

            if (windowState == "maximized")
                ShowWindow(ThisConsole, MAXIMIZE);
            if (windowState == "minimized")
                ShowWindow(ThisConsole, MINIMIZE);
            if (windowState == "hidden")
                ShowWindow(ThisConsole, HIDE);
            // ============================================================ //
            if (Console.WindowHeight != windowHeight)
            {
                try {
                    Console.SetWindowSize(Console.WindowWidth, windowHeight > 50 ? windowHeight = 50 : windowHeight);
                }
                catch (Exception e) {
                    ThrowErrorMessage tErM = new ThrowErrorMessage();
                    tErM.sendErrMessage("Uh oh, the value you wanted to specify for the Console Window Hight won't work! (check if the value is a string/decimal and change it to an integer)", null, "error");
                    tErM.sendErrMessage("Couldn't Change Window Height using the value in options.ini, using Default window height", null, "warning");
                    tErM.sendErrMessage("Here is Exception Error;\n" + e.Message, null, "error");
                }
            }

            if (Console.WindowWidth != windowWidth) 
            {
                try {
                    Console.SetWindowSize(windowWidth > 150 ? windowWidth = 150 : windowWidth, Console.WindowHeight);
                }
                catch {
                    ThrowErrorMessage tErM = new ThrowErrorMessage();
                    tErM.sendErrMessage("Uh oh, the value you wanted to specify for the Console Window Width won't work! (check if the value is a string/decimal and change it to an integer)", null, "error");
                    tErM.sendErrMessage("Couldn't Change Window Width using the value in options.ini, using Default window width", null, "warning");
                }
            }
            /*
            */

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
            try
            {
                if (textArray == null && fileLoc != null)
                {
                    lines = File.ReadAllLines(fileLoc);
                }
                else if (textArray != null && fileLoc == null)
                {
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
                    if (disableLibraries) {
                        ThrowErrorMessage tErM = new ThrowErrorMessage();
                        tErM.sendErrMessage("You have disabled libraries in options.ini, if you want to use libraries, please change the true to false at line 10 in options.ini", null, "error");
                        break;
                    }
                    if (line == "using this;")
                    {
                        Console.WriteLine("The(Mervin's) Way of C# Programming by Mervinpais");
                        Console.WriteLine("=========================================\n");
                        Console.WriteLine("- No need for big functions for basic stuff (need to use a for loop?, just use a while loop if you can)");
                        Console.WriteLine("- Implicit code is better than Explicit code");
                        Console.WriteLine("- If its hard to make, it will probably be hard to explain");
                        Console.WriteLine("- Similarly, If its easy to make, it will probably be easier to explain");
                        //Console.WriteLine("- Its your opinion to use an if statement like this\n\n  if (condition) {\n\n  }\n\n   or\n\n  if (condition)\n  {\n\n  }\n\n it doesnt matter, all that matters is that it looks readable");
                        Console.WriteLine("=========================================\n");
                        Console.WriteLine("Last Edited - 24/5/2022 9:25 PM");
                        Console.WriteLine("=======");  
                        /*Console.WriteLine("- No use of using old libraries that haven't been supported for a few years as they may be broken (Espicially NuGet Packages, dont use the older/unsupported version of NuGet packages that dont support the latest version of your version of C#");
                        Console.WriteLine("- Make your code simple for others");
                        Console.WriteLine("- Dont use ");
                        Console.WriteLine("* Other Stuff *");
                        Console.WriteLine("- Did you really think making fake commits that are only 1 or 2 changes will get a job? you do know they will research what the changes were in your repo.");
                        Console.WriteLine("- Don't try for hard concepts, they will make your brain go brrr (in a bad way)");
                        Console.WriteLine("- Don't Copy someone's code that is not under any license allows the code to be open sourced (example; Windows OS's code since microsoft has it closed source");*/
                        break;
                    }
                    //assume its a library :) 👍👍👍💯💯🔥🔥
                    //though it my not be and its just "fjejfiofiriojfeojiws" or some random sh!t like that
                    string currentDir = Directory.GetCurrentDirectory();
                    string theSupposedNamspace = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net5.0", "") + "\\Functions\\" + line.Replace("using ", "").Replace(";", "");
                    bool doesUsingExist = Directory.Exists(theSupposedNamspace);
                    //Console.WriteLine(theSupposedNamspace);
                    //Console.WriteLine(doesUsingExist);
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
                        break;
                    }
                }
                else if (line.ToLower() == "exit()" || line.ToLower() == "exit();")
                {
                    return;
                }
                else if (line.ToLower() == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPlease use \"exit()\" or \"exit();\" or Ctrl+C to close the interative console");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    continue;
                }
                else if (line.StartsWith($"Console.print(") || line.StartsWith($"print(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    ConsolePrint conPrint = new ConsolePrint();
                    conPrint.endOfStatementCode = endOfStatementCode;
                    conPrint.interperate(line, textArray, fileLoc);

                }
                else if (line.StartsWith($"Console.input(") || line.StartsWith($"input(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    ConsoleInput conInput = new ConsoleInput();
                    conInput.endOfStatementCode = endOfStatementCode;
                    conInput.interperate(line, textArray, fileLoc, null);

                }
                else if (line.StartsWith($"Console.clear(") || line.StartsWith($"clear(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    ConsoleClear conClear = new ConsoleClear();
                    conClear.endOfStatementCode = endOfStatementCode;
                    conClear.interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"Console.exec(") || line.StartsWith($"exec(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    ConsoleExec conExec = new ConsoleExec();
                    conExec.endOfStatementCode = endOfStatementCode;
                    conExec.interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"wait(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    TimeWait wait = new TimeWait();
                    wait.interperate(line, lineCount);
                }
                else if (line.StartsWith($"var") && line.EndsWith(endOfStatementCode == ")" ? "" : ";"))
                {
                    VariableCode varCode = new VariableCode();
                    varCode.endOfStatementCode = endOfStatementCode;
                    varCode.interperate(line, lines, lineCount);
                }
                else if (line == "true")
                {
                    Console.WriteLine("true");
                }
                else if (line == "false")
                {
                    Console.WriteLine("false");
                }
                else if (line.StartsWith($"if") && line.EndsWith("{"))
                {
                    If_Loop if_Loop = new If_Loop();
                    if_Loop.interperate(line, lines, textArray, fileLoc, lineCount, isInAMethod, methodName);
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
                else if (line.StartsWith($"FileSystem.MakeFile(") || line.StartsWith($"MakeFile(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    FileSystem_MakeFile fs_mkFile = new FileSystem_MakeFile();
                    fs_mkFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.MakeFolder(") || line.StartsWith($"MakeFolder(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    FileSystem_MakeFolder fs_mkFolder = new FileSystem_MakeFolder();
                    fs_mkFolder.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.DeleteFile(") || line.StartsWith($"DeleteFile(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    FileSystem_DeleteFile fs_delFile = new FileSystem_DeleteFile();
                    fs_delFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.DeleteFolder(") || line.StartsWith($"DeleteFolder(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    FileSystem_DeleteFolder fs_delFolder = new FileSystem_DeleteFolder();
                    fs_delFolder.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.ReadFile(") || line.StartsWith($"ReadFile(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    FileSystem_ReadFile fs_readFile = new FileSystem_ReadFile();                    
                    fs_readFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.RenameFile(") || line.StartsWith($"RenameFile(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    FileSystem_RenameFile fs_renameFile = new FileSystem_RenameFile();
                    fs_renameFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.WriteFile(") || line.StartsWith($"WriteFile(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    FileSystem_WriteFile fs_writeFile = new FileSystem_WriteFile();
                    fs_writeFile.interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"Network.Ping(") || line.StartsWith($"Ping(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    NetworkPing netPing = new NetworkPing();
                    netPing.interperate(line, fileLoc, textArray, lineCount);
                }
                else
                {
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
                    {
                        bool gotFiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length > -1;
                        if (gotFiles)
                        {
                            foreach (string file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
                            {
                                string supposedVar = file.Substring(file.LastIndexOf("\\")).Replace(".txt", "").Substring(1);
                                if (line.StartsWith(supposedVar))
                                {
                                    if (line.Contains("=") && line.Count(f => (f == '=')) == 1)
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1).Replace(".txt", "") + " = ";
                                        string content = line.Replace(partToReplace, "");
                                        content = content.Substring(1).Substring(0, content.Length - 2);
                                        File.WriteAllText(filePath, content);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line) && line != "}" && line != "break" && line != "return" && !line.StartsWith("using") && !line.StartsWith("//"))
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

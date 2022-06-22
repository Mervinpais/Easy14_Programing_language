using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Easy14_Programming_Language
{
    public class Program
    {
        public static bool showCommands = false;
        public static bool previewTheFile = false;

        //code from https://iq.direct/blog/51-how-to-get-the-current-executable-s-path-in-csharp.html :)
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);

        //static string[] configFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Application Code\\options.ini");
        static string[] configFile = File.ReadAllLines(Path.Combine(strWorkPath + "..\\..\\..\\..\\Application Code", "options.ini"));

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                foreach (string item in args)
                {
                    Console.Write(item + " ");
                }
            }

            if (args.Length != 0)
                if (args[0] == "-run" || args[0] == "/run")
                    Console.WriteLine("\n==== Easy14 Interative Console ====\n");
                else
                    Console.WriteLine("\n==== Easy14 Console ====\n");
            else
                Console.WriteLine("\n==== Easy14 Console ====\n");
            //============================================================\\

            /* Deleting the temporary folder that was created in the previous step. */
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP", true);
            }

            //==================The Update Checker====================\\

            /* Checking if the user has disabled updates in the config file. If they have not, it will
            check for updates. */

            bool UpdatesDisabled = false;
            bool UpdatesWarningsDisabled = false;
            foreach (string item in configFile)
            {
                if (item.StartsWith("UpdatesDisabled"))
                {
                    if (item.EndsWith("true"))
                    {
                        UpdatesDisabled = true;
                    }
                }
                if (item.StartsWith("UpdatesWarningsDisabled"))
                {
                    if (item.EndsWith("true"))
                    {
                        UpdatesWarningsDisabled = true;
                    }
                }
            }

            if (!UpdatesDisabled)
            {
                updateChecker uc = new updateChecker();
                uc.checkLatestVersion(UpdatesWarningsDisabled);
            }

            //=========================================================\\

            /* Reading the config file and getting the delay value. */
            foreach (string line in configFile)
            {
                if (line.StartsWith("delay"))
                {
                    int delay = Convert.ToInt32(line.Replace("delay = ", ""));
                    Thread.Sleep(delay * 1000);
                    break;
                }
            }

            /* The below code is the code that runs when the app is ran, it checks for arguments
            and runs the code accordingly */
            if (args.Length != 0)
            {
                if (args[0] == "-run" || args[0] == "/run")
                {
                    int itemCount = 1;
                    string[] filePath = null;
                    foreach (var item in args)
                    {
                        if (item.EndsWith(".e14") || item.EndsWith(".ese14"))
                        {
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
                    Console.WriteLine("\n   -help || /help = Show the list of arguments you can run with the Easy14 Language");
                    Console.WriteLine("\n   -run || /run = Runs an easy14 file, the file extention must be .ese14 (ex; *easy14 app path* run *file.s14c*)");
                    Console.WriteLine("    |");
                    Console.WriteLine("     -show_cmds || /show_cmds = shows what command runs while running a file");
                    Console.WriteLine("\n   -keywords || /keywords = Shows all keywords that are statements in Easy14");
                    Console.WriteLine("\n   -intro || /intro = Introduction/Tutorial of Easy14");
                    Console.WriteLine("\n");
                    Console.WriteLine("     |More Commands Comming Soon|");
                }
                if (args[0].ToLower() == "-keywords" || args[0].ToLower() == "/keywords")
                {
                    Console.WriteLine("\n===== KEYWORDS =====");
                    Console.WriteLine("\n   === Main (6) ===");
                    Console.WriteLine("\n   print, if, while, func, wait, var");
                    Console.WriteLine("\n   ================");
                    Console.WriteLine("\n====================");
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
                        /* Deleting the temporary folder that was created in the previous step. */
                        Console.WriteLine("\n Clearing Variables... \n");
                        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
                        {
                            Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP", true);
                        }
                        Console.WriteLine("\n Resetting Terminal Colors... \n");
                        Console.ResetColor();
                        Thread.Sleep(75);
                        Console.WriteLine("\n Exiting Easy14 Interactive... \n");
                        return;
                    }
                    else if (command.ToLower() == "exit")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nPlease use \"exit()\" or \"exit();\" or Ctrl+C to close the interative console");
                        Console.ResetColor();
                        continue;
                    }
                    if (command.StartsWith("send_exception("))
                    {
                        foreach (string line in configFile)
                        {
                            if (line.StartsWith("turnOnDeveloperOptions"))
                            {
                                if (line.EndsWith("true"))
                                {
                                    string exceptionToSend_str = command.Replace("send_exception(", "");
                                    exceptionToSend_str = exceptionToSend_str.Substring(0, exceptionToSend_str.Length - 1);
                                    int exceptionToSend_int = Convert.ToInt32(exceptionToSend_str, 16);
                                    ExceptionSender es = new ExceptionSender();
                                    es.SendException(exceptionToSend_int);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("\nDev mode has not been turned on!\n");
                                }
                            }
                        }
                    }
                    else if (command.ToLower().StartsWith("/run") || command.ToLower().StartsWith("-run"))
                    {
                        Program prog = new Program();
                        prog.compileCode_fromOtherFiles(command.Replace("/run", "").Replace("-run", "").TrimStart());
                    }
                    else if (command.ToLower() == "/help" || command.ToLower() == "-help")
                    {
                        Console.WriteLine("Hello! Welcome to the help section of Easy14!");
                        Console.WriteLine("\n   -help || /help = Show the list of arguments you can run with the Easy14 Language");
                        Console.WriteLine("\n   -run || /run = Runs an easy14 file, the file extention must be .ese14 (ex; *easy14 app path* run *file.s14c*)");
                        Console.WriteLine("    |");
                        Console.WriteLine("     -show_cmds || /show_cmds = shows what command runs while running a file");
                        Console.WriteLine("\n   -keywords || /keywords = Shows all keywords that are statements in Easy14");
                        Console.WriteLine("\n   -intro || /intro = Introduction/Tutorial of Easy14");
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
                        Console.ResetColor();
                        continue;
                    }
                    else
                    {
                        if (!command.StartsWith("-") && !command.StartsWith("/"))
                        {
                            string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Functions");
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
                            Console.ResetColor();
                            continue;
                        }
                    }
                }
            }

            //Thread.Sleep(500);
            Console.WriteLine("\n===== Easy14 Interactive Console =====\n");
            foreach (string line in configFile)
            {
                if (line.StartsWith("turnOnDeveloperOptions"))
                {
                    if (line.EndsWith("true"))
                    {
                        Console.WriteLine("\nDeveloper Options Enabled\n");
                        Console.WriteLine("\n========================\n\n");
                        Console.WriteLine("\nIf you want to turn off developer options, type 'turnOffDeveloperOptions', and press enter.\n");
                        Console.WriteLine($"Current Buffer Height; {Console.BufferHeight}");
                        Console.WriteLine($"Current Buffer Width; {Console.BufferWidth}");
                        Console.WriteLine($"Capslock Status; {Console.CapsLock}");
                        Console.WriteLine($"Foreground Color; {Console.ForegroundColor}");
                        Console.WriteLine($"Background Color; {Console.BackgroundColor}");
                        Console.WriteLine($"Current Window title; {Console.Title}");
                        Console.WriteLine("\n========================\n\n");
                    }
                    break;
                }
            }
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
                    Console.ResetColor();
                    continue;
                }
                else if (command.ToLower().StartsWith("/run") || command.ToLower().StartsWith("-run"))
                {
                    Program prog = new Program();
                    prog.compileCode_fromOtherFiles(command.Replace("/run", "").Replace("-run", "").TrimStart());
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

                /// <summary>
                /// It prints out the keywords of the language.
                /// </summary>
                /// <param name="keywords">Prints out the keywords of the language.</param>
                else if (command.ToLower() == "/keywords" || command.ToLower() == "-keywords")
                {
                    Console.WriteLine("\n===== KEYWORDS =====");
                    Console.WriteLine("\n   === Main (6) ===");
                    Console.WriteLine("\n   print, if, while, func, wait, var");
                    Console.WriteLine("\n   ================");
                    Console.WriteLine("\n====================");
                }
                /// <summary>
                /// It's a function that checks if the user has typed in the command "/intro" or
                /// "-intro" and if they have, it will run the IntroductionCode class
                /// </summary>
                /// <param name="intro">This is the command that the user will type in to run the
                /// code.</param>
                else if (command.ToLower() == "/intro" || command.ToLower() == "-intro")
                {
                    IntroductionCode introCode = new IntroductionCode();
                    introCode.IntroCode();
                }
                /// <summary>
                /// It's a function that shows the information about the application.
                /// </summary>
                /// <param name="appinfo">Shows the application information</param>
                else if (command.ToLower() == "/appinfo" || command.ToLower() == "-appinfo")
                {
                    AppInformation appInfo = new AppInformation();
                    appInfo.ShowInfo();
                }

                /// <summary>
                /// If the command is a single line, then it will run it. If it's a multiline, then it
                /// will not run it
                /// </summary>
                else if (command.Contains("\n"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR; Easy14 Interactive Cannot run multiline code \n");
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    if (!command.StartsWith("-") && !command.StartsWith("/"))
                    {
                        /* Getting all the namespaces in the Functions folder and adding them to a
                        list. */
                        string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Functions");
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
                        Console.ResetColor();
                        continue;
                    }
                }
            }
            /*Console.WriteLine("\nPress Any Key to exit");
            Console.Read();
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
            {
                Console.WriteLine("\nDeleting current project variables folder...");
                string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP");
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                string var_MethodPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";

                Directory.Delete(var_MethodPath, true);
            }
            Console.WriteLine("\n Exiting Console...");
            Environment.Exit(-1);*/
        }

        /// <summary>
        /// This function is used to compile code from other files
        /// </summary>
        /// <param name="fileLoc">The location of the file that is being compiled.</param>
        /// <param name="textArray">The array of strings that contains the code.</param>
        /// <param name="lineIDX">The line number of the code that is being compiled.</param>
        /// <param name="isInAMethod">If the code is in a method, this will be true.</param>
        /// <param name="methodName">The name of the method that is being called.</param>
        public void compileCode_fromOtherFiles(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {
            compileCode(fileLoc, textArray, lineIDX, isInAMethod, methodName);
        }

        public static void compileCode(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {

            /*
                SETUP
            */

            /* Reading the Options.ini file and setting the variables based on the Options.ini file. */

            /* Options.ini vars code */
            string endOfStatementCode = ");";
            bool disableLibraries = false;
            int windowHeight = Console.WindowHeight;
            int windowWidth = Console.WindowWidth;
            string windowState = "normal";
            string windowTitle = "Easy14 Interactive";

            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons"))
                {
                    if (line.EndsWith("true"))
                    {
                        endOfStatementCode = ");";
                    }
                    else
                    {
                        endOfStatementCode = ")";
                    }
                }
                if (line.StartsWith("disableLibraries"))
                {
                    if (line.EndsWith("true"))
                    {
                        disableLibraries = true;
                    }
                    else
                    {
                        disableLibraries = false;
                    }
                }
                if (line.StartsWith("windowHeight"))
                {
                    if (!line.EndsWith("false"))
                    {
                        windowHeight = Convert.ToInt32(line.Replace("windowHeight = ", ""));
                    }
                }
                if (line.StartsWith("windowWidth"))
                {
                    if (!line.EndsWith("false"))
                    {
                        windowWidth = Convert.ToInt32(line.Replace("windowWidth = ", ""));
                    }
                }
                if (line.StartsWith("windowState"))
                {
                    if (line.EndsWith("max"))
                    {
                        windowState = "maximized";
                    }
                    else if (line.EndsWith("min"))
                    {
                        windowState = "minimized";
                    }
                    else if (line.EndsWith("hide"))
                    {
                        windowState = "hidden";
                    }
                    else if (line.EndsWith("normal"))
                    {
                        windowState = "normal";
                    }
                    else
                    {
                        windowState = "normal";
                    }
                }
                if (line.StartsWith("showOptionsINI_DataWhenE14_Loads"))
                {
                    if (line.EndsWith("true"))
                    {
                        List<string> configFileLIST = new List<string>();

                        foreach (string line_ in configFile)
                        {
                            if (line_.StartsWith(";") || line_ == "" || line_ == " ") continue;
                            configFileLIST.Add(line_);
                        }

                        string[] configFile_modified = configFileLIST.ToArray();

                        Console.WriteLine(string.Join(Environment.NewLine, configFile_modified));
                        Console.WriteLine("\n========================\n\n");
                    }
                }

            }

            //========== Only thing using System.Runtime.InteropServices =========//

            /// <summary>
            /// It's a function that takes a string as an argument and then changes the state of the
            /// console window to either maximized, minimized, hidden, or restored
            /// </summary>
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
            {
                ShowWindow(ThisConsole, MAXIMIZE);
            }
            else if (windowState == "minimized")
            {
                ShowWindow(ThisConsole, MINIMIZE);
            }
            else if (windowState == "hidden")
            {
                ShowWindow(ThisConsole, HIDE);
            }
            else if (windowState == "restore")
            {
                ShowWindow(ThisConsole, RESTORE);
            }
            // ============================================================ //

            /* Checking if the console window height is not equal to the windowHeight variable. If it
            is not equal, it will try to set the console window height to the windowHeight variable.
            If it can't, it will throw an error message. */
            if (Console.WindowHeight != windowHeight)
            {
                try
                {
                    Console.SetWindowSize(Console.WindowWidth, windowHeight > 50 ? windowHeight = 50 : windowHeight);
                }
                catch (Exception e)
                {
                    ThrowErrorMessage tErM = new ThrowErrorMessage();
                    tErM.sendErrMessage("Uh oh, the value you wanted to specify for the Console Window Hight won't work! (check if the value is a string/decimal and change it to an integer)", null, "error");
                    tErM.sendErrMessage("Couldn't Change Window Height using the value in options.ini, using Default window height", null, "warning");
                    tErM.sendErrMessage("Here is Exception Error;\n" + e.Message, null, "error");
                }
            }

            /* Checking if the console window width is not equal to the window width specified in the
            options.ini file. If it is not, it will try to set the console window width to the value
            specified in the options.ini file. If it can't, it will throw an error message. */
            if (Console.WindowWidth != windowWidth)
            {
                try
                {
                    Console.SetWindowSize(windowWidth > 150 ? windowWidth = 150 : windowWidth, Console.WindowHeight);
                }
                catch
                {
                    ThrowErrorMessage tErM = new ThrowErrorMessage();
                    tErM.sendErrMessage("Uh oh, the value you wanted to specify for the Console Window Width won't work! (check if the value is a string/decimal and change it to an integer)", null, "error");
                    tErM.sendErrMessage("Couldn't Change Window Width using the value in options.ini, using Default window width", null, "warning");
                }
            }

            /*

            /* Checking the free memory of the system and if it is less than 25 MB it will send an
            exception to the user.
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
                    if (freeMemory < 10)
                    {
                        ExceptionSender ex_sender = new ExceptionSender();
                        //ex_sender.SendException(0x0003); Wrong/Unavaliable Exception Code
                        ex_sender.SendException(0x0000A1);
                    }
                }
            }
            catch
            {
                Debug.Write("ERROR: System is not a NT-SYSTEM/Windows-OS, Can't Retrive Memory :(");
            }

            /* Reading the file and storing it in a string array. */
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

            /* Removing the first lineIDX lines from the list. */
            List<string> lines_list = new List<string>(lines);
            if (lineIDX != 0)
            {
                lines_list.RemoveRange(0, lineIDX);
            }

            /* Removing the leading whitespace from each line in the list. */
            List<string> lines_list_mod = new List<string>();
            foreach (string item_ in lines_list)
            {
                lines_list_mod.Add(item_.TrimStart());
            }

            lines = lines_list_mod.ToArray();

            //Now below is where the magic happens!

            foreach (string line in lines)
            {

                char[] line_chrArr = line.ToCharArray();

                if (showCommands == true) Console.WriteLine(">>>" + line);

                /* Checking if the line starts with "using" and ends with ";" and if it does, it checks
                if the user has disabled libraries in the options.ini file. If the user has disabled
                libraries, it will throw an error message. If the user has not disabled libraries,
                it will check if the line is "using this;". If it is, it will print out a message.
                If it is not, it will check if the using exists. If it does, it will continue. If it
                does not, it will throw an error message. */

                if (line.StartsWith($"using") && line.EndsWith($";"))
                {
                    if (disableLibraries)
                    {
                        ThrowErrorMessage tErM = new ThrowErrorMessage();
                        tErM.sendErrMessage("You have disabled libraries in options.ini, if you want to use libraries, please change the true to false at line 10 in options.ini", null, "error");
                        break;
                    }
                    if (line == "using this;")
                    {
                        Console.WriteLine("Did you expect \"The Zen of {programmingLanguageName}\" ?");
                        Console.WriteLine("=========================================\n");
                        Console.WriteLine("\nIm sorry but i don't care about making my own \"Zen of Easy14\"");
                        break;
                    }

                    string currentDir = Directory.GetCurrentDirectory();
                    string theSupposedNamspace = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Functions\\" + line.Replace("using ", "").Replace(";", "");

                    /* Checking if the using exists. */
                    bool doesUsingExist = Directory.Exists(theSupposedNamspace);
                    if (doesUsingExist)
                    {
                        /* just */
                        continue;
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.WriteLine($"ERROR; The Using {line.Replace("using ", "").Replace(";", "")} Mentioned on line {lineCount} is not found!");
                        Console.ResetColor();
                        break;
                    }
                }
                else if (string.Join("", line_chrArr) != "" && char.IsDigit(line_chrArr[0]))
                {
                    string statement = string.Join("", line_chrArr);
                    if (statement.Contains("+"))
                    {
                        var var1 = statement.Split('+')[0];
                        var var2 = statement.Split('+')[1];
                        double? double1 = null;
                        double? double2 = null;
                        try
                        {
                            double1 = Convert.ToDouble(var1.TrimStart().TrimEnd());
                        }
                        catch
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to add {var1} which can't be done!",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) Integer Convertion Error!; {var1} is not a valid double"
                                };
                            //tErM.sendErrMessage(null, errText, 0x0000B1);
                            exceptionSend.SendException(0x0000B1, errText); //String in int/decimal function exception
                            break;
                        }
                        try
                        {
                            double2 = Convert.ToDouble(var2.TrimStart().TrimEnd());
                        }
                        catch
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to add {var2} which can't be done!",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) Integer Convertion Error!; {var2} is not a valid double"
                                };
                            //tErM.sendErrMessage(null, errText, 0x0000B1);
                            exceptionSend.SendException(0x0000B1, errText); //String in int/decimal function exception
                            break;
                        }
                        double answer = 0;
                        try
                        {
                            answer = Convert.ToDouble(double1 + double2);
                            Console.WriteLine(answer);
                        }
                        catch (Exception e)
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to add {double1} and {double2} which can't be added due to an error with C#",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) {e.Message}"
                                };
                            exceptionSend.SendException(0x0000B7, errText);
                        }
                    }
                    if (statement.Contains("-"))
                    {
                        var var1 = statement.Split('-')[0];
                        var var2 = statement.Split('-')[1];
                        double? double1 = null;
                        double? double2 = null;
                        try
                        {
                            double1 = Convert.ToDouble(var1.TrimStart().TrimEnd());
                        }
                        catch
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to subtract {var1} which can't be done!",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) Integer Convertion Error!; {var1} is not a valid double"
                                };
                            //tErM.sendErrMessage(null, errText, 0x0000B1);
                            exceptionSend.SendException(0x0000B1, errText); //String in int/decimal function exception
                            break;
                        }
                        try
                        {
                            double2 = Convert.ToDouble(var2.TrimStart().TrimEnd());
                        }
                        catch
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to subtract {var2} which can't be done!",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) Integer Convertion Error!; {var2} is not a valid double"
                                };
                            //tErM.sendErrMessage(null, errText, 0x0000B1);
                            exceptionSend.SendException(0x0000B1, errText); //String in int/decimal function exception
                            break;
                        }
                        double answer = 0;
                        try
                        {
                            answer = Convert.ToDouble(double1 - double2);
                            Console.WriteLine(answer);
                        }
                        catch (Exception e)
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to subtract {double1} and {double2} which can't be added due to an error with C#",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) {e.Message}"
                                };
                            exceptionSend.SendException(0x0000B8, errText);
                        }
                    }
                    if (statement.Contains("*"))
                    {
                        var var1 = statement.Split('*')[0];
                        var var2 = statement.Split('*')[1];
                        double? double1 = null;
                        double? double2 = null;
                        try
                        {
                            double1 = Convert.ToDouble(var1.TrimStart().TrimEnd());
                        }
                        catch
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to multiply {var1} which can't be done!",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) Integer Convertion Error!; {var1} is not a valid double"
                                };
                            //tErM.sendErrMessage(null, errText, 0x0000B1);
                            exceptionSend.SendException(0x0000B1, errText); //String in int/decimal function exception
                            break;
                        }
                        try
                        {
                            double2 = Convert.ToDouble(var2.TrimStart().TrimEnd());
                        }
                        catch
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to multiply {var2} which can't be done!",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) Integer Convertion Error!; {var2} is not a valid double"
                                };
                            //tErM.sendErrMessage(null, errText, 0x0000B1);
                            exceptionSend.SendException(0x0000B1, errText); //String in int/decimal function exception
                            break;
                        }
                        double answer = 0;
                        try
                        {
                            answer = Convert.ToDouble(double1 * double2);
                            Console.WriteLine(answer);
                        }
                        catch (Exception e)
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to multiply {double1} and {double2} which can't be added due to an error with C#",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) {e.Message}"
                                };
                            exceptionSend.SendException(0x0000B9, errText);
                        }
                    }
                    if (statement.Contains("/"))
                    {
                        var var1 = statement.Split('/')[0];
                        var var2 = statement.Split('/')[1];
                        double? double1 = null;
                        double? double2 = null;
                        try
                        {
                            double1 = Convert.ToDouble(var1.TrimStart().TrimEnd());
                        }
                        catch
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to divide {var1} which can't be done!",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) Integer Convertion Error!; {var1} is not a valid double"
                                };
                            //tErM.sendErrMessage(null, errText, 0x0000B1);
                            exceptionSend.SendException(0x0000B1, errText); //String in int/decimal function exception
                            break;
                        }
                        try
                        {
                            double2 = Convert.ToDouble(var2.TrimStart().TrimEnd());
                        }
                        catch
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to divide {var2} which can't be done!",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) Integer Convertion Error!; {var2} is not a valid double"
                                };
                            //tErM.sendErrMessage(null, errText, 0x0000B1);
                            exceptionSend.SendException(0x0000B1, errText); //String in int/decimal function exception
                            break;
                        }
                        double answer = 0;
                        try
                        {
                            answer = Convert.ToDouble(double1 / double2);
                            Console.WriteLine(answer);
                        }
                        catch (DivideByZeroException divBy0)
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to divide {double1} and {double2} which can't be added due to {(double1 == 0 ? double1 : double2)} being 0 and you can't Divide by Zero!",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) You tried to Divide by 0, here is the rest of the Error Message\n{divBy0.Message}"
                                };
                            exceptionSend.SendException(0x0000B11, errText);
                        }
                        catch (Exception e)
                        {
                            ExceptionSender exceptionSend = new ExceptionSender();
                            string[] errText = {
                                    $"Error; You tried to divide {double1} and {double2} which can't be added due to an error with C#",
                                    $"   at Line {lineCount}",
                                    $"   at Line {line}",
                                    $"(C# Error) {e.Message}"
                                };
                            exceptionSend.SendException(0x0000B10, errText);
                        }
                    }
                }

                /* Checking if the user has entered "exit()" or "exit();" and if they have, it will
                exit the program. */
                else if (line.ToLower() == "exit()" || line.ToLower() == "exit();")
                {
                    return;
                }

                /// <summary>
                /// If the user types "exit" then the console will print a message telling the user to
                /// use "exit()" or "exit();" or Ctrl+C to close the console.
                /// </summary>
                /// <param name="exit">The command to exit the console</param>
                else if (line.ToLower() == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPlease use \"exit()\" or \"exit();\" or Ctrl+C to close the interative console");
                    Console.ResetColor();
                    continue;
                }
                else if (line.StartsWith($"Console.print(") || line.StartsWith($"print(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    ConsolePrint conPrint = new ConsolePrint();
                    conPrint.interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"Console.input(") || line.StartsWith($"input(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    ConsoleInput conInput = new ConsoleInput();
                    conInput.interperate(line, textArray, fileLoc, null);
                }
                else if (line.StartsWith($"Console.clear(") || line.StartsWith($"clear(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    ConsoleClear conClear = new ConsoleClear();
                    conClear.interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"Console.exec(") || line.StartsWith($"exec(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    ConsoleExec conExec = new ConsoleExec();
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
                    if_Loop.interperate(line, lines, textArray, fileLoc, isInAMethod, methodName);
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
                else if (line.StartsWith($"Time.CurrentTime(") || line.StartsWith($"CurrentTime(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    Time_CurrentTime currentTime = new Time_CurrentTime();
                    string time = currentTime.interperate(line, textArray, fileLoc);
                    Console.WriteLine(time);
                }
                else if (line.StartsWith($"Time.IsLeapYear(") || line.StartsWith($"IsLeapYear(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    Time_IsLeapYear isLeapYear = new Time_IsLeapYear();
                    string isLeapYear_str = isLeapYear.interperate(line, textArray, fileLoc);
                    Console.WriteLine(Convert.ToBoolean(isLeapYear_str));
                }
                else if (line.StartsWith($"Random.RandomRange(") || line.StartsWith($"RandomRange(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    Random_RandomRange randomRange = new Random_RandomRange();
                    string randomRange_str = randomRange.interperate(line, textArray, fileLoc);
                    Console.WriteLine(randomRange_str);
                }
                else if (line.StartsWith($"sdl2.makeWindow(") || line.StartsWith($"makeWindow(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    SDL2_makeWindow makeWindow = new SDL2_makeWindow();
                    string code_line = line.Replace("sdl2.", "").Replace("makeWindow(", "");
                    code_line = code_line.Substring(0, code_line.Length - endOfStatementCode.Length);
                    string[] values = code_line.Split(",");
                    int sizeX = 200;
                    int sizeY = 200;
                    int posX = SDL.SDL_WINDOWPOS_UNDEFINED;
                    int posY = SDL.SDL_WINDOWPOS_UNDEFINED;
                    string title = "myWindowTitle";

                    try { sizeX = Convert.ToInt32(values[0]); } catch { }
                    try { sizeY = Convert.ToInt32(values[1]); } catch { }
                    try { posX = Convert.ToInt32(values[2]); } catch { }
                    try { posY = Convert.ToInt32(values[3]); } catch { }
                    try { title = values[4]; } catch { }
                    IntPtr window = (IntPtr)0;
                    long window_int = -1;
                    new Task(() => { window_int = makeWindow.interperate(sizeX, sizeY, posX, posY, title); }).Start();
                    //window_int = makeWindow.interperate(sizeX, sizeY, posX, posY, title);
                    window = (IntPtr)window_int;
                    Thread.Sleep(100);
                    continue;
                }
                else if (line.StartsWith($"sdl2.createShape(") || line.StartsWith($"createShape(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    SDL2_createShape createShape = new SDL2_createShape();
                    string code_line = line.Replace("sdl2.", "").Replace("createShape(", "");
                    code_line = code_line.Substring(0, code_line.Length - endOfStatementCode.Length);
                    string[] values = code_line.Split(",");
                    long window = 0;
                    int x = 0;
                    int y = 0;
                    int w = 0;
                    int h = 0;

                    try { window = Convert.ToInt64(values[0]); } catch { }
                    try { x = Convert.ToInt32(values[1]); } catch { }
                    try { y = Convert.ToInt32(values[2]); } catch { }
                    try { w = Convert.ToInt32(values[3]); } catch { }
                    try { h = Convert.ToInt32(values[4]); } catch { }

                    new Task(() => { createShape.interperate(window, x, y, w, h); }).Start();
                }
                else if (line.StartsWith($"sdl2.clearScreen(") || line.StartsWith($"clearScreen(") && line.EndsWith($"{endOfStatementCode}"))
                {
                    SDL2_clearScreen clearScreen = new SDL2_clearScreen();
                    string code_line = line.Replace("sdl2.", "").Replace("clearScreen(", "");
                    code_line = code_line.Substring(0, code_line.Length - endOfStatementCode.Length);
                    long window = 0;
                    string color = null;
                    string[] values = code_line.Split(",");
                    window = Convert.ToInt64(values[0]);
                    color = values[1];
                    clearScreen.interperate(window, color);
                }
                else
                {
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
                    {
                        bool gotFiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP").Length > -1;
                        if (gotFiles)
                        {
                            if (line.EndsWith("();")) //Means its probably a function
                            {
                                MethodCode methodCode = new MethodCode();
                                methodCode.interperate(line, textArray, lines, fileLoc, false);
                                return;
                            }
                            foreach (string file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
                            {
                                string supposedVar = file.Substring(file.LastIndexOf("\\")).Replace(".txt", "").Substring(1);
                                if (line.StartsWith(supposedVar))
                                {
                                    /* Checking if the line contains an equal sign, and if it does, it
                                    will check if the line contains a plus sign, and if it does, it
                                    will check if the line contains more than one equal sign. If it
                                    does, it will check if the plus sign is not before the equal
                                    sign. If it is not, it will check if the line contains more than
                                    one equal sign. If it does, it will check if the plus sign is
                                    before the equal sign. If it is, it will check if the line
                                    contains more than one equal sign. If it does, it */
                                    if (line.Contains("=") && (line.IndexOf("+") + 1) != line.IndexOf("=") && line.Count(f => (f == '=')) == 1)
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1).Replace(".txt", "") + " = ";
                                        string content = line.Replace(partToReplace, "");
                                        //content = content.Substring(1).Substring(0, content.Length - 2);
                                        if (content.Contains("+") && content.Count(f => (f == '+')) == 1)
                                        {
                                            Math_Add math_Add = new Math_Add();
                                            int result = math_Add.interperate(content, 0, supposedVar);
                                        }
                                        else
                                        {
                                            File.WriteAllText(filePath, content);
                                        }
                                        break;
                                    }
                                    /* Adding a line to a var. */
                                    if (line.Contains("+=") && line.Count(f => (f == '=')) == 1 && line.Count(f => (f == '+')) == 1 && line.IndexOf("=") == (line.IndexOf("+") + 1))
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1).Replace(".txt", "") + " = ";
                                        string[] FileContents = File.ReadAllLines(filePath);
                                        string content = line.Replace(partToReplace, "");
                                        content = content.Substring(1).Substring(0, content.Length - 2);
                                        content = content.Substring(5);
                                        content = content.Substring(0, content.Length - 1);
                                        List<string> FileContents_list = new List<string>(FileContents);
                                        FileContents_list.Add(content);
                                        File.WriteAllText(filePath, string.Join(Environment.NewLine, FileContents_list.ToArray()));
                                        break;
                                    }
                                    /* Removing a line from a var. */
                                    if (line.Contains("-=") && line.Count(f => (f == '-')) == 1 && line.Count(f => (f == '-')) == 1 && line.IndexOf("-") == (line.IndexOf("-") + 1))
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1).Replace(".txt", "") + " = ";
                                        string[] FileContents = File.ReadAllLines(filePath);
                                        string content = line.Replace(partToReplace, "");
                                        content = content.Substring(1).Substring(0, content.Length - 2);
                                        content = content.Substring(5);
                                        content = content.Substring(0, content.Length - 1);
                                        List<string> FileContents_list = new List<string>(FileContents);
                                        try
                                        {
                                            FileContents_list.Remove(content);
                                        }
                                        catch
                                        {

                                        }
                                        File.WriteAllText(filePath, string.Join(Environment.NewLine, FileContents_list.ToArray()));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    /* The below code is checking to see if the line is not empty, not whitespace, not
                    a closing bracket, not a break, not a return, not a using statement, and not a
                    comment. If the line is not any of those things, then it will print out an error
                    message. */
                    else if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line) && line != "}" && line != "break" && line != "return" && !line.StartsWith("using") && !line.StartsWith("//"))
                    {
                        string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Functions");
                        List<string> allNamespaces_list_fullPaths = new List<string>(allNamespacesAvaiable_array);
                        List<string> allNamespaces_list = new List<string>();

                        foreach (string item in allNamespaces_list_fullPaths)
                        {
                            allNamespaces_list.Add(item.Substring(item.LastIndexOf("\\") + 1));
                        }

                        allNamespacesAvaiable_array = allNamespaces_list.ToArray();
                        string theNamespaceOfTheLine = line.Split(".")[0];
                        
                        if (allNamespacesAvaiable_array.Contains(theNamespaceOfTheLine))
                        {
                            int index = Array.IndexOf(allNamespacesAvaiable_array, theNamespaceOfTheLine);
                            string theClassOfTheLine = line.Split(".")[0];
                            string theFunctionOfTheLine = line.Split(".")[1];
                            string params_str = line.Replace($"{theNamespaceOfTheLine}.{theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("))}(", "");
                            params_str = params_str.Substring(0, params_str.Length - 2);
                            string[] params_ = { };

                            try { params_ = params_str.Split(","); }
                            catch { /* Now we know this is a method without parameters */}
                            string theFunctionOfTheLine_params = theFunctionOfTheLine;
                            theFunctionOfTheLine = theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("));

                            //Older
                            /*Type type_ = Type.GetType(theFunctionOfTheLine);
                            MethodInfo method = type_.GetMethod("run");
                            method.Invoke(null, null);*/

                            //Old
                            //Activator.CreateInstance(Convert.ToString(Assembly.GetExecutingAssembly()), Convert.ToString(Type.GetType(theFunctionOfTheLine)));                            
                            
                            string[] code =
                            {
                                $"Easy14_Programming_Language.{theFunctionOfTheLine} myfunc = new Easy14_Programming_Language.{theFunctionOfTheLine}();",
                                $"myfunc.interperate({string.Join(",", params_)});"
                            };
                            myFunc myfunc = new myFunc();
                            try
                            {
                                CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                            }
                            catch (Exception e)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nFUNCTION EXCEPTION ERROR\n---------------------------------------");
                                switch (e.HResult)
                                {
                                    case -2146233088:
                                        Console.WriteLine("You gave an incorrect parameter! Check Below");
                                        Console.WriteLine("---------------------------------------");
                                        Console.WriteLine("The parameters you gave were:");
                                        foreach (var item in params_)
                                        {
                                            Console.WriteLine(item);
                                        }
                                        Console.WriteLine("---------------------------------------");
                                        Console.WriteLine(e.Message);
                                        break;
                                }
                                Console.ResetColor();
                            }
                            return;
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ERROR; '{line}' is not a vaild code statement\n  Error was located on Line {lineCount}");
                        Console.ResetColor();
                        break;
                    }
                }
                lineCount++;
            }
        }
    }
}

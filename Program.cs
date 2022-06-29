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
    /// <summary>
    /// This class is the main class of the whole app
    /// </summary>
    public class Program
    {
        /// <summary>
        /// showCommands is a variable that shows all the commands that are executed by the user (only when running files, not with the intractive Interperater)
        /// </summary>
        public static bool showCommands = false;
        /// <summary>
        /// previewTheFile is a variable that instead of running the code, lets you preview what the file looks like
        /// </summary>
        public static bool previewTheFile = false;

        //code from https://iq.direct/blog/51-how-to-get-the-current-executable-s-path-in-csharp.html :)
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);

        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        readonly static string tempVariableFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";

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
            {
                if (args[0] == "-run" || args[0] == "/run")
                {
                    Console.WriteLine("\n==== Easy14 Interative Console ====\n");
                }
                else
                {
                    Console.WriteLine("\n==== Easy14 Console ====\n");
                }
            }
            else
            {
                Console.WriteLine("\n==== Easy14 Console ====\n");
            }

            //============================================================\\

            /* Deleting the temporary folder that was created in the previous step. */

            if (Directory.Exists(tempVariableFolder))
            {
                Directory.Delete(tempVariableFolder);
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
                        itemCount++;
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
                            CompileCode(String.Join(" ", filePath));
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
                else if (File.Exists(args[0]) || File.Exists(args[0]))
                {
                    int itemCount = 1;
                    string[] filePath = null;
                    foreach (var item in args)
                    {
                        if (item.EndsWith(".e14") || item.EndsWith(".ese14"))
                        {
                            filePath = args[1..itemCount];
                        }
                        itemCount++;
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
                            CompileCode(String.Join(" ", filePath));
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
                        string exceptionToSend_str = command.Replace("send_exception(", "");
                        exceptionToSend_str = exceptionToSend_str[..^2];
                        ExceptionSender es = new ExceptionSender();
                        es.SendException(exceptionToSend_str);
                        break;
                    }
                    else if (command.ToLower().StartsWith("/run") || command.ToLower().StartsWith("-run"))
                    {
                        Program prog = new Program();
                        prog.CompileCode_fromOtherFiles(command.Replace("/run", "").Replace("-run", "").TrimStart());
                    }
                    else if (File.Exists(command) || File.Exists(command))
                    {
                        string filePath = command;

                        Console.WriteLine($"==== {DateTime.Now} | {command} ====\n");

                        if (filePath == null)
                        {
                            Console.WriteLine("\n ERROR; CAN NOT FIND FILE SPECIFIED \n");
                        }

                        if (command.EndsWith(".ese14") || command.EndsWith(".e14"))
                        {
                            try
                            {
                                Program prog = new Program();
                                prog.CompileCode_fromOtherFiles(command);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("\n An Error Occured While Reading File, Below Is the Full Error Exception Message\n");
                                Console.WriteLine(e);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n Uh Oh, this file isnt an actual .e14/.ese14 file!, please change the file extention to .e14 (preferred) or .ese14 to use this file \n");
                        }
                    }
                    else if (command.ToLower() == "/help" || command.ToLower() == "-help")
                    {
                        HelpCommandCode helpCommandCode = new HelpCommandCode();
                        helpCommandCode.DisplayDefaultHelpOptions();
                    }
                    else if (command.ToLower().StartsWith("/help") || command.ToLower().StartsWith("-help"))
                    {
                        HelpCommandCode helpCommandCode = new HelpCommandCode();
                        string functionToGetHelpWith = command;
                        functionToGetHelpWith = functionToGetHelpWith.Substring(5).TrimStart().TrimEnd();
                        helpCommandCode.GetHelp(functionToGetHelpWith);
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
                    else if (command.Contains('\n')) //This is impossible since pasting a multiline will just paste one lines, then will automatically hit enter and continue on
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
                            string[] allNamespacesAvaiable_array = Directory.GetDirectories(strWorkPath + "\\..\\..\\..\\Functions");

                            List<string> allNamespacesAvaiable_list = new List<string>();
                            foreach (string namespace_ in allNamespacesAvaiable_array)
                            {
                                allNamespacesAvaiable_list.Add(string.Concat("using ", namespace_.AsSpan(namespace_.LastIndexOf("\\") + 1), ";"));
                            }

                            //thanks to the question https://stackoverflow.com/questions/59217/merging-two-arrays-in-net i managed to merge 2 arrays
                            string[] allNamespacesAvaiable = allNamespacesAvaiable_list.ToArray();
                            string[] command_array = { command };
                            string[] commandAsArray = new string[allNamespacesAvaiable.Length + command_array.Length];
                            Array.Copy(allNamespacesAvaiable, commandAsArray, allNamespacesAvaiable.Length);
                            Array.Copy(command_array, 0, commandAsArray, allNamespacesAvaiable.Length, command_array.Length);

                            Program prog = new Program();
                            prog.CompileCode_fromOtherFiles(null, commandAsArray, 0, false, "}");
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
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            try { Console.WriteLine($"Capslock Status; {Console.CapsLock}"); } catch { }
                        Console.WriteLine($"Foreground Color; {Console.ForegroundColor}");
                        Console.WriteLine($"Background Color; {Console.BackgroundColor}");
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
                    prog.CompileCode_fromOtherFiles(command.Replace("/run", "").Replace("-run", "").TrimStart());
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
                else if (command.Contains('\n'))
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
                            allNamespacesAvaiable_list.Add(string.Concat("using ", namespace_.AsSpan(namespace_.LastIndexOf("\\") + 1), ";"));
                        }

                        //thanks to the question https://stackoverflow.com/questions/59217/merging-two-arrays-in-net i managed to merge 2 arrays
                        string[] allNamespacesAvaiable = allNamespacesAvaiable_list.ToArray();
                        string[] command_array = { command };
                        string[] commandAsArray = new string[allNamespacesAvaiable.Length + command_array.Length];
                        Array.Copy(allNamespacesAvaiable, commandAsArray, allNamespacesAvaiable.Length);
                        Array.Copy(command_array, 0, commandAsArray, allNamespacesAvaiable.Length, command_array.Length);

                        Program prog = new Program();
                        prog.CompileCode_fromOtherFiles(null, commandAsArray, 0, false, "}");
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

        /// <summary>
        /// This function is used to compile code from other files
        /// </summary>
        /// <param name="fileLoc">The location of file than has the code</param>
        /// <param name="textArray">The array of strings that contains the code.</param>
        /// <param name="lineIDX">The line number of the code to run on</param>
        /// <param name="isInAMethod">If the code is in a method, this will be true.</param>
        /// <param name="methodName">The name of the method that is being called.</param>
        public void CompileCode_fromOtherFiles(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {
            CompileCode(fileLoc, textArray, lineIDX, isInAMethod, methodName);
        }

        /// <summary>
        /// This function is used to compile code (only code within the file, unless you use the CompileCode_fromOtherFiles() function)
        /// </summary>
        /// <param name="fileLoc">The location of file than has the code</param>
        /// <param name="textArray">The array of strings that contains the code.</param>
        /// <param name="lineIDX">The line number of the code to run on</param>
        /// <param name="isInAMethod">If the code is in a method, this will be true.</param>
        /// <param name="methodName">The name of the method that is being called.</param>
        public static void CompileCode(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {
            /*
                SETUP
            */

            /* Reading the Options.ini file and setting the variables based on the Options.ini file. */

            /* Options.ini vars code */
            bool disableLibraries = false;
            int windowHeight = Console.WindowHeight;
            int windowWidth = Console.WindowWidth;
            string windowState = "normal";

            foreach (string line in configFile)
            {
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
                else if (line.StartsWith("windowHeight"))
                {
                    if (!line.EndsWith("false"))
                    {
                        windowHeight = Convert.ToInt32(line.Replace("windowHeight = ", ""));
                    }
                }
                else if (line.StartsWith("windowWidth"))
                {
                    if (!line.EndsWith("false"))
                    {
                        windowWidth = Convert.ToInt32(line.Replace("windowWidth = ", ""));
                    }
                }
                else if (line.StartsWith("windowState"))
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
                else if (line.StartsWith("showOptionsINI_DataWhenE14_Loads"))
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
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
                    ThrowErrorMessage tErM = new ThrowErrorMessage();
                    try
                    {
                        Console.SetWindowSize(windowWidth > 150 ? windowWidth = 150 : windowWidth, Console.WindowHeight);
                    }
                    catch
                    {
                        tErM.sendErrMessage("Uh oh, the value you wanted to specify for the Console Window Width won't work! (check if the value is a string/decimal and change it to an integer)", null, "error");
                        tErM.sendErrMessage("Couldn't Change Window Width using the value in options.ini, using Default window width", null, "warning");
                    }
                }
                else
                {
                    ThrowErrorMessage tErM = new ThrowErrorMessage();
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        tErM.sendErrMessage("Changing terminal size on linux with C# isn't Possible", null, "error");
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        tErM.sendErrMessage("Changing terminal size on OSX/MAC OS with C# isn't Possible", null, "error");
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                    {
                        tErM.sendErrMessage("Changing terminal size on FreeBSD with C# isn't Possible", null, "error");
                    }
                }
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
                ex_sender.SendException("0xF000C1");
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

                    string theSupposedNamspace = strWorkPath.Replace("\\bin\\Debug\\net6.0", "") + "\\Functions\\" + line.Replace("using ", "").Replace(";", "");

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
                else if (string.Join("", line_chrArr) != "" && char.IsDigit(line_chrArr[0])) //Example if you typed in 4 + 4 in the interactive interperator it would return 8 and same for -, *, / etc.
                {
                    string statement = string.Join("", line_chrArr);
                    if (statement.Contains('+'))
                    {
                        Math_Add math_Add = new Math_Add();
                        var result = math_Add.Interperate(statement, -1);
                        Console.WriteLine(result);
                    }
                    if (statement.Contains('-'))
                    {
                        Math_Subtract math_Subtract = new Math_Subtract();
                        var result = math_Subtract.Interperate(statement, -1);
                        Console.WriteLine(result);
                    }
                    if (statement.Contains('*'))
                    {
                        Math_Multiply math_Multiply = new Math_Multiply();
                        var result = math_Multiply.Interperate(statement, -1);
                        Console.WriteLine(result);
                    }
                    if (statement.Contains('/'))
                    {
                        Math_Divide math_Divide = new Math_Divide();
                        var result = math_Divide.Interperate(statement, -1);
                        Console.WriteLine(result);
                    }
                }

                /* Checking if the user has entered "exit()" or "exit();" and if they have, it will
                exit the program. */
                else if (line.ToLower() == "exit()" || line.ToLower() == "exit();")
                {
                    return;
                }
                else if (line.ToLower() == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPlease use \"exit()\" or \"exit();\" or Ctrl+C to close the interative console");
                    Console.ResetColor();
                    continue;
                }

                else if (line.StartsWith($"Console.print(") || line.StartsWith($"print(") && line.EndsWith(";"))
                {
                    ConsolePrint conPrint = new ConsolePrint();
                    ConsolePrint.Interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"Console.input(") || line.StartsWith($"input(") && line.EndsWith(";"))
                {
                    ConsoleInput conInput = new ConsoleInput();
                    conInput.Interperate(line, textArray, fileLoc, null);
                }
                else if (line.StartsWith($"Console.clear(") || line.StartsWith($"clear(") && line.EndsWith("}"))
                {
                    ConsoleClear conClear = new ConsoleClear();
                    conClear.Interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"Console.exec(") || line.StartsWith($"exec(") && line.EndsWith("}"))
                {
                    ConsoleExec conExec = new ConsoleExec();
                    conExec.Interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"Console.beep(") || line.StartsWith($"beep(") && line.EndsWith("}"))
                {
                    AudioPlay conBeep = new AudioPlay();
                    conBeep.Interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"wait(") && line.EndsWith("}"))
                {
                    TimeWait wait = new TimeWait();
                    wait.Interperate(line, lineCount);
                }
                else if (line.StartsWith($"var") && line.EndsWith(";"))
                {
                    VariableCode varCode = new VariableCode();
                    varCode.Interperate(line, lines, lineCount);
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
                    if_Loop.Interperate(line, lines, textArray, fileLoc, isInAMethod, methodName);
                    return;
                }
                else if (line.StartsWith($"while") && line.EndsWith("{")) // || (lines[lineCount + 1] == "{")
                {
                    //since we need the part that we need to loop until x == true, we first get and save the lines of the file/textArray
                    WhileLoop whileLoop = new WhileLoop();
                    whileLoop.Interperate(line, textArray, lines, fileLoc);
                    return;
                }
                else if (line.StartsWith("func ") && line.EndsWith(") {")) // || line.EndsWith("()" + (lines[lineCount + 1] == "{")
                {
                    MethodCode methodCode = new MethodCode();
                    methodCode.Interperate(line, textArray, lines, fileLoc, true);
                    return;
                }
                else if (line.StartsWith($"FileSystem.MakeFile(") || line.StartsWith($"MakeFile(") && line.EndsWith("}"))
                {
                    FileSystem_MakeFile fs_mkFile = new FileSystem_MakeFile();
                    fs_mkFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.MakeFolder(") || line.StartsWith($"MakeFolder(") && line.EndsWith("}"))
                {
                    FileSystem_MakeFolder fs_mkFolder = new FileSystem_MakeFolder();
                    fs_mkFolder.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.DeleteFile(") || line.StartsWith($"DeleteFile(") && line.EndsWith("}"))
                {
                    FileSystem_DeleteFile fs_delFile = new FileSystem_DeleteFile();
                    fs_delFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.DeleteFolder(") || line.StartsWith($"DeleteFolder(") && line.EndsWith("}"))
                {
                    FileSystem_DeleteFolder fs_delFolder = new FileSystem_DeleteFolder();
                    fs_delFolder.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.ReadFile(") || line.StartsWith($"ReadFile(") && line.EndsWith("}"))
                {
                    FileSystem_ReadFile fs_readFile = new FileSystem_ReadFile();
                    fs_readFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.RenameFile(") || line.StartsWith($"RenameFile(") && line.EndsWith("}"))
                {
                    FileSystem_RenameFile fs_renameFile = new FileSystem_RenameFile();
                    fs_renameFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.WriteFile(") || line.StartsWith($"WriteFile(") && line.EndsWith("}"))
                {
                    FileSystem_WriteFile fs_writeFile = new FileSystem_WriteFile();
                    fs_writeFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"Network.Ping(") || line.StartsWith($"Ping(") && line.EndsWith("}"))
                {
                    NetworkPing netPing = new NetworkPing();
                    netPing.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"Time.CurrentTime(") || line.StartsWith($"CurrentTime(") && line.EndsWith("}"))
                {
                    Time_CurrentTime currentTime = new Time_CurrentTime();
                    string time = currentTime.Interperate(line, textArray, fileLoc);
                    Console.WriteLine(time);
                }
                else if (line.StartsWith($"Time.IsLeapYear(") || line.StartsWith($"IsLeapYear(") && line.EndsWith("}"))
                {
                    Time_IsLeapYear isLeapYear = new Time_IsLeapYear();
                    string isLeapYear_str = isLeapYear.Interperate(line, textArray, fileLoc);
                    Console.WriteLine(Convert.ToBoolean(isLeapYear_str));
                }
                else if (line.StartsWith($"Random.RandomRange(") || line.StartsWith($"RandomRange(") && line.EndsWith("}"))
                {
                    Random_RandomRange randomRange = new Random_RandomRange();
                    string randomRange_str = randomRange.Interperate(line, textArray, fileLoc);
                    Console.WriteLine(randomRange_str);
                }
                else if (line.StartsWith($"sdl2.makeWindow(") || line.StartsWith($"makeWindow(") && line.EndsWith(")"))
                {
                    SDL2_makeWindow makeWindow = new SDL2_makeWindow();
                    string code_line = line.Replace("sdl2.", "").Replace("makeWindow(", "");
                    code_line = code_line.Substring(0, code_line.Length - 2);
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
                    new Task(() => { window_int = makeWindow.Interperate(sizeX, sizeY, posX, posY, title); }).Start();
                    //window_int = makeWindow.Interperate(sizeX, sizeY, posX, posY, title);
                    window = (IntPtr)window_int;
                    Thread.Sleep(100);
                    continue;
                }
                else if (line.StartsWith($"sdl2.createShape(") || line.StartsWith($"createShape(") && line.EndsWith(")"))
                {
                    SDL2_createShape createShape = new SDL2_createShape();
                    string code_line = line.Replace("sdl2.", "").Replace("createShape(", "");
                    code_line = code_line.Substring(0, code_line.Length - 2);
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

                    new Task(() => { createShape.Interperate(window, x, y, w, h); }).Start();
                }
                else if (line.StartsWith($"sdl2.clearScreen(") || line.StartsWith($"clearScreen(") && line.EndsWith(")"))
                {
                    SDL2_clearScreen clearScreen = new SDL2_clearScreen();
                    string code_line = line.Replace("sdl2.", "").Replace("clearScreen(", "");
                    code_line = code_line.Substring(0, code_line.Length - 2);
                    long window = 0;
                    string color = null;
                    string[] values = code_line.Split(",");
                    window = Convert.ToInt64(values[0]);
                    color = values[1];
                    clearScreen.Interperate(window, color);
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
                                methodCode.Interperate(line, textArray, lines, fileLoc, false);
                                return;
                            }
                            foreach (string file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
                            {
                                string supposedVar = file[file.LastIndexOf("\\")..].Replace(".txt", "").Substring(1);
                                if (line.StartsWith(supposedVar))
                                {
                                    if (line.Contains('=') && (line.IndexOf("+") + 1) != line.IndexOf("=") && line.Count(f => (f == '=')) == 1)
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1).Replace(".txt", "") + " = ";
                                        string content = line.Replace(partToReplace, "");
                                        if (content.Contains('+') && content.Count(f => (f == '+')) == 1)
                                        {
                                            Math_Add math_Add = new Math_Add();
                                            int result = math_Add.Interperate(content, 0, supposedVar);
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
                                        content = content[1..][..(content.Length - 2)];
                                        content = content[5..];
                                        content = content[..^1];
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
                                        content = content[1..][..(content.Length - 2)];
                                        content = content[5..];
                                        content = content[..^1];
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

                        if (line.Contains("."))
                        {
                            string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Functions");
                            List<string> allNamespacesAvaiable_list = new List<string>(allNamespacesAvaiable_array);
                            List<string> allNamespacesAvaiable_list_main = new List<string>();
                            foreach (string item in allNamespacesAvaiable_list)
                            {
                                allNamespacesAvaiable_list_main.Add(item[(item.LastIndexOf("\\") + 1)..]);
                            }
                            allNamespacesAvaiable_array = allNamespacesAvaiable_list_main.ToArray();
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

                                //theFunctionOfTheLine = theFunctionOfTheLine.Replace("(", "").Replace(");", "");

                                //Older
                                /*Type type_ = Type.GetType(theFunctionOfTheLine);
                                MethodInfo method = type_.GetMethod("run");
                                method.Invoke(null, null);*/

                                //Old
                                //Activator.CreateInstance(Convert.ToString(Assembly.GetExecutingAssembly()), Convert.ToString(Type.GetType(theFunctionOfTheLine)));                            

                                string[] code =
                                {
                                $"Easy14_Programming_Language.{theFunctionOfTheLine} myfunc = new Easy14_Programming_Language.{theFunctionOfTheLine}();",
                                $"myfunc.Interperate({string.Join(",", params_)});"
                                };

                                try
                                {
                                    CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error: The function you are trying to use is not defined.");
                                }
                                return;
                            }

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"ERROR; '{line}' is not a vaild code statement\n  Error was located on Line {lineCount}");
                            Console.ResetColor();
                            break;
                        }
                    }
                }
                lineCount++;
            }
        }
    }
}

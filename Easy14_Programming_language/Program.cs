using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SDL2;
using System;
using System.Collections.Generic;
using System.Data;
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
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));
        readonly static string tempVariableFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";

        static void Main(string[] args)
        {
            Console.WriteLine("\n==== Easy14 Console ====\n");

            if (Directory.Exists(tempVariableFolder))
            {
                Directory.Delete(tempVariableFolder, true);
            }

            //==================The Update Checker====================\\

            /* Checking if the user has disabled updates in the config file. If they have not, it will
            check for updates. */

            bool UpdatesDisabled = Convert.ToBoolean(Easy14_configuration.getBoolOption("UpdatesDisabled"));

            bool UpdatesWarningsDisabled = Convert.ToBoolean(Easy14_configuration.getBoolOption("UpdatesWarningsDisabled"));

            if (!UpdatesDisabled)
            {
                updateChecker.checkLatestVersion(UpdatesWarningsDisabled);
            }

            //=========================================================\\

            /* Reading the config file and getting the delay value. */
            foreach (string line in configFile)
            {
                if (line.StartsWith("delay"))
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(line.Replace("delay = ", "")) * 1000); break;
                    }
                    catch (FormatException formatException)
                    {
                        ThrowErrorMessage.sendErrMessage(formatException.InnerException + "; unable to set delay, delaying by (default) 3 seconds", null, "warning");
                    }
                }
            }

            /* The below code is the code that runs when the app is ran, it checks for arguments
            and runs the code accordingly */
            if (args.Length > 0)
            {
                //Very bad way of geting a file from arguments but "if it works, dont touch it ever"
                if (File.Exists(args[0]))
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

                    Console.WriteLine($"==== {DateTime.Now} | {string.Join(" ", args)} ====\n");

                    if (File.Exists(string.Join(" ", args)))
                    {
                        Console.WriteLine("\n ERROR; CAN NOT FIND FILE SPECIFIED \n");
                    }

                    if (string.Join(" ", args).EndsWith(".ese14") || string.Join(" ", args).EndsWith(".e14"))
                    {
                        try
                        {
                            Program prog = new Program();
                            prog.CompileCode_fromOtherFiles(textArray: File.ReadAllLines(string.Join("", args)));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"\n An Error Occured While Reading File To Display in Preview, Below Is the Full Error Exception Message\n\n{e}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n Uh Oh, this file isnt an actual .e14/.ese14 file!, please change the file extention to .e14 (preferred) or .ese14 to use this file \n");
                    }
                }
                if (args[0] == "/help")
                {
                    HelpCommandCode.DisplayDefaultHelpOptions();
                }
                if (args[0].ToLower() == "/intro")
                {
                    IntroductionCode.IntroCode();
                }
                if (args[0].ToLower() == "/appinfo")
                {
                    AppInformation.ShowInfo();
                }
            }
            else if (args.Length == 0)
            {
                Console.WriteLine("No Arguments to Given :/");
                Console.WriteLine("if you need help with what args you can run, run the app with argument '/help' to get args help");
            }

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
                        {
                            try
                            {
                                Console.WriteLine($"Capslock Status; {Console.CapsLock}");
                            }
                            catch { }
                        }
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

                if (command.ToLower() == "exit();")
                {
                    return;
                }

                else if (command.ToLower() == "exit")
                {
                    CSharpErrorReporter.ConsoleLineReporter.Warning("\nPlease use \"exit();\" or Ctrl+C to close the interative console", "e"); continue;
                }
                else if (command.ToLower().StartsWith("/run"))
                {
                    Program prog = new Program();
                    prog.CompileCode_fromOtherFiles(command.TrimStart().Substring(4));
                }
                else if (command.ToLower() == "/help")
                {
                    HelpCommandCode.DisplayDefaultHelpOptions();
                }
                else if (command.ToLower() == "/intro")
                {
                    IntroductionCode.IntroCode();
                }
                else if (command.ToLower() == "/appinfo")
                {
                    AppInformation.ShowInfo();
                }
                else
                {
                    if (!command.StartsWith("/"))
                    {
                        string[] allNamespaces_array = Directory.GetDirectories(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location)))))))) + "\\Functions");
                        List<string> allNamespaces_list = new List<string>();
                        foreach (string namespace_ in allNamespaces_array)
                        {
                            allNamespaces_list.Add(string.Concat("using ", namespace_.AsSpan(namespace_.LastIndexOf("\\") + 1), ";"));
                        }

                        string[] allNamespaces = allNamespaces_list.ToArray();
                        string[] command_Array = new string[allNamespaces.Length + new string[] { command }.Length];
                        Array.Copy(allNamespaces, command_Array, allNamespaces.Length);
                        Array.Copy(new string[] { command }, 0, command_Array, allNamespaces.Length, new string[] { command }.Length);

                        Program prog = new Program();
                        prog.CompileCode_fromOtherFiles(null, command_Array, 0, false, "}");
                    }
                    else
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Error("ERROR; Easy14 Interactive can't understand what the args you specified \n");
                        continue;
                    }
                }
            }
        }

        //=========================== SEPERATOR =========================\\








        //=========================== SEPERATOR =========================\\

        public void CompileCode_fromOtherFiles(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {
            CompileCode(fileLoc, textArray, lineIDX, isInAMethod, methodName);
        }

        public static void CompileCode(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {
            bool disableLibraries = false;
            int windowHeight = Console.WindowHeight;
            int windowWidth = Console.WindowWidth;
            string windowState = "normal";

            disableLibraries = Convert.ToBoolean(Easy14_configuration.getBoolOption("disableLibraries"));
            if ((bool)Easy14_configuration.getBoolOption("windowHeight") != false)
            {
                windowHeight = Convert.ToInt32(Easy14_configuration.getBoolOption("windowHeight"));
            }
            if ((bool)Easy14_configuration.getBoolOption("windowWidth") != false)
            {
                windowWidth = Convert.ToInt32(Easy14_configuration.getBoolOption("windowWidth"));
            }
            if ((string)Easy14_configuration.getBoolOption("windowState") != "false")
            {
                windowState = (string)Easy14_configuration.getBoolOption("windowState");
            }

            if ((bool)Easy14_configuration.getBoolOption("showOptionsINI_DataWhenE14_Loads") != false)
            {
                if ((string)Easy14_configuration.getBoolOption("showOptionsINI_DataWhenE14_Loads") == "true")
                {
                    List<string> configFileLIST = new List<string>();

                    foreach (string line_ in configFile)
                    {
                        if (line_.StartsWith(";") || line_ == "" || line_ == " ") continue;
                        configFileLIST.Add(line_);
                    }

                    string[] configFile_modified = configFileLIST.ToArray();

                    Console.WriteLine(string.Join(Environment.NewLine, configFile_modified)); Console.WriteLine("\n========================\n\n");
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

            if (windowState == "maximized") ShowWindow(ThisConsole, MAXIMIZE);
            else if (windowState == "minimized") ShowWindow(ThisConsole, MINIMIZE);
            else if (windowState == "hidden") ShowWindow(ThisConsole, HIDE);
            else if (windowState == "restore") ShowWindow(ThisConsole, RESTORE);

            // ============================================================ //

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
                        CSharpErrorReporter.ConsoleLineReporter.Error("Uh oh, the value you wanted to specify for the Console Window Hight won't work! (check if the value is a string/decimal and change it to an integer)");
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Couldn't Change Window Height using the value in options.ini, using Default window height");
                        CSharpErrorReporter.ConsoleLineReporter.Error("Here is Exception Error;\n" + e.Message);
                    }
                }

                if (Console.WindowWidth != windowWidth)
                {
                    try
                    {
                        Console.SetWindowSize(windowWidth > 150 ? windowWidth = 150 : windowWidth, Console.WindowHeight);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Error("Uh oh, the value you wanted to specify for the Console Window Width won't work! (check if the value is a string/decimal and change it to an integer)");
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Couldn't Change Window Width using the value in options.ini, using Default window width");
                    }
                }
            }
            else if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                CSharpErrorReporter.ConsoleLineReporter.Error("Changing terminal size on a system other than Windows with C# isn't Possible");
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
                ExceptionSender.SendException("0xFC00001");
            }

            /* Removing the first lineIDX lines from the list. */
            List<string> lines_list = new List<string>(lines);
            if (lineIDX != 0)
            {
                lines_list.RemoveRange(0, lineIDX);
            }

            /* Removing the leading whitespace from each line in the list. */
            List<string> lines_list_mod = new List<string>();

            if (lines != null)
            {
                lines = formatUserCode.format(lines);
            }
            if (textArray != null)
            {
                textArray = formatUserCode.format(textArray);
            }

            foreach (string line in lines)
            {
                char[] line_chrArr = line.ToCharArray();

                if (showCommands == true)
                {
                    Console.WriteLine(">>>" + line);
                }

                if (line.StartsWith($"using") && line.EndsWith($";"))
                {
                    Using_namespace_code.usingFunction_interp(line, disableLibraries, lineCount);
                }
                else if (line.StartsWith($"from") && line.EndsWith($";"))
                {
                    Using_namespace_code.fromFunction_interp(line, disableLibraries, lineCount);
                }
                else if (string.Join("", line_chrArr) != "" && char.IsDigit(line_chrArr[0]))
                {
                    string statement = string.Join("", line_chrArr);
                    try
                    {
                        Console.WriteLine(Convert.ToDouble(new DataTable().Compute(statement, null)));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.GetType());
                        if (e.GetType().ToString() == "System.OverflowException")
                        {
                            ThrowErrorMessage.sendErrMessage("The Number to calculate is too large! (For Int32), please try a number less than 2147483647", null, "error");
                        }
                        else
                        {
                            ThrowErrorMessage.sendErrMessage("Uh oh, the value you wanted to calculate won't work! (check if the value has a string value and change it to an integer)", null, "error");
                        }
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
                    Console.ResetColor(); continue;
                }

                else if (line.StartsWith($"Console.print(") || line.StartsWith($"print("))
                {
                    ConsolePrint.Interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"Console.input(") || line.StartsWith($"input("))
                {
                    ConsoleInput.Interperate(line, lines, textArray, fileLoc, null);
                }
                else if (line.StartsWith($"Console.clear(") || line.StartsWith($"clear("))
                {
                    ConsoleClear.Interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"Console.exec(") || line.StartsWith($"exec("))
                {
                    ConsoleExec.Interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"Console.beep(") || line.StartsWith($"beep("))
                {
                    AudioPlay.Interperate(line, textArray, fileLoc);
                }
                else if (line.StartsWith($"wait(") && line.EndsWith(");"))
                {
                    TimeWait.Interperate(line, lineCount);
                }
                else if (line.StartsWith($"var") && line.EndsWith(";"))
                {
                    VariableCode_fixed.Interperate(line, lines, lineCount);
                }
                else if (line.StartsWith($"Random.RandomRange(") || line.StartsWith($"RandomRange("))
                {
                    Console.WriteLine(Random_RandomRange.Interperate(line, textArray, fileLoc));
                }
                else if (line.StartsWith($"Random.RandomRangeDouble(") || line.StartsWith($"RandomRangeDouble("))
                {
                    Console.WriteLine(Random_RandomRangeDouble.Interperate(line, textArray, fileLoc));
                }
                else if (line.StartsWith($"ToString(") && line.EndsWith(");"))
                {
                    Console.WriteLine(ConvertToString.Interperate(line));
                }
                else if ((line.StartsWith($"if") && line.EndsWith("{")) || (line.StartsWith("if") && lines[lineCount] == "{"))
                {
                    If_Loop.Interperate(line, lines, textArray, fileLoc, isInAMethod, methodName); return;
                }
                else if ((line.StartsWith($"while") && line.EndsWith("{")) || (line.StartsWith("while") && lines[lineCount] == "{"))
                {
                    While_Loop.Interperate(line, lines, textArray, fileLoc); return;
                }
                else if ((line.StartsWith("func ") && line.EndsWith(") {")) || (line.StartsWith("func ")))
                {
                    Method_Code.Interperate(line, textArray, lines, fileLoc, true); return;
                }
                else if (line.StartsWith($"FileSystem.MakeFile(") || line.StartsWith($"MakeFile("))
                {
                    FileSystem_MakeFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.MakeFolder(") || line.StartsWith($"MakeFolder("))
                {
                    FileSystem_MakeFolder.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.DeleteFile(") || line.StartsWith($"DeleteFile("))
                {
                    FileSystem_DeleteFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.DeleteFolder(") || line.StartsWith($"DeleteFolder("))
                {
                    FileSystem_DeleteFolder.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.ReadFile(") || line.StartsWith($"ReadFile("))
                {
                    FileSystem_ReadFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.RenameFile(") || line.StartsWith($"RenameFile("))
                {
                    FileSystem_RenameFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"FileSystem.WriteFile(") || line.StartsWith($"WriteFile("))
                {
                    FileSystem_WriteFile.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"Network.Ping(") || line.StartsWith($"Ping("))
                {
                    NetworkPing.Interperate(line, fileLoc, textArray, lineCount);
                }
                else if (line.StartsWith($"Time.CurrentTime(") || line.StartsWith($"CurrentTime("))
                {
                    Console.WriteLine(Time_CurrentTime.Interperate(line, textArray, fileLoc));
                }
                else if (line.StartsWith($"Time.IsLeapYear(") || line.StartsWith($"IsLeapYear("))
                {
                    Console.WriteLine(Convert.ToBoolean(Time_IsLeapYear.Interperate(line, textArray, fileLoc)));
                }
                else if (line.StartsWith($"Random.RandomRange(") || line.StartsWith($"RandomRange("))
                {
                    Console.WriteLine(Random_RandomRange.Interperate(line, textArray, fileLoc));
                }
                else if (line.StartsWith($"sdl2.makeWindow(") || line.StartsWith($"makeWindow("))
                {
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
                    new Task(() => { window_int = SDL2_makeWindow.Interperate(sizeX, sizeY, posX, posY, title); }).Start();
                    //window_int = makeWindow.Interperate(sizeX, sizeY, posX, posY, title);
                    window = (IntPtr)window_int;
                    Thread.Sleep(100);
                    continue;
                }
                else if (line.StartsWith($"sdl2.createShape(") || line.StartsWith($"createShape("))
                {
                    string code_line = line.Replace("sdl2.", "").Replace("createShape(", "");
                    code_line = code_line.Substring(0, code_line.Length - 2);
                    string[] values = code_line.Split(",");
                    long window = 0;
                    int x = 0;
                    int y = 0;
                    int w = 0;
                    int h = 0;

                    try
                    {
                        window = Convert.ToInt64(values[0]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Error("Failed to get Window parameter");
                        return;
                    }

                    try
                    {
                        x = Convert.ToInt32(values[1]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Failed to get x (right) position parameter");
                        x = int.MaxValue;
                    }

                    try
                    {
                        y = Convert.ToInt32(values[2]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Failed to get y (up) position parameter");
                        y = int.MaxValue;
                    }

                    try
                    {
                        w = Convert.ToInt32(values[3]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Failed to get w (width) size parameter");
                        w = int.MaxValue;
                    }

                    try
                    {
                        h = Convert.ToInt32(values[4]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Failed to get h (height) size parameter");
                        h = int.MaxValue;
                    }

                    new Task(() => { SDL2_createShape.Interperate(window, x, y, w, h); }).Start();
                }
                else if (line.StartsWith($"sdl2.clearScreen(") || line.StartsWith($"clearScreen("))
                {
                    string code_line = line.Replace("sdl2.", "").Replace("clearScreen(", "");
                    code_line = code_line.Substring(0, code_line.Length - 2);
                    long window = 0;
                    string color = null;
                    string[] values = code_line.Split(",");
                    window = Convert.ToInt64(values[0]);
                    color = values[1];
                    SDL2_clearScreen.Interperate(window, color);
                }
                else
                {
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
                    {
                        if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP").Length > -1)
                        {
                            if (line.EndsWith("();")) //Means its probably a function
                            {
                                Method_Code.Interperate(line, textArray, lines, fileLoc, false);
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
                                            int result = Math_Add.Interperate(content, 0, supposedVar);
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
                                        catch { }

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
                        string funcLine = null;
                        if (line.Contains("("))
                        {
                            try
                            {
                                funcLine = line.Substring(0, line.IndexOf("("));
                            }
                            catch
                            {
                                CSharpErrorReporter.ConsoleLineReporter.Error(line + " is not a valid line.");
                                return;
                            }
                        }
                        if (funcLine != null)
                        {
                            if (funcLine.Contains("."))
                            {
                                string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0-windows", "").Replace("\\bin\\Release\\net6.0-windows", "") + "\\Functions");
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

                                    try
                                    {
                                        params_ = params_str.Split(",");
                                    }
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
                                        $"Easy14_Programming_Language.{theFunctionOfTheLine}.Interperate({string.Join(",", params_)});"
                                    };

                                    try
                                    {
                                        CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                                    }
                                    catch (Exception e)
                                    {
                                        CSharpErrorReporter.ConsoleLineReporter.Error("The function you are trying to use returned an Error");
                                        Console.WriteLine($"\n{e.Message}");
                                    }
                                    return;
                                }
                            }
                            else if (!funcLine.Contains("."))
                            {
                                string[] allNamespacesAvaiable_array = Directory.GetDirectories(strWorkPath.Replace("\\bin\\Debug\\net6.0-windows", "").Replace("\\bin\\Release\\net6.0-windows", "") + "\\Functions");
                                List<string> allNamespacesAvaiable_list = new List<string>(allNamespacesAvaiable_array);
                                List<string> allNamespacesAvaiable_list_main = new List<string>();
                                foreach (string item in allNamespacesAvaiable_list)
                                {
                                    allNamespacesAvaiable_list_main.Add(item[(item.LastIndexOf("\\") + 1)..]);
                                }
                                allNamespacesAvaiable_array = allNamespacesAvaiable_list_main.ToArray();
                                string theFunctionOfTheLine = line;
                                int index = Array.IndexOf(allNamespacesAvaiable_array, theFunctionOfTheLine);
                                string params_str = line.Replace($"{theFunctionOfTheLine}.{theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("))}(", "");

                                params_str = params_str.Substring(1, params_str.Length - 2);
                                params_str = params_str.Substring(params_str.IndexOf("("), params_str.Length - params_str.IndexOf("("));
                                params_str = params_str.Substring(1, params_str.Length - 2);
                                string[] params_ = { };

                                try
                                {
                                    params_ = params_str.Split(",");
                                }
                                catch { } // Now we know this is a method without parameters

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
                                    $"Easy14_Programming_Language.{theFunctionOfTheLine}.Interperate({string.Join(",", params_)});"
                                };

                                try
                                {
                                    CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                                }
                                catch (Exception e)
                                {
                                    CSharpErrorReporter.ConsoleLineReporter.Error("C# EXCEPTION ERROR; " + e.GetType().Name, e.Message);
                                    Console.WriteLine("CSHARP_Error");
                                }
                                return;
                            }
                            else
                            {
                                CSharpErrorReporter.ConsoleLineReporter.Error($"\'{line}\' is not a vaild code statement\n  Error was located on Line {lineCount - 13}");
                                break;
                            }
                        }
                        else
                        {
                            CSharpErrorReporter.ConsoleLineReporter.Error($"\'{line}\' is not a vaild code statement\n  Error was located on Line {lineCount - 13}");
                            break;
                        }
                    }
                }
                lineCount++;
            }
        }

    }
}
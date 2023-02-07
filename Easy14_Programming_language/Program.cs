﻿using Microsoft.CodeAnalysis;
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
        // Show statements during runtime
        public static bool showStatementsDuringRuntime = false;

        // Display file contents before runtime
        public static bool DisplayFileContentsBeforeRuntime = false;

        // Root path of the assembly
        static string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // Parent path of the root path
        static string parentPath = Path.GetDirectoryName(Directory.GetParent(Directory.GetParent(rootPath).FullName).FullName);

        // Path to the options file
        static string optionsPath = "Application Code\\options.ini";

        // Array of contents in the options file
        static string[] configFile = File.ReadAllLines(optionsPath);

        // Desktop path of the system
        static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        // Temp path
        static string tempPath = Path.Combine(desktopPath, "EASY14_Variables_TEMP");

        // Path to the current executing assembly
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;

        // Path to the working directory
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);

        // Temporary variable folder
        readonly static string TemporaryVariableFolder = tempPath;

        // Path to the current version file
        readonly static string version = Path.Combine(parentPath, @"Application Code\currentVersion.txt");



        static void Main(string[] args)
        {
            string osName = "Not Detected";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) osName = "Windows";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) osName = "Linux";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) osName = "OSX";
            else osName = "Unknown";

            Console.WriteLine("Easy14 " + File.ReadAllLines(version)[1] + " on " + osName);
            Console.WriteLine("Type in \"help\" or \"info\" for information");

            if (Directory.Exists(TemporaryVariableFolder))
            {
                Directory.Delete(TemporaryVariableFolder, true);
            }


            //==================The Update Checker====================\\

            /* Checking if the user has disabled updates in the config file. If they have not, it will
            check for updates. */

            bool UpdatesDisabled = Convert.ToBoolean(Easy14_configuration.GetBoolOption("UpdatesDisabled"));

            bool UpdatesWarningsDisabled = Convert.ToBoolean(Easy14_configuration.GetBoolOption("UpdatesWarningsDisabled"));

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

            Console.WriteLine("\n===== Easy14 =====\n");
            foreach (string line in configFile)
            {
                if (line.StartsWith("turnOnDevOptions"))
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
                Console.Write(":>");
                string line = Console.ReadLine();
                if (line.ToLower() == "")
                {
                    continue;
                }
                if (line.ToLower() == "exit();")
                {
                    return;
                }

                else if (line.ToLower() == "exit")
                {
                    ErrorReportor.ConsoleLineReporter.Warning("\nPlease use \"exit();\" or Ctrl+C to close the interative console"); continue;
                }
                else if (line.ToLower().StartsWith("/run"))
                {
                    Program compiler = new Program();
                    compiler.CompileCode_fromOtherFiles(line.TrimStart().Substring(4));
                }
                else if (line.ToLower() == "/help")
                {
                    HelpCommandCode.DisplayDefaultHelpOptions();
                }
                else if (line.ToLower() == "/intro")
                {
                    IntroductionCode.IntroCode();
                }
                else if (line.ToLower() == "/appinfo")
                {
                    AppInformation.ShowInfo();
                }
                else
                {
                    if (!line.StartsWith("/"))
                    {
                        Program prog = new Program();
                        prog.CompileCode_fromOtherFiles(null, new string[] { line }, 0, false, "}");
                    }
                }
            }
        }

        //=========================== SEPERATOR =========================\\








        //=========================== SEPERATOR =========================\\

        public object CompileCode_fromOtherFiles(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {
            return CompileCode(fileLoc, textArray, lineIDX, isInAMethod, methodName);
        }

        public static object CompileCode(string FileLocation = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {
            bool librariesDisabled = false;
            int windowHeight = Console.WindowHeight;
            int windowWidth = Console.WindowWidth;
            string windowState = "normal";

            librariesDisabled = Convert.ToBoolean(Easy14_configuration.GetBoolOption("disableLibraries"));

            windowHeight = Convert.ToInt32(Easy14_configuration.GetBoolOption("windowHeight"));
            windowWidth = Convert.ToInt32(Easy14_configuration.GetBoolOption("windowWidth"));
            windowState = (string)Easy14_configuration.GetBoolOption("windowState");

            if ((bool)Easy14_configuration.GetBoolOption("showOptionsINI_DataWhenE14_Loads") != false)
            {
                if ((string)Easy14_configuration.GetBoolOption("showOptionsINI_DataWhenE14_Loads") == "true")
                {
                    List<string> configFileLIST = new List<string>();

                    foreach (string currentLine in configFile)
                    {
                        if (currentLine.StartsWith(";") || currentLine == "" || currentLine == " ") continue;
                        configFileLIST.Add(currentLine);
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
                        if (windowHeight < 50)
                        {
                            windowHeight = 50;
                        }
                        Console.SetWindowSize(Console.WindowWidth, windowHeight);
                    }
                    catch (Exception e)
                    {
                        ErrorReportor.ConsoleLineReporter.Error("The Console Window Height value must be an integer, not a string or decimal");
                        ErrorReportor.ConsoleLineReporter.Warning("Could not change window height using options.ini, using default instead.");
                        ErrorReportor.Logger.Error(e, "", ErrorReportor.EASY14_IO_FILE_ERROR);
                    }
                }

                if (Console.WindowWidth != windowWidth)
                {
                    try
                    {
                        if (windowWidth < 150)
                        {
                            windowWidth = 150;
                        }
                        Console.SetWindowSize(windowWidth, Console.WindowHeight);
                    }
                    catch
                    {
                        ErrorReportor.ConsoleLineReporter.Error("Uh oh, the value you wanted to specify for the Console Window Width won't work! (check if the value is a string/decimal and change it to an integer)");
                        ErrorReportor.ConsoleLineReporter.Warning("Couldn't Change Window Width using the value in options.ini, using Default window width");
                    }
                }
            }
            else if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ErrorReportor.ConsoleLineReporter.Error("Changing terminal size on a system other than Windows with C# isn't Possible");
            }

            /* Reading the file and storing it in a string array. */
            int lineCount = 1;
            string[] statements = null;
            try
            {
                if (textArray == null && FileLocation != null)
                {
                    statements = File.ReadAllLines(FileLocation);
                }
                else if (textArray != null && FileLocation == null)
                {
                    statements = textArray;
                }
            }
            catch
            {
                ExceptionSender.SendException("0xFC00001");
            }

            /* Removing the first lineIDX lines from the list. */
            List<string> linesList = new List<string>(statements);
            if (lineIDX != 0)
            {
                linesList.RemoveRange(0, lineIDX);
            }

            /* Removing the leading whitespace from each line in the list. */
            List<string> linesListMod = new List<string>();

            if (statements != null)
            {
                statements = formatUserCode.format(statements);
            }
            if (textArray != null)
            {
                textArray = formatUserCode.format(textArray);
            }

            foreach (string statement in statements)
            {
                string[] statementSplitSpace = statement.Split(" ");
                string[] statementSplitDot = statement.Split(".");
                List<string> statementListSplit = new List<string>(statement.Split(" "));

                if (showStatementsDuringRuntime == true)
                {
                    Console.WriteLine(">>>" + statement);
                }

                if (statement.StartsWith($"using") && statement.EndsWith($";"))
                {
                    UsingNamspaceFunction.UsingFunction(statement, librariesDisabled, lineCount);
                }
                else if (statement.StartsWith($"from") && statement.EndsWith($";"))
                {
                    UsingNamspaceFunction.ForFunction(statement, librariesDisabled, lineCount);
                }
                else if (string.Join("", statement.ToCharArray()) != "" && char.IsDigit(statement.ToCharArray()[0]))
                {
                    string expression = string.Join("", statement.ToCharArray());
                    try
                    {
                        Console.WriteLine(Convert.ToDouble(new DataTable().Compute(expression, null)));
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
                else if (statement.ToLower() == "exit()" || statement.ToLower() == "exit();")
                {
                    return "";
                }
                else if (statement.ToLower() == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPlease use \"exit();\" or Ctrl+C to close the interative console");
                    Console.ResetColor(); continue;
                }
                else if (statementSplitDot[0] == "Console")
                {
                    if (statementSplitDot[1].StartsWith("Print"))
                    {
                        ConsolePrint.Interperate(statementSplitDot[1]);
                    }
                    else if (statementSplitDot[1].StartsWith("Input"))
                    {
                        ConsoleInput.Interperate(statementSplitDot[1]);
                    }
                    else if (statementSplitDot[1].StartsWith("Clear"))
                    {
                        ConsoleClear.Interperate();
                    }
                    else if (statementSplitDot[1].StartsWith("Execute") || statementSplitDot[1].StartsWith("Exec"))
                    {
                        ConsoleExec.Interperate(statementSplitDot[1]);
                    }
                    else if (statementSplitDot[1].StartsWith("Beep"))
                    {
                        ConsoleBeep.Interperate($"{statementSplitDot[2]} {statementSplitDot[3]}");
                    }
                }
                else if (statementSplitDot[0] == "Time")
                {
                    if (statementSplitDot[1] == "Wait")
                    {
                        TimeWait.Interperate(statementSplitDot[2]);
                    }
                }
                else if (statementSplitSpace[0] == "var")
                {
                    if (statement.EndsWith(";") && statementSplitSpace[2] != "=")
                    {
                        VariableCode.Interperate(statementSplitSpace[1]);
                    }
                    else if (statementSplitSpace[2] == "=" && statement.EndsWith(";"))
                    {
                        VariableCode.Interperate(statementSplitSpace[1], statementSplitSpace[3]);
                    }
                }
                else if (VariableCode.variableList.ContainsKey(statementSplitSpace[0]))
                {
                    Console.WriteLine(VariableCode.variableList[statementSplitSpace[0]]);
                }
                else if (statementSplitDot[0] == "Random")
                {
                    if (statementSplitDot[1] == "Range")
                    {
                        return Random_RandomRange.Interperate(String.Join(" ", statementListSplit.GetRange(2, statementListSplit.Count - 2)));
                    }
                    else if (statementSplitDot[1] == $"RangeDouble")
                    {
                        Random_RandomRangeDouble.Interperate(statement, textArray, FileLocation);
                    }
                }
                else if (statement.StartsWith($"ToString(") && statement.EndsWith(");"))
                {
                    ConvertToString.Interperate(statement);
                }
                try
                {
                    if (statementSplitSpace[0] == $"if" && (statement.EndsWith("{") || statements[lineCount] == "{"))
                    {
                        If_Loop.Interperate(statement, statements, textArray, FileLocation, isInAMethod, methodName); return "";
                    }
                    else if (statementSplitSpace[0] == $"while" && (statement.EndsWith("{") || statements[lineCount] == "{"))
                    {
                        While_Loop.Interperate(statement, statements, textArray, FileLocation); return "";
                    }
                    else if (statementSplitSpace[0] == "func " && (statement.EndsWith(") {") || statements[lineCount] == "{"))
                    {
                        Method_Code.Interperate(statement, textArray, statements, FileLocation, true); return "";
                    }
                }
                catch (IndexOutOfRangeException /*indexOutRangeEx*/)
                {
                    ErrorReportor.ConsoleLineReporter.Error("Invalid Syntax", $"Invalid Syntax at line {lineCount}\n{statement}\n");
                }
                if (statementSplitDot[0] == "FileSystem")
                {
                    if (statementSplitDot[1].StartsWith("MakeFile("))
                    {
                        FileSystem_MakeFile.Interperate(statement, FileLocation, textArray, lineCount);
                    }
                    else if (statementSplitDot[1].StartsWith("MakeFolder("))
                    {
                        FileSystem_MakeFolder.Interperate(statement, FileLocation, textArray, lineCount);
                    }
                    else if (statementSplitDot[1].StartsWith("DeleteFile"))
                    {
                        FileSystem_DeleteFile.Interperate(statement, FileLocation, textArray, lineCount);
                    }
                    else if (statementSplitDot[1].StartsWith("DeleteFolder"))
                    {
                        FileSystem_DeleteFolder.Interperate(statement, FileLocation, textArray, lineCount);
                    }
                    else if (statementSplitDot[1].StartsWith("ReadFile"))
                    {
                        FileSystem_ReadFile.Interperate(statement, FileLocation, textArray, lineCount);
                    }
                    else if (statementSplitDot[1].StartsWith("RenameFile"))
                    {
                        FileSystem_RenameFile.Interperate(statement, FileLocation, textArray, lineCount);
                    }
                    else if (statementSplitDot[1].StartsWith("WriteFile"))
                    {
                        FileSystem_WriteFile.Interperate(statement, FileLocation, textArray, lineCount);
                    }
                }
                else if (statementSplitDot[0] == "Network")
                {
                    if (statementSplitDot[1].StartsWith("Ping"))
                    {
                        NetworkPing.Interperate(statement, FileLocation, textArray, lineCount);
                    }
                }
                else if (statementSplitDot[0] == "Time")
                {
                    if (statementSplitDot[1].StartsWith("CurrentTime"))
                    {
                        return Time_CurrentTime.Interperate(statement, textArray, FileLocation);
                    }
                    else if (statementSplitDot[1].StartsWith("IsLeapYear"))
                    {
                        return Convert.ToBoolean(Time_IsLeapYear.Interperate(statement, textArray, FileLocation));
                    }
                }
                else if (statementSplitDot[0] == "sdl2")
                {
                    if (statementSplitDot[1].StartsWith("makeWindow"))
                    {
                        string[] values = statement.Replace("sdl2.", "").Replace("makeWindow(", "").TrimEnd(')').Split(",");
                        int sizeX = 200;
                        int sizeY = 200;
                        int posX = SDL.SDL_WINDOWPOS_UNDEFINED;
                        int posY = SDL.SDL_WINDOWPOS_UNDEFINED;
                        string title = "myWindowTitle";

                        if (values.Length >= 1 && int.TryParse(values[0], out sizeX))
                        {
                            // sizeX was successfully parsed as an integer
                        }

                        if (values.Length >= 2 && int.TryParse(values[1], out sizeY))
                        {
                            // sizeY was successfully parsed as an integer
                        }

                        if (values.Length >= 3 && int.TryParse(values[2], out posX))
                        {
                            // posX was successfully parsed as an integer
                        }

                        if (values.Length >= 4 && int.TryParse(values[3], out posY))
                        {
                            // posY was successfully parsed as an integer
                        }

                        if (values.Length >= 5)
                        {
                            title = values[4];
                        }

                        IntPtr window = (IntPtr)0;
                        long window_int = -1;
                        new Task(() => { window_int = SDL2_makeWindow.Interperate(sizeX, sizeY, posX, posY, title); }).Start();
                        //window_int = makeWindow.Interperate(sizeX, sizeY, posX, posY, title);
                        window = (IntPtr)window_int;
                        Thread.Sleep(100);
                        continue;
                    }
                    if (statementSplitDot[1].StartsWith("createShape"))
                    {
                        string[] values = statement.Replace("sdl2.", "").Replace("createShape(", "").TrimEnd(')').Split(",");
                        long window = 0;
                        int xPosition = 0;
                        int yPosition = 0;
                        int width = 0;
                        int height = 0;

                        if (!long.TryParse(values[0], out window))
                        {
                            ErrorReportor.ConsoleLineReporter.Error("Failed to get Window parameter");
                            return "";
                        }

                        if (values.Length >= 2 && !int.TryParse(values[1], out xPosition))
                        {
                            ErrorReportor.ConsoleLineReporter.Warning("Failed to get x (right) position parameter");
                            xPosition = int.MaxValue;
                        }

                        if (values.Length >= 3 && !int.TryParse(values[2], out yPosition))
                        {
                            ErrorReportor.ConsoleLineReporter.Warning("Failed to get y (up) position parameter");
                            yPosition = int.MaxValue;
                        }

                        if (values.Length >= 4 && !int.TryParse(values[3], out width))
                        {
                            ErrorReportor.ConsoleLineReporter.Warning("Failed to get w (width) size parameter");
                            width = int.MaxValue;
                        }

                        if (values.Length >= 5 && !int.TryParse(values[4], out height))
                        {
                            ErrorReportor.ConsoleLineReporter.Warning("Failed to get h (height) size parameter");
                            height = int.MaxValue;
                        }

                        new Task(() => { SDL2_createShape.Interperate(window, xPosition, yPosition, width, height); }).Start();

                    }
                    if (statementSplitDot[1].StartsWith("clearScreen"))
                    {
                        string code_line = statement.Replace("sdl2.", "").Replace("clearScreen(", "");
                        code_line = code_line.Substring(0, code_line.Length - 2);
                        long window = 0;
                        string color = null;
                        string[] values = code_line.Split(",");
                        window = Convert.ToInt64(values[0]);
                        color = values[1];
                        SDL2_clearScreen.Interperate(window, color);
                    }
                }
                else
                {
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string e14VerTemp_directoryPath = Path.Combine(desktopPath, "EASY14_Variables_TEMP");

                    if (Directory.Exists(e14VerTemp_directoryPath))
                    {
                        if (Directory.GetFiles(e14VerTemp_directoryPath).Length > -1)
                        {
                            if (statement.EndsWith("();"))
                            {
                                // Check if the statement ends with "();", indicating it's probably a function
                                Method_Code.Interperate(statement, textArray, statements, FileLocation, false);
                                return "";
                            }
                            foreach (string file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
                            {
                                string supposedVar = file[file.LastIndexOf("\\")..].Substring(1);

                                if (statement.StartsWith(supposedVar))
                                {
                                    if (statement.Contains('=') && statement.IndexOf("+") != statement.IndexOf("=") - 1 && statement.Count(f => f == '=') == 1)
                                    {
                                        string filePath = file;
                                        string partToReplace = $"{file.Substring(file.LastIndexOf("\\") + 1)} = ";
                                        string content = statement.Replace(partToReplace, "");

                                        if (content.Contains('+') && content.Count(f => f == '+') == 1)
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
                                    if (statement.Contains("+=") && statement.Count(f => f == '=') == 1 && statement.Count(f => f == '+') == 1)
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1) + " = ";
                                        string content = statement.Replace(partToReplace, "");
                                        content = content.Substring(5, content.Length - 6);

                                        string[] fileContents = File.ReadAllLines(filePath);
                                        List<string> fileContentsList = new List<string>(fileContents);
                                        fileContentsList.Add(content);
                                        File.WriteAllText(filePath, string.Join(Environment.NewLine, fileContentsList));
                                        break;
                                    }

                                    /* Removing a line from a var. */
                                    if (statement.Contains("-=") && statement.Count(f => f == '-') == 2)
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1) + " = ";
                                        string content = statement.Replace(partToReplace, "");
                                        content = content.Substring(5, content.Length - 6);

                                        string[] fileContents = File.ReadAllLines(filePath);
                                        List<string> fileContentsList = new List<string>(fileContents);

                                        fileContentsList.Remove(content);

                                        File.WriteAllText(filePath, string.Join(Environment.NewLine, fileContentsList));
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
                    else if (!string.IsNullOrEmpty(statement) && !string.IsNullOrWhiteSpace(statement) && statement != "}" && statement != "break" && statement != "return" && !statement.StartsWith("using") && !statement.StartsWith("//"))
                    {
                        string funcLine = null;
                        if (statement.Contains("("))
                        {
                            try
                            {
                                funcLine = statement.Substring(0, statement.IndexOf("("));
                            }
                            catch
                            {
                                ErrorReportor.ConsoleLineReporter.Error(statement + " is not a valid line.");
                                return "";
                            }
                        }
                        if (funcLine != null)
                        {
                            if (funcLine.Contains("."))
                            {
                                string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net7.0-windows", "").Replace("\\bin\\Release\\net7.0-windows", "") + "\\Functions");
                                List<string> allNamespacesAvaiable_list = new List<string>(allNamespacesAvaiable_array);
                                List<string> allNamespacesAvaiable_list_main = new List<string>();
                                foreach (string item in allNamespacesAvaiable_list)
                                {
                                    allNamespacesAvaiable_list_main.Add(item[(item.LastIndexOf("\\") + 1)..]);
                                }
                                allNamespacesAvaiable_array = allNamespacesAvaiable_list_main.ToArray();
                                string theNamespaceOfTheLine = statement.Split(".")[0];
                                if (allNamespacesAvaiable_array.Contains(theNamespaceOfTheLine))
                                {
                                    int index = Array.IndexOf(allNamespacesAvaiable_array, theNamespaceOfTheLine);
                                    string theClassOfTheLine = statement.Split(".")[0];
                                    string theFunctionOfTheLine = statement.Split(".")[1];
                                    string params_str = statement.Replace($"{theNamespaceOfTheLine}.{theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("))}(", "");
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
                                        return "";//CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                                    }
                                    catch (Exception e)
                                    {
                                        ErrorReportor.ConsoleLineReporter.Error("The function you are trying to use returned an Error");
                                        Console.WriteLine($"\n{e.Message}");
                                    }
                                    return "";
                                }
                            }
                            else if (!funcLine.Contains("."))
                            {
                                string[] allNamespacesAvaiable_array = Directory.GetDirectories(strWorkPath.Replace("\\bin\\Debug\\net7.0-windows", "").Replace("\\bin\\Release\\net7.0-windows", "") + "\\Functions");
                                List<string> allNamespacesAvaiable_list = new List<string>(allNamespacesAvaiable_array);
                                List<string> allNamespacesAvaiable_list_main = new List<string>();
                                foreach (string item in allNamespacesAvaiable_list)
                                {
                                    allNamespacesAvaiable_list_main.Add(item[(item.LastIndexOf("\\") + 1)..]);
                                }
                                allNamespacesAvaiable_array = allNamespacesAvaiable_list_main.ToArray();
                                string theFunctionOfTheLine = statement;
                                int index = Array.IndexOf(allNamespacesAvaiable_array, theFunctionOfTheLine);
                                string params_str = statement.Replace($"{theFunctionOfTheLine}.{theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("))}(", "");

                                params_str = params_str.Substring(1, params_str.Length - 2);
                                params_str = params_str.Substring(params_str.IndexOf("("), params_str.Length - params_str.IndexOf("("));
                                params_str = params_str.Substring(1, params_str.Length - 1);
                                string[] params_ = { };

                                try
                                {
                                    params_ = params_str.Split(",");
                                }
                                catch { } // Now we know this is a method without parameters

                                string theFunctionOfTheLine_params = theFunctionOfTheLine;
                                theFunctionOfTheLine = theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("));
                                string[] code =
                                {
                                    $"Easy14_Programming_Language.{theFunctionOfTheLine}.Interperate({string.Join(",", params_)});"
                                };

                                try
                                {
                                    return CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                                }
                                catch (Exception e)
                                {
                                    ErrorReportor.ConsoleLineReporter.Error("C# EXCEPTION ERROR; " + e.GetType().Name, e.Message);
                                    Console.WriteLine("CSHARP_Error");
                                }
                                return "";
                            }
                            else
                            {
                                //CSharpErrorReporter.ConsoleLineReporter.Error($"\'{statement}\' is not a vaild code statement\n  Error was located on Line {lineCount - 13}");
                                break;
                            }
                        }
                        else
                        {
                            //CSharpErrorReporter.ConsoleLineReporter.Error($"\'{statement}\' is not a vaild code statement\n  Error was located on Line {lineCount - 13}");
                            break;
                        }
                    }
                }
                lineCount++;
            }
            return "";
        }

    }
}
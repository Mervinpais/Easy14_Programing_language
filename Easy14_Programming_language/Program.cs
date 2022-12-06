using Microsoft.CodeAnalysis;
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
            Console.ResetColor();
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

                string line = Console.ReadLine();

                if (line.ToLower() == "exit();")
                {
                    return;
                }

                else if (line.ToLower() == "exit")
                {
                    CSharpErrorReporter.ConsoleLineReporter.Warning("\nPlease use \"exit();\" or Ctrl+C to close the interative console"); continue;
                }
                else if (line.ToLower().StartsWith("/run"))
                {
                    Program prog = new Program();
                    prog.CompileCode_fromOtherFiles(line.TrimStart().Substring(4));
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
                        string[] NamespacesArray = Directory.GetDirectories(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location)))))))) + "\\Functions");
                        List<string> NamespaceList = new List<string>();
                        foreach (string namespace_ in NamespacesArray)
                        {
                            NamespaceList.Add(string.Concat("using ", namespace_.AsSpan(namespace_.LastIndexOf("\\") + 1), ";"));
                        }

                        string[] allNamespaces = NamespaceList.ToArray();
                        string[] command_Array = new string[allNamespaces.Length + new string[] { line }.Length];
                        Array.Copy(allNamespaces, command_Array, allNamespaces.Length);
                        Array.Copy(new string[] { line }, 0, command_Array, allNamespaces.Length, new string[] { line }.Length);

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

        public object CompileCode_fromOtherFiles(string fileLoc = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
        {
            return CompileCode(fileLoc, textArray, lineIDX, isInAMethod, methodName);
        }

        public static object CompileCode(string FileLocation = null, string[] textArray = null, int lineIDX = 0, bool isInAMethod = false, string methodName = "}")
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
            List<string> lines_list = new List<string>(statements);
            if (lineIDX != 0)
            {
                lines_list.RemoveRange(0, lineIDX);
            }

            /* Removing the leading whitespace from each line in the list. */
            List<string> lines_list_mod = new List<string>();

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
                char[] statement_chrArr = statement.ToCharArray();

                if (showCommands == true)
                {
                    Console.WriteLine(">>>" + statement);
                }

                if (statement.StartsWith($"using") && statement.EndsWith($";"))
                {
                    UsingNamspaceFunction.UsingFunction(statement, disableLibraries, lineCount);
                }
                else if (statement.StartsWith($"from") && statement.EndsWith($";"))
                {
                    UsingNamspaceFunction.ForFunction(statement, disableLibraries, lineCount);
                }
                else if (string.Join("", statement_chrArr) != "" && char.IsDigit(statement_chrArr[0]))
                {
                    string expression = string.Join("", statement_chrArr);
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
                    Console.WriteLine("\nPlease use \"exit()\" or \"exit();\" or Ctrl+C to close the interative console");
                    Console.ResetColor(); continue;
                }

                else if (statement.StartsWith($"Console.print(") || statement.StartsWith($"print("))
                {
                    ConsolePrint.Interperate(statement, textArray, FileLocation);
                }
                else if (statement.StartsWith($"Console.input(") || statement.StartsWith($"input("))
                {
                    ConsoleInput.Interperate(statement, statements, textArray, FileLocation, null);
                }
                else if (statement.StartsWith($"Console.clear(") || statement.StartsWith($"clear("))
                {
                    ConsoleClear.Interperate(statement, textArray, FileLocation);
                }
                else if (statement.StartsWith($"Console.exec(") || statement.StartsWith($"exec("))
                {
                    ConsoleExec.Interperate(statement, textArray, FileLocation);
                }
                else if (statement.StartsWith($"Console.beep(") || statement.StartsWith($"beep("))
                {
                    ConsoleBeep.Interperate(statement, textArray, FileLocation);
                }
                else if (statement.StartsWith($"wait(") && statement.EndsWith(");"))
                {
                    TimeWait.Interperate(statement, lineCount);
                }
                else if (statement.StartsWith($"var") && statement.EndsWith(";"))
                {
                    VariableCode.Interperate(statement, statements, lineCount);
                }
                else if (statement.StartsWith($"Random.RandomRange(") || statement.StartsWith($"RandomRange("))
                {
                    Random_RandomRange.Interperate(statement, textArray, FileLocation);
                }
                else if (statement.StartsWith($"Random.RandomRangeDouble(") || statement.StartsWith($"RandomRangeDouble("))
                {
                    Random_RandomRangeDouble.Interperate(statement, textArray, FileLocation);
                }
                else if (statement.StartsWith($"ToString(") && statement.EndsWith(");"))
                {
                    ConvertToString.Interperate(statement);
                }
                try
                {
                    if ((statement.StartsWith($"if") && statement.EndsWith("{")) || (statement.StartsWith("if") && statements[lineCount] == "{"))
                    {
                        If_Loop.Interperate(statement, statements, textArray, FileLocation, isInAMethod, methodName); return "";
                    }
                    else if ((statement.StartsWith($"while") && statement.EndsWith("{")) || (statement.StartsWith("while") && statements[lineCount] == "{"))
                    {
                        While_Loop.Interperate(statement, statements, textArray, FileLocation); return "";
                    }
                    else if ((statement.StartsWith("func ") && statement.EndsWith(") {")) || (statement.StartsWith("func ")))
                    {
                        Method_Code.Interperate(statement, textArray, statements, FileLocation, true); return "";
                    }
                }
                catch (IndexOutOfRangeException /*indexOutRangeEx*/)
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error("Invalid Syntax", $"Invalid Syntax at line {lineCount}\n{statement}\n");
                }
                if (statement.StartsWith($"FileSystem.MakeFile(") || statement.StartsWith($"MakeFile("))
                {
                    FileSystem_MakeFile.Interperate(statement, FileLocation, textArray, lineCount);
                }
                else if (statement.StartsWith($"FileSystem.MakeFolder(") || statement.StartsWith($"MakeFolder("))
                {
                    FileSystem_MakeFolder.Interperate(statement, FileLocation, textArray, lineCount);
                }
                else if (statement.StartsWith($"FileSystem.DeleteFile(") || statement.StartsWith($"DeleteFile("))
                {
                    FileSystem_DeleteFile.Interperate(statement, FileLocation, textArray, lineCount);
                }
                else if (statement.StartsWith($"FileSystem.DeleteFolder(") || statement.StartsWith($"DeleteFolder("))
                {
                    FileSystem_DeleteFolder.Interperate(statement, FileLocation, textArray, lineCount);
                }
                else if (statement.StartsWith($"FileSystem.ReadFile(") || statement.StartsWith($"ReadFile("))
                {
                    FileSystem_ReadFile.Interperate(statement, FileLocation, textArray, lineCount);
                }
                else if (statement.StartsWith($"FileSystem.RenameFile(") || statement.StartsWith($"RenameFile("))
                {
                    FileSystem_RenameFile.Interperate(statement, FileLocation, textArray, lineCount);
                }
                else if (statement.StartsWith($"FileSystem.WriteFile(") || statement.StartsWith($"WriteFile("))
                {
                    FileSystem_WriteFile.Interperate(statement, FileLocation, textArray, lineCount);
                }
                else if (statement.StartsWith($"Network.Ping(") || statement.StartsWith($"Ping("))
                {
                    NetworkPing.Interperate(statement, FileLocation, textArray, lineCount);
                }
                else if (statement.StartsWith($"Time.CurrentTime(") || statement.StartsWith($"CurrentTime("))
                {
                    return Time_CurrentTime.Interperate(statement, textArray, FileLocation);
                }
                else if (statement.StartsWith($"Time.IsLeapYear(") || statement.StartsWith($"IsLeapYear("))
                {
                    return Convert.ToBoolean(Time_IsLeapYear.Interperate(statement, textArray, FileLocation));
                }
                else if (statement.StartsWith($"Random.RandomRange(") || statement.StartsWith($"RandomRange("))
                {
                    return Random_RandomRange.Interperate(statement, textArray, FileLocation);
                }
                else if (statement.StartsWith($"sdl2.makeWindow(") || statement.StartsWith($"makeWindow("))
                {
                    string code_line = statement.Replace("sdl2.", "").Replace("makeWindow(", "");
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
                else if (statement.StartsWith($"sdl2.createShape(") || statement.StartsWith($"createShape("))
                {
                    string code_line = statement.Replace("sdl2.", "").Replace("createShape(", "");
                    code_line = code_line.Substring(0, code_line.Length - 2);
                    string[] values = code_line.Split(",");
                    long window = 0;
                    int XPosition = 0;
                    int YPosition = 0;
                    int width = 0;
                    int height = 0;

                    try
                    {
                        window = Convert.ToInt64(values[0]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Error("Failed to get Window parameter");
                        return "";
                    }

                    try
                    {
                        XPosition = Convert.ToInt32(values[1]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Failed to get x (right) position parameter");
                        XPosition = int.MaxValue;
                    }

                    try
                    {
                        YPosition = Convert.ToInt32(values[2]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Failed to get y (up) position parameter");
                        YPosition = int.MaxValue;
                    }

                    try
                    {
                        width = Convert.ToInt32(values[3]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Failed to get w (width) size parameter");
                        width = int.MaxValue;
                    }

                    try
                    {
                        height = Convert.ToInt32(values[4]);
                    }
                    catch
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Warning("Failed to get h (height) size parameter");
                        height = int.MaxValue;
                    }

                    new Task(() => { SDL2_createShape.Interperate(window, XPosition, YPosition, width, height); }).Start();
                }
                else if (statement.StartsWith($"sdl2.clearScreen(") || statement.StartsWith($"clearScreen("))
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
                else
                {
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
                    {
                        if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP").Length > -1)
                        {
                            if (statement.EndsWith("();")) //Means its probably a function
                            {
                                Method_Code.Interperate(statement, textArray, statements, FileLocation, false);
                                return "";
                            }
                            foreach (string file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
                            {
                                string supposedVar = file[file.LastIndexOf("\\")..].Substring(1);

                                if (statement.StartsWith(supposedVar))
                                {
                                    if (statement.Contains('=') && (statement.IndexOf("+") + 1) != statement.IndexOf("=") && statement.Count(f => (f == '=')) == 1)
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1) + " = ";
                                        string content = statement.Replace(partToReplace, "");

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
                                    if (statement.Contains("+=") && statement.Count(f => (f == '=')) == 1 && statement.Count(f => (f == '+')) == 1 && statement.IndexOf("=") == (statement.IndexOf("+") + 1))
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1) + " = ";
                                        string[] FileContents = File.ReadAllLines(filePath);
                                        string content = statement.Replace(partToReplace, "");
                                        content = content[1..][..(content.Length - 2)];
                                        content = content[5..];
                                        content = content[..^1];

                                        List<string> FileContents_list = new List<string>(FileContents);
                                        FileContents_list.Add(content);
                                        File.WriteAllText(filePath, string.Join(Environment.NewLine, FileContents_list.ToArray()));
                                        break;
                                    }
                                    /* Removing a line from a var. */
                                    if (statement.Contains("-=") && statement.Count(f => (f == '-')) == 1 && statement.Count(f => (f == '-')) == 1 && statement.IndexOf("-") == (statement.IndexOf("-") + 1))
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1) + " = ";
                                        string[] FileContents = File.ReadAllLines(filePath);
                                        string content = statement.Replace(partToReplace, "");
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
                                CSharpErrorReporter.ConsoleLineReporter.Error(statement + " is not a valid line.");
                                return "";
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
                                        return CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                                    }
                                    catch (Exception e)
                                    {
                                        CSharpErrorReporter.ConsoleLineReporter.Error("The function you are trying to use returned an Error");
                                        Console.WriteLine($"\n{e.Message}");
                                    }
                                    return "";
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
                                    return CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                                }
                                catch (Exception e)
                                {
                                    CSharpErrorReporter.ConsoleLineReporter.Error("C# EXCEPTION ERROR; " + e.GetType().Name, e.Message);
                                    Console.WriteLine("CSHARP_Error");
                                }
                                return "";
                            }
                            else
                            {
                                CSharpErrorReporter.ConsoleLineReporter.Error($"\'{statement}\' is not a vaild code statement\n  Error was located on Line {lineCount - 13}");
                                break;
                            }
                        }
                        else
                        {
                            CSharpErrorReporter.ConsoleLineReporter.Error($"\'{statement}\' is not a vaild code statement\n  Error was located on Line {lineCount - 13}");
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
using Easy14_Programming_Language.Application_Code;
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
        // Configuration flags
        public static bool showStatementsDuringRuntime = false;
        public static bool DisplayFileContentsBeforeRuntime = false;

        // Paths and file-related variables
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string execAsmParentParentPath = Path.GetDirectoryName(Directory.GetParent(Directory.GetParent(executingAssemblyPath).FullName).FullName);
        private static readonly string optionsPath = "Application Code\\options.ini";
        private static readonly string[] configFile = File.ReadAllLines(Path.Combine(executingAssemblyPath, optionsPath));
        private static readonly string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        private static readonly string workingDirectoryPath = Path.GetDirectoryName(strExeFilePath);
        private static readonly string version = "Application Code\\currentVersion.txt";

        static void Main(string[] args)
        {
            Console.WriteLine(string.Join(" ", args));
            string osName = $"{RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}";

            Console.WriteLine($"Easy14 {File.ReadAllLines(version)[1]} ({osName})");
            Console.WriteLine("Type in \"help\" or \"info\" for information");

            //to get rid of older ways of variable management
            if (Directory.Exists(Path.Combine(desktopPath, "EASY14_Variables_TEMP"))) Directory.Delete(Path.Combine(desktopPath, "EASY14_Variables_TEMP"), true);

            if (!Convert.ToBoolean(Configuration.GetBoolOption("UpdatesDisabled"))) updateChecker.checkLatestVersion();

            foreach (string line in configFile)
            {
                if (line.StartsWith("delay"))
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(line.Replace("delay = ", "").Trim()) * 1000);
                        break;
                    }
                    catch (FormatException formatException)
                    {
                        ThrowErrorMessage.sendErrMessage($"{formatException.InnerException}; unable to set delay", null, "warning");
                    }
                }
            }

            if (args.Length != 0)
            {
                if (args[0] == "/help") HelpCommandCode.DisplayDefaultHelpOptions();
                else if (args[0].ToLower() == "/intro") IntroductionCode.IntroCode();
                else if (args[0].ToLower() == "/appinfo") AppInformation.ShowInfo();
                else if (File.Exists(args[0]) == true) CompileCode(new string[] { args[0] });
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
                string line = "";
                line = Console.ReadLine();
                if (line == "") continue;
                else if (line == "exit();") return;
                else if (line == "exit")
                {
                    ErrorReportor.ConsoleLineReporter.Warning("\nPlease use \"exit();\" or Ctrl+C to close the interative console"); continue;
                }
                else if (line.StartsWith("/run"))
                {
                    Program compiler = new Program();
                    compiler.ExternalComplieCode(line.TrimStart().Substring(4));
                }
                else if (line == "/help") HelpCommandCode.DisplayDefaultHelpOptions();
                else if (line == "/intro") IntroductionCode.IntroCode();
                else if (line == "/appinfo") AppInformation.ShowInfo();
                else
                {
                    if (!line.StartsWith("/"))
                    {
                        Program prog = new Program();
                        prog.ExternalComplieCode(null, new string[] { line }, 0);
                    }
                }
            }
        }

        public object ExternalComplieCode(string fileLoc = null, string[] textArray = null, int lineIDX = 0)
        {
            if (textArray == null)
            {
                if (fileLoc != null) textArray = File.ReadAllLines(fileLoc.Trim());
                else textArray = new string[] { "" };
            }

            return CompileCode(textArray, lineIDX);
        }

        public static object CompileCode(string[] textArray = null, int lineIDX = 0)
        {
            bool librariesDisabled = false;
            int windowHeight = Console.WindowHeight;
            int windowWidth = Console.WindowWidth;
            string windowState = "normal";

            librariesDisabled = Convert.ToBoolean(Configuration.GetBoolOption("disableLibraries"));

            windowHeight = (int)Configuration.GetBoolOption("windowHeight", true);
            windowWidth = (int)Configuration.GetBoolOption("windowWidth", true);
            windowState = (string)Configuration.GetBoolOption("windowState");
            if ((string)Configuration.GetBoolOption("showOptionsINI_DataWhenE14_Loads") == "true")
            {
                List<string> configFileLIST = new List<string>();

                foreach (string currentLine in configFile)
                {
                    if (currentLine.StartsWith(";") || currentLine == "" || currentLine == " ") continue;
                    configFileLIST.Add(currentLine);
                }

                string[] configFile_modified = configFileLIST.ToArray();

                Console.WriteLine(string.Join(Environment.NewLine, configFile_modified));
                Console.WriteLine("\n========================\n\n");
            }

            /* Reading the file and storing it in a string array. */
            int lineCount = 0;
            string[] codeLines = null;

            /* Removing the first lineIDX lines from the list. */
            List<string> linesList = new List<string>(codeLines != null ? codeLines : new string[] { "" });
            if (lineIDX != 0) linesList.RemoveRange(0, lineIDX);

            /* Removing the leading whitespace from each line in the list. */
            List<string> linesListMod = new List<string>();

            if (codeLines != null) codeLines = formatUserCode.format(codeLines);
            if (textArray != null) textArray = formatUserCode.format(textArray);

            if (codeLines == null)
            {
                codeLines = new string[] { "" };
            }

            foreach (string currentLine in textArray)
            {
                if (currentLine.Trim() == " ")
                { continue; }
                var StatementResult = CommandParser.SplitCommand(currentLine);
                string[] statementSplitSpace = currentLine.Split(" ");
                string[] statementSplitDot = currentLine.Split(".");
                List<string> statementListSplit = new List<string>(currentLine.Split(" "));

                if (showStatementsDuringRuntime == true) Console.WriteLine($">>>{currentLine}");

                if (currentLine.StartsWith($"using") && currentLine.EndsWith($";"))
                {
                    UsingNamspaceFunction.UsingMethod(currentLine, librariesDisabled, lineCount);
                }
                else if (currentLine.StartsWith($"from") && currentLine.EndsWith($";"))
                {
                    UsingNamspaceFunction.FromMethod(currentLine, librariesDisabled, lineCount);
                }
                else if (currentLine.Contains("+") || currentLine.Contains("-") || currentLine.Contains("*") || currentLine.Contains("/") || currentLine.Contains("%"))
                {
                    string expression = string.Join("", currentLine.ToCharArray());
                    try { Console.WriteLine(Convert.ToDouble(new DataTable().Compute(expression, null))); }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.GetType());
                        if (e.GetType().ToString() == "System.OverflowException") ErrorReportor.ConsoleLineReporter.Error("The Number to calculate is too large! (For Int32), please try a number less than 2147483647");
                        else ErrorReportor.ConsoleLineReporter.Error("Uh oh, the value you wanted to calculate won't work! (check if the value has a string value and change it to an integer)");
                    }
                }

                /* Checking if the user has entered "exit()" or "exit();" and if they have, it will
                exit the program. */
                else if (currentLine.ToLower() == "exit()" || currentLine.ToLower() == "exit();") return "";
                else if (currentLine.ToLower() == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPlease use \"exit()\" or Ctrl+C to close the interative console");
                    Console.ResetColor(); continue;
                }
                else if (StatementResult.className[0] == "Console")
                {
                    if (StatementResult.methodName == "Print")
                    { ConsolePrint.Interperate(StatementResult.paramItems[0]); }
                    else if (StatementResult.methodName == "Input")
                    { ConsoleInput.Interperate(StatementResult.paramItems[0]); }
                    else if (StatementResult.methodName == "Clear")
                    { ConsoleClear.Interperate(); }
                    else if (StatementResult.methodName == "Exec")
                    { ConsoleExec.Interperate(StatementResult.paramItems[0]); }
                    else if (StatementResult.methodName == "Beep")
                    { ConsoleBeep.Interperate(StatementResult.paramItems[0]); }
                }
                else if (StatementResult.className[0] == "Time")
                {
                    if (StatementResult.methodName == "Wait") TimeWait.Interperate(StatementResult.paramItems[0]);
                }
                else if (StatementResult.className[0] == "Var")
                {
                    if (StatementResult.methodName == "New")
                    {
                        if (StatementResult.paramItems.Count > 1)
                        {
                            VariableCode.Interperate(StatementResult.paramItems[0], StatementResult.paramItems[1], true);
                        }
                        else
                        {
                            return VariableCode.Interperate(StatementResult.paramItems[0], setVariable: true);
                        }
                    }
                    if (StatementResult.methodName == "Get")
                    {
                        VariableCode.Interperate(StatementResult.paramItems[0], setVariable: false);
                    }
                }
                else if (StatementResult.className[0] == "Random")
                {
                    if (StatementResult.methodName == "Range")
                    {
                        try
                        {
                            return Random_RandomRange.Interperate(Convert.ToInt32(StatementResult.paramItems[0]), Convert.ToInt32(StatementResult.paramItems[1]));
                        }
                        catch
                        {
                            throw new Exception("Not valid Integers");
                        }
                    }
                    else if (StatementResult.methodName == "RangeDouble")
                    { Random_RandomRangeDouble.Interperate(currentLine, textArray); }
                }
                else if (StatementResult.methodName == "ToString")
                {
                    ConvertToString.Interperate(currentLine);
                }
                else if (StatementResult.className[0] == "FileSystem")
                {
                    if (StatementResult.methodName == "MakeFile")
                        FileSystem_MakeFile.Interperate(lineCount, codeLines);
                    else if (StatementResult.methodName == "MakeFolder(")
                        FileSystem_MakeFolder.Interperate(currentLine, textArray);
                    else if (StatementResult.methodName == "DeleteFile")
                        FileSystem_DeleteFile.Interperate(currentLine, textArray);
                    else if (StatementResult.methodName == "DeleteFolder")
                        FileSystem_DeleteFolder.Interperate(currentLine, textArray);
                    else if (StatementResult.methodName == "ReadFile")
                        FileSystem_ReadFile.Interperate(currentLine, textArray);
                    else if (StatementResult.methodName == "RenameFile")
                        FileSystem_RenameFile.Interperate(currentLine, textArray);
                    else if (StatementResult.methodName == "WriteFile")
                        FileSystem_WriteFile.Interperate(currentLine, textArray);
                }
                else if (statementSplitDot[0] == "Network")
                {
                    if (StatementResult.methodName == "Ping")
                        NetworkPing.Interperate(currentLine, textArray);
                }
                else if (statementSplitDot[0] == "Time")
                {
                    if (StatementResult.methodName == "CurrentTime")
                        return Time_CurrentTime.Interperate(currentLine, textArray);
                    else if (StatementResult.methodName == "IsLeapYear")
                        return Convert.ToBoolean(Time_IsLeapYear.Interperate(currentLine, textArray));
                }
                else if (statementSplitDot[0] == "sdl2")
                {
                    if (StatementResult.methodName == "makeWindow")
                    {
                        string[] values = currentLine.Replace("sdl2.", "").Replace("makeWindow(", "").TrimEnd(')').Split(",");
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
                    if (StatementResult.methodName == "createShape")
                    {
                        string[] values = currentLine.Replace("sdl2.", "").Replace("createShape(", "").TrimEnd(')').Split(",");
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
                    if (StatementResult.methodName == "clearScreen")
                    {
                        string code_line = currentLine.Replace("sdl2.", "").Replace("clearScreen(", "");
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
                    string e14VerTemp_directoryPath = Path.Combine(desktopPath, "EASY14_Variables_TEMP");

                    if (Directory.Exists(e14VerTemp_directoryPath))
                    {
                        if (Directory.GetFiles(e14VerTemp_directoryPath).Length > -1)
                        {
                            if (currentLine.EndsWith("();"))
                            {
                                // Check if the statement ends with "();", indicating it's probably a function
                                Method_Code.Interperate(currentLine, textArray, codeLines, false);
                                return "";
                            }
                            foreach (string file in Directory.GetFiles($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\EASY14_Variables_TEMP"))
                            {
                                string supposedVar = file[file.LastIndexOf("\\")..].Substring(1);

                                if (currentLine.StartsWith(supposedVar))
                                {
                                    if (currentLine.Contains('=') && currentLine.IndexOf("+") != currentLine.IndexOf("=") - 1 && currentLine.Count(f => f == '=') == 1)
                                    {
                                        string filePath = file;
                                        string partToReplace = $"{file.Substring(file.LastIndexOf("\\") + 1)} = ";
                                        string content = currentLine.Replace(partToReplace, "");

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
                                    if (currentLine.Contains("+=") && currentLine.Count(f => f == '=') == 1 && currentLine.Count(f => f == '+') == 1)
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1) + " = ";
                                        string content = currentLine.Replace(partToReplace, "");
                                        content = content.Substring(5, content.Length - 6);

                                        string[] fileContents = File.ReadAllLines(filePath);
                                        List<string> fileContentsList = new List<string>(fileContents);
                                        fileContentsList.Add(content);
                                        File.WriteAllText(filePath, string.Join(Environment.NewLine, fileContentsList));
                                        break;
                                    }

                                    /* Removing a line from a var. */
                                    if (currentLine.Contains("-=") && currentLine.Count(f => f == '-') == 2)
                                    {
                                        string filePath = file;
                                        string partToReplace = file.Substring(file.LastIndexOf("\\") + 1) + " = ";
                                        string content = currentLine.Replace(partToReplace, "");
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

                    if (IsExecutableCode(currentLine))
                    {
                        string funcLine = null;
                        if (currentLine.Contains("("))
                        {
                            try
                            {
                                funcLine = currentLine.Substring(0, currentLine.IndexOf("("));
                            }
                            catch
                            {
                                HandleError($"{currentLine} is not a valid line.");
                                return "";
                            }
                        }

                        if (funcLine != null)
                        {
                            string[] allNamespacesAvailableArray = null;
                            bool hasNamespace = funcLine.Contains(".");

                            if (hasNamespace)
                            {
                                allNamespacesAvailableArray = GetAvailableNamespacesFromDirectory("Functions");
                            }
                            else
                            {
                                allNamespacesAvailableArray = GetAvailableNamespacesFromDirectory(workingDirectoryPath.Replace("\\bin\\Debug\\net7.0-windows", "").Replace("\\bin\\Release\\net7.0-windows", "") + "\\Functions");
                            }

                            if (allNamespacesAvailableArray.Contains(funcLine))
                            {
                                if (hasNamespace)
                                {
                                    ExecuteFunctionWithNamespace(currentLine);
                                }
                                else
                                {
                                    ExecuteFunctionWithoutNamespace(currentLine);
                                    return "";
                                }
                            }
                            else
                            {
                                HandleError($"\'{currentLine}\' is not a valid code statement\n  Error was located on Line {lineCount - 13}");
                                break;
                            }
                        }
                        else
                        {
                            HandleError($"\'{currentLine}\' is not a valid code statement\n  Error was located on Line {lineCount - 13}");
                            break;
                        }
                    }
                }
            }
            return "";
        }

        private static bool IsExecutableCode(string currentLine)
        {
            return currentLine != "}" &&
                   currentLine != "break" &&
                   currentLine != "return" &&
                   !currentLine.StartsWith("using") &&
                   !currentLine.StartsWith("var") &&
                   !currentLine.StartsWith("//");
        }
        private static void HandleError(string errorMessage)
        {
            ErrorReportor.ConsoleLineReporter.Error(errorMessage);
        }

        private static string[] GetAvailableNamespacesFromDirectory(string directoryName)
        {
            string functionsDirectory = Path.Combine(Directory.GetCurrentDirectory(), directoryName);
            return Directory.GetDirectories(functionsDirectory)
                            .Select(item => item[(item.LastIndexOf("\\") + 1)..])
                            .ToArray();
        }

        private static void ExecuteFunctionWithNamespace(string currentLine)
        {
            string theNamespaceOfTheLine = currentLine.Split(".")[0];
            string theFunctionOfTheLine = currentLine.Split(".")[1];
            string params_str = currentLine.Replace($"{theNamespaceOfTheLine}.{theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("))}(", "");
            params_str = params_str.Substring(0, params_str.Length - 2);
            string[] params_ = params_str.Split(",");

            string theFunctionOfTheLine_params = theFunctionOfTheLine;
            theFunctionOfTheLine = theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("));

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
                ErrorReportor.ConsoleLineReporter.Error("The function you are trying to use returned an Error");
                Console.WriteLine($"\n{e.Message}");
            }
        }

        private static async Task<string> ExecuteFunctionWithoutNamespace(string currentLine)
        {
            string[] allNamespacesAvaiable_array = Directory.GetDirectories(workingDirectoryPath.Replace("\\bin\\Debug\\net7.0-windows", "").Replace("\\bin\\Release\\net7.0-windows", "") + "\\Functions");
            List<string> allNamespacesAvaiable_list = new List<string>(allNamespacesAvaiable_array);
            List<string> allNamespacesAvaiable_list_main = new List<string>();
            foreach (string item in allNamespacesAvaiable_list)
            {
                allNamespacesAvaiable_list_main.Add(item[(item.LastIndexOf("\\") + 1)..]);
            }

            allNamespacesAvaiable_array = allNamespacesAvaiable_list_main.ToArray();
            string theFunctionOfTheLine = currentLine;
            int index = Array.IndexOf(allNamespacesAvaiable_array, theFunctionOfTheLine);
            string params_str = currentLine.Replace($"{theFunctionOfTheLine}.{theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("))}(", "");

            params_str = params_str.Substring(1, params_str.Length - 2);
            params_str = params_str.Substring(params_str.IndexOf("("), params_str.Length - params_str.IndexOf("("));
            params_str = params_str.Substring(1, params_str.Length - 1);
            string[] params_ = params_str.Split(",");
            theFunctionOfTheLine = theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("));
            string[] code = { $"Easy14_Programming_Language.{theFunctionOfTheLine}.Interperate({string.Join(",", params_)});" };

            try
            {
                var scriptState = await CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                return scriptState.ReturnValue?.ToString() ?? ""; // Convert the return value to string or return an empty string if null
            }
            catch (Exception e)
            {
                ErrorReportor.ConsoleLineReporter.Error("C# EXCEPTION ERROR; " + e.GetType().Name, e.Message);
                Console.WriteLine("CSHARP_Error");
            }
            return "";
        }

    }
}
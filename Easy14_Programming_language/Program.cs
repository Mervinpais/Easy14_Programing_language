﻿using Easy14_Programming_Language.Application_Code;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SDL2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easy14_Programming_Language
{
    public class Program
    {
        // Configuration flags
        public static bool showStatementsDuringRuntime = false;
        public static bool DisplayFileContentsBeforeRuntime = false;

        // Paths and file-related variables
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string optionsPath = Path.Combine(executingAssemblyPath, "Application Code", "options.ini");
        private static readonly string[] configFile = File.ReadAllLines(Path.Combine(executingAssemblyPath, optionsPath));
        private static readonly string version = Path.Combine(executingAssemblyPath, "Application Code", "currentVersion.txt");

        static void Main(string[] args)
        {
            Console.WriteLine(string.Join(" ", args));
            string osName = $"{RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}";

            try { Console.WriteLine($"Easy14 {File.ReadAllLines(version)[1]} ({osName})"); }
            catch { Console.WriteLine($"Easy14 ({osName})"); }

            if (!Convert.ToBoolean(Configuration.GetBoolOption("UpdatesDisabled"))) UpdateChecker.CheckLatestVersion();

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
                if (args[0].ToLower() == "/intro") IntroductionCode.IntroCode();
                else if (File.Exists(args[0]) == true)
                {
                    CompileCode(File.ReadAllLines(args[0]));
                    return;
                }
            }

            Console.WriteLine("\n===== Easy14 =====\n");

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
                    compiler.ExternalComplieCode(line.Trim().Substring(4));
                }
                else if (line == "/intro") IntroductionCode.IntroCode();
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
            int windowHeight = 0;
            int windowWidth = 0;
            string windowState = "normal";

            try
            {
                windowHeight = Console.WindowHeight;
                windowWidth = Console.WindowWidth;

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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("THIS IS NOT AN ERROR, Just that Easy14 config couldnt be set, so using defaults\n\n========");
            }


            /* Reading the file and storing it in a string array. */
            int lineCount = 0;
            string[] codeLines = null;

            /* Removing the first lineIDX lines from the list. */
            List<string> linesList = new List<string>(codeLines != null ? codeLines : new string[] { "" });
            if (lineIDX != 0) linesList.RemoveRange(0, lineIDX);

            /* Removing the leading whitespace from each line in the list. */
            List<string> linesListMod = new List<string>();

            if (codeLines == null)
            {
                codeLines = new string[] { "" };
            }

            for (int i = 0; i < textArray.Length; i++)
            {
                string currentLine = textArray[i];
                if (currentLine.Trim() == "")
                { continue; }

                var StatementResult = CommandParser.SplitCommand(currentLine);

                if (showStatementsDuringRuntime == true) Console.WriteLine($">>>{currentLine}");

                if (double.TryParse(currentLine.ToCharArray(), out _) == true)
                {
                    try { return Convert.ToDouble(new DataTable().Compute(currentLine, null)); }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
                else if (StatementResult.methodName.ToLower() == "exit()" || StatementResult.methodName.ToLower() == "exit();") return "";
                else if (StatementResult.methodName.ToLower() == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPlease use \"exit()\" or Ctrl+C to close the interative console");
                    Console.ResetColor(); continue;
                }
                else if (currentLine.StartsWith("if"))
                {
                    textArray = IfLoop.Interperate(i, textArray.ToList());
                    i = 0;
                    continue;
                }
                else if (currentLine.StartsWith("while"))
                {
                    textArray = WhileLoop.Interperate(i, textArray.ToList());
                    i = 0;
                    continue;
                }
                else if (currentLine.StartsWith("for"))
                {
                    textArray = ForLoop.Interperate(i, textArray.ToList());
                    i = 0;
                    continue;
                }
                /*else if (StatementResult.className[0] == "Console")
                {
                    if (StatementResult.methodName == "Print")
                    { ConsolePrint.Interperate(StatementResult.paramItems[0]); }
                    else if (StatementResult.methodName == "Input")
                    { return ConsoleInput.Interperate(StatementResult.paramItems[0]); }
                    else if (StatementResult.methodName == "Clear")
                    { ConsoleClear.Interperate(); }
                    else if (StatementResult.methodName == "Exec")
                    { ConsoleExec.Interperate(StatementResult.paramItems[0]); }
                    else if (StatementResult.methodName == "Beep")
                    { ConsoleBeep.Interperate(StatementResult.paramItems[0]); }
                    else if (StatementResult.methodName == "GetKeyPress")
                    { return ConsoleKeyPress.Interperate(StatementResult.paramItems[0]); }
                }*/
                else if (StatementResult.className[0] == "Time")
                {
                    if (StatementResult.methodName == "Wait") TimeWait.Interperate(StatementResult.paramItems[0]);
                }
                else if (StatementResult.className[0] == "Var")
                {
                    if (StatementResult.methodName == "New")
                    {
                        if (StatementResult.paramItems.Count > 1) //does variable include a value to be set?
                        {
                            VariableCode.Interperate(StatementResult.paramItems[0], StatementResult.paramItems[1], true);
                        }
                        else
                        {
                            VariableCode.Interperate(StatementResult.paramItems[0], setVariable: true);
                        }
                    }
                    if (StatementResult.methodName == "Get")
                    {
                        return VariableCode.Interperate(StatementResult.paramItems[0], setVariable: false);
                    }
                }
                else if (StatementResult.className[0] == "Random")
                {
                    if (StatementResult.methodName == "Range")
                    {
                        try
                        {
                            return Random_RandomRange.Interperate(
                                Convert.ToInt32(StatementResult.paramItems[0]),
                                Convert.ToInt32(StatementResult.paramItems[1])
                            );
                        }
                        catch { throw new Exception("Not valid Integers"); }
                    }
                    else if (StatementResult.methodName == "RangeDouble")
                    { return Random_RandomRangeDouble.Interperate(); }
                }
                /*
                else if (StatementResult.className[0] == "FileSystem")
                {
                    if (StatementResult.methodName == "MakeFile")
                        FileSystem_MakeFile.Interperate(StatementResult.paramItems[0]);
                    else if (StatementResult.methodName == "MoveFile")
                        FileSystem_MoveFile.Interperate(StatementResult.paramItems[0], StatementResult.paramItems[1]);
                    else if (StatementResult.methodName == "MakeFolder(")
                        FileSystem_MakeFolder.Interperate(StatementResult.paramItems[0]);
                    else if (StatementResult.methodName == "DeleteFile")
                        FileSystem_DeleteFile.Interperate(StatementResult.paramItems[0]);
                    else if (StatementResult.methodName == "DeleteFolder")
                        FileSystem_DeleteFolder.Interperate(StatementResult.paramItems[0]);
                    else if (StatementResult.methodName == "ReadFile")
                        FileSystem_ReadFile.Interperate(StatementResult.paramItems[0]);
                    else if (StatementResult.methodName == "RenameFile")
                        FileSystem_RenameFile.Interperate(StatementResult.paramItems[0], StatementResult.paramItems[1]);
                    else if (StatementResult.methodName == "WriteFile")
                        FileSystem_WriteFile.Interperate(StatementResult.paramItems[0], StatementResult.paramItems[1]);
                }*/
                /*else if (StatementResult.className[0] == "Network")
                {
                    if (StatementResult.methodName == "Ping")
                        NetworkPing.Interpret(StatementResult.paramItems[0]);
                    else if (StatementResult.methodName == "WebSocket")
                    {
                        _ = WebSocketActions.Interpret(StatementResult.paramItems[0], StatementResult.paramItems[1]);
                    }
                }*/
                /*
                else if (StatementResult.className[0] == "Time")
                {
                    if (StatementResult.methodName == "CurrentTime")
                        return Time_CurrentTime.Interperate();
                    else if (StatementResult.methodName == "IsLeapYear")
                        return Time_IsLeapYear.Interperate(StatementResult.paramItems[0]);
                }*/
                /*
                else if (StatementResult.className[0] == "Audio")
                {
                    if (StatementResult.methodName == "Play")
                    {
                        AudioPlay.Interperate(StatementResult.paramItems[0]);
                    }
                    else if (StatementResult.methodName == "Stop")
                    {
                        AudioStop.Interperate(Convert.ToInt32(StatementResult.paramItems[0]));
                    }
                }*/
                else if (StatementResult.className[0] == "SDL2")
                {
                    if (StatementResult.methodName == "MakeWindow")
                    {
                        List<string> values = StatementResult.paramItems;

                        int sizeX = values.Count >= 1 && int.TryParse(values[0], out int parsedSizeX) ? parsedSizeX : 200;
                        int sizeY = values.Count >= 2 && int.TryParse(values[1], out int parsedSizeY) ? parsedSizeY : 200;
                        int posX = values.Count >= 3 && int.TryParse(values[2], out int parsedPosX) ? parsedPosX : SDL.SDL_WINDOWPOS_UNDEFINED;
                        int posY = values.Count >= 4 && int.TryParse(values[3], out int parsedPosY) ? parsedPosY : SDL.SDL_WINDOWPOS_UNDEFINED;
                        string title = values.Count >= 5 ? values[4] : "E14 SDL2 Window";

                        IntPtr window = IntPtr.Zero;
                        long window_int = -1;
                        new Task(() => { window_int = SDL2_makeWindow.Interperate(sizeX, sizeY, posX, posY, title); }).Start();
                        window = (IntPtr)window_int;
                        SDL.SDL_SetWindowTitle(window, values.Count >= 5 ? $"{values[4]} - {window_int}" : $"E14 SDL2 Window - {window_int}");
                        continue;
                    }

                    if (StatementResult.methodName == "CreateShape")
                    {
                        List<string> values = StatementResult.paramItems;
                        long window = 0;
                        int xPosition = int.MaxValue;
                        int yPosition = int.MaxValue;
                        int width = int.MaxValue;
                        int height = int.MaxValue;

                        if (long.TryParse(values[0], out window) && values.Count >= 2) int.TryParse(values[1], out xPosition);

                        if (values.Count >= 3) int.TryParse(values[2], out yPosition);
                        if (values.Count >= 4) int.TryParse(values[3], out width);
                        if (values.Count >= 5) int.TryParse(values[4], out height);

                        new Task(() => { SDL2_createShape.Interperate(window, xPosition, yPosition, width, height); }).Start();
                    }
                    if (StatementResult.methodName == "ClearScreen")
                    {
                        long window = 0;
                        string color = null;
                        List<string> values = StatementResult.paramItems;
                        window = Convert.ToInt64(values[0]);
                        color = values[1];
                        SDL2_clearScreen.Interperate(window, color);
                    }
                }
                else
                {
                    int currentLineLenght = currentLine.Length;
                    if (IsExecutableCode(currentLine))
                    {
                        try { return ExecuteFunctionWithNamespace(StatementResult); }
                        catch
                        {
                            HandleError($"\'{currentLine}\' is not a valid code statement\n  {' ',-7}^ \n Error was located on Line {lineCount}");
                            break;
                        }
                    }
                    else
                    {
                        HandleError($"\'{currentLine}\' is not a valid code statement\n  {' ',-7}^ \n Error was located on Line {lineCount}");
                        break;
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
                   !currentLine.StartsWith("//");
        }
        private static void HandleError(string errorMessage)
        {
            ErrorReportor.ConsoleLineReporter.Error(errorMessage);
        }

        public static object ExecuteFunctionWithNamespace((List<string> classes, string method, List<string> params_) StatementResult)
        {
            List<string> theClassesOfTheLine = StatementResult.classes;
            string theMethodOfTheLine = StatementResult.method;
            List<string> paramsGiven = StatementResult.params_;

            string classHierarchy = string.Join("/", theClassesOfTheLine);

            // Create the folder path for Easy14 packages within AppData Local
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 packages");
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            // Construct the path for the method's C# file
            string methodFolderPath = Path.Combine(appDataPath, classHierarchy);
            string codeFilePath = Path.Combine(methodFolderPath, $"{theMethodOfTheLine}.cs");

            if (File.Exists(codeFilePath))
            {
                string code = File.ReadAllText(codeFilePath);
                List<string> codeSplitIntoLines = File.ReadAllLines(codeFilePath).ToList();

                try
                {
                    // Create a wrapper class containing the dynamic method
                    if (codeSplitIntoLines[0].StartsWith("//_params = "))
                    {
                        string _paramsDeclareLine = codeSplitIntoLines[0];
                        List<string> paramsRequired = codeSplitIntoLines[0].Substring("//_params = ".Length).Split(",").ToList();
                        // Compare params_ and paramNames count
                        if (paramsGiven.Count > paramsRequired.Count)
                        {
                            // If params_ has more elements than paramNames, truncate the excess
                            paramsGiven = paramsGiven.Take(paramsRequired.Count).ToList();
                        }
                        else if (paramsGiven.Count < paramsRequired.Count)
                        {
                            // If params_ has fewer elements than paramNames, add in null values
                            //Console.WriteLine("Error: Insufficient parameters provided.");
                            //return null;
                            paramsGiven = paramsGiven.Take(paramsRequired.Count).ToList();
                            for (int i = 0; i < (paramsRequired.Count - paramsGiven.Count); i++)
                            {
                                paramsGiven.Add("\"\"");
                                StatementResult.params_.Add("\"\"");
                            }
                        }

                        List<string> usingReferences = new List<string>();
                        List<string> restOfCode = new List<string>();
                        codeSplitIntoLines.Remove(_paramsDeclareLine);

                        for (int i = 0; i < paramsRequired.Count; i++)
                        {
                            string dataType = "var";
                            string value = StatementResult.params_[i];
                            if (value != "")
                            {
                                if (ItemChecks.DetectType(StatementResult.params_[i]) == "str")
                                { dataType = "string"; value = "\"\\\"" + value.Substring(1, value.Length - 2) + "\\\"\""; } //this is an abomination but works
                                if (ItemChecks.DetectType(StatementResult.params_[i]) == "int") dataType = "int";
                                if (ItemChecks.DetectType(StatementResult.params_[i]) == "double") dataType = "double";
                                if (ItemChecks.DetectType(StatementResult.params_[i]) == "bool") dataType = "bool";
                                if (ItemChecks.DetectType(StatementResult.params_[i]) == "cmd")
                                { dataType = "string"; value = "\"" + value.Substring("() =>".Length).Trim().Replace("\"", "\\\"") + ";\""; }
                            }
                            else { dataType = "object"; value = "null"; }
                            codeSplitIntoLines.Insert(0, $"{dataType} {paramsRequired[i]} = {value};");
                        }
                        foreach (string line in codeSplitIntoLines)
                        {
                            if (line.StartsWith("using ")) usingReferences.Add(line);
                            else restOfCode.Add(line);
                        }
                        usingReferences.AddRange(restOfCode);
                        codeSplitIntoLines = usingReferences;
                        code = string.Join(Environment.NewLine, codeSplitIntoLines);
                    }

                    var references = new List<MetadataReference>
                    {
                        MetadataReference.CreateFromFile(typeof(DataTable).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Windows.Forms.Form).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Net.NetworkInformation.Ping).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Net.NetworkInformation.IPStatus).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Net.NetworkInformation.IPGlobalProperties).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Media.SoundPlayer).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Media.SystemSound).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Media.SystemSounds).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Drawing.Point).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Easy14_Programming_Language.Program).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Easy14_Programming_Language.ItemChecks).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Easy14_Programming_Language.VariableCode).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Easy14_Programming_Language.UniversalVariables).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                    };

                    ScriptOptions scriptOptions = ScriptOptions.Default
                        .WithReferences(references)
                        .WithImports("System", "System.IO", "System.Windows","System.Media", "System.Drawing", "System.Drawing.Point", "System.Windows.Forms", "System.Collections.Generic","System.Net", "System.Net.NetworkInformation", "Easy14_Programming_Language", "Easy14_Programming_Language.UniversalVariables");
                    
                    var script = CSharpScript.Create(code, options: scriptOptions);
                    var result = script.RunAsync().Result;

                    if (result.Exception != null)
                    {
                        Console.WriteLine("Error occurred: " + result.Exception);
                    }
                    else
                    {
                        var returnValue = result.ReturnValue;
                        return returnValue;
                    }
                }
                catch (Exception e)
                {
                    ErrorReportor.ConsoleLineReporter.Error("An Error Occurred while running the Easy14 Package (C# Error)");
                    Console.WriteLine($"\n{e.Message}");
                    throw new Exception("Not valid statement");
                }
            }
            else
            {
                Debug.WriteLine($"The method '{theMethodOfTheLine}' for class '{classHierarchy}' was not found.");
                throw new Exception("Not valid statement");
            }

            return null;
        }
    }
}
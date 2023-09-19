using Easy14_Programming_Language.Application_Code;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SDL2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

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

            try
            {
                Console.WriteLine($"Easy14 {File.ReadAllLines(version)[1]} ({osName})");
            }
            catch
            {
                Console.WriteLine($"Easy14 {{Unknown Version}} ({osName})");
            }

            if (!Configuration.GetBoolOptionValue("UpdatesDisabled")) { UpdateChecker.CheckLatestVersion(); }

            Thread.Sleep(Configuration.GetIntOptionValue("delay") * 1000);

            if (args.Length != 0)
            {
                if (args[0].ToLower() == "/intro")
                {
                    IntroductionCode.IntroCode();
                }
                else if (File.Exists(args[0]) == true)
                {
                    CompileCode(File.ReadAllLines(args[0]));
                    return;
                }
            }

            Console.WriteLine("\n===== Easy14 =====\n");
            try
            {
                int windowHeight = Console.WindowHeight;
                int windowWidth = Console.WindowWidth;

                bool librariesDisabled = Convert.ToBoolean(Configuration.GetBoolOptionValue("disableLibraries"));

                windowHeight = Configuration.GetIntOptionValue("windowHeight") == -1 ? Console.WindowHeight : Configuration.GetIntOptionValue("windowHeight");
                windowWidth = Configuration.GetIntOptionValue("windowWidth") == -1 ? Console.WindowWidth : Configuration.GetIntOptionValue("windowWidth");
                string windowState = Configuration.GetStringOptionValue("windowState");
                if (Configuration.GetBoolOptionValue("showOptionsINI_DataWhenE14_Loads") == true)
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
                ErrorReportor.ConsoleLineReporter.Message("THIS IS NOT AN ERROR, Just that Easy14 config couldnt be set, so using defaults\n\n========");
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

        public class Token
        {
            public string Value { get; set; }
            public string Tag { get; set; }

            public Token(string value, string tag)
            {
                Value = value;
                Tag = tag;
            }
        }

        public class Tokenizer
        {
            public List<Token> Tokenize(string input)
            {
                List<Token> tokens = new List<Token>();

                // Define token patterns

                var identifierPattern = @"[a-zA-Z_]\w*";
                var methodsPattern = @"(.*?\.)([A-Za-z]+)\((.*?)\);";
                var numberPattern = @"\d+";
                var operatorPattern = @"\+|-|\*|/"; // Add more operators as needed

                // Create a combined regular expression pattern
                var combinedPattern = string.Join("|", methodsPattern, identifierPattern, numberPattern, operatorPattern);

                // Tokenize the input
                // Tokenize the input
                var matches = Regex.Matches(input, combinedPattern);
                foreach (Match match in matches)
                {
                    string value = match.Value;

                    var methodMatch = Regex.Match(value, @"(.*?\.)([A-Za-z]+)\((.*?)\);");
                    if (methodMatch.Success)
                    {
                        string classPart = methodMatch.Groups[1].Value;
                        string methodPart = methodMatch.Groups[2].Value;
                        string paramsPart = methodMatch.Groups[3].Value;

                        // Split the parameters by commas while ignoring commas within parentheses
                        List<string> parameters = new List<string>();
                        int parenthesesCount = 0;
                        StringBuilder currentParameter = new StringBuilder();

                        foreach (char c in paramsPart)
                        {
                            if (c == '(')
                            {
                                parenthesesCount++;
                                currentParameter.Append(c);
                            }
                            else if (c == ')')
                            {
                                parenthesesCount--;
                                currentParameter.Append(c);
                            }
                            else if (c == ',' && parenthesesCount == 0)
                            {
                                // Found a comma outside of parentheses, add the current parameter
                                parameters.Add(currentParameter.ToString().Trim());
                                currentParameter.Clear();
                            }
                            else
                            {
                                currentParameter.Append(c);
                            }
                        }

                        // Add the last parameter
                        parameters.Add(currentParameter.ToString().Trim());

                        // You can add the extracted class, method, and parameters as tokens here
                        tokens.Add(new Token(classPart, "Class"));
                        tokens.Add(new Token(methodPart, "Method"));
                        foreach (string param in parameters)
                        {
                            tokens.Add(new Token(param, "Param"));
                        }
                    }
                    else
                    {
                        string tag = DetermineTag(value); // Implement a function to determine the tag based on the matched value
                        tokens.Add(new Token(value, tag));
                    }
                }

                return tokens;
            }

            private string DetermineTag(string value)
            {
                if (Regex.IsMatch(value, @"[a-zA-Z_]\w*"))
                {
                    return "Identifier";
                }
                else if (Regex.IsMatch(value, @"\d+"))
                {
                    return "Number";
                }
                else if (Regex.IsMatch(value, @"\+|-|\*|/"))
                {
                    return "Operator";
                }
                else
                {
                    return "Unknown"; // Handle unknown tokens as needed
                }
            }
        }


        public static object CompileCode(string[] textArray = null, int lineIDX = 0)
        {
            int lineCount = 0;
            string[] codeLines = null;

            List<string> linesList = new List<string>(codeLines != null ? codeLines : new string[] { "" });
            if (lineIDX != 0) linesList.RemoveRange(0, lineIDX);

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

                Tokenizer tokenizer = new Tokenizer();
                List<Token> tokens = tokenizer.Tokenize(currentLine);
                for (int index = 0; index < tokens.Count; index++)
                {

                    List<(List<string>, string, List<string>)> Statements = new List<(List<string>, string, List<string>)>();

                    if (tokens[index].Tag == "Class")
                    {
                        Statements.Add(new(tokens[index].Value.Split(".").ToList(), null, null));
                        index = index + 1;
                        if (tokens[index].Tag == "Method")
                        {
                            Statements.Add(new(Statements[0].Item1, tokens[index].Value, null));
                            Statements.RemoveAt(0);
                            index = index + 1;
                            if (tokens[index].Tag == "Params")
                            {
                                Statements.Add(new(Statements[0].Item1, Statements[0].Item2, (tokens[index].Value).Split("|").ToList()));
                                Statements.RemoveAt(0);
                                index = index + 1;
                                return ExecuteFunctionWithNamespace(new(Statements[0].Item1, Statements[0].Item2, Statements[0].Item3));
                            }
                        }
                    }
                    else if (tokens[index].Tag == "Number")
                    {
                        string expression = "";

                        while (index < tokens.Count && (tokens[index].Tag == "Number" || tokens[index].Tag == "Operator"))
                        {
                            expression += tokens[index].Value; // Use += to concatenate strings
                            index++; // Increment index
                        }

                        try
                        {
                            return Convert.ToDouble(new DataTable().Compute(expression, null));
                        }
                        catch (Exception e)
                        {
                            return e.Message;
                        }
                    }
                }

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
                else
                {
                    int currentLineLength = currentLine.Length;
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
                                try
                                {
                                    if (ItemChecks.DetectType(StatementResult.params_[i]) == "str")
                                    { dataType = "string"; value = "\"\\\"" + value.Substring(1, value.Length - 2) + "\\\"\""; } //this is an abomination but works
                                }
                                catch { }
                                try
                                {
                                    if (ItemChecks.DetectType(StatementResult.params_[i]) == "int") dataType = "int";
                                }
                                catch { }
                                try
                                {
                                    if (ItemChecks.DetectType(StatementResult.params_[i]) == "double") dataType = "double";
                                }
                                catch { }
                                try
                                {
                                    if (ItemChecks.DetectType(StatementResult.params_[i]) == "bool") dataType = "bool";
                                }
                                catch { }
                                try
                                {
                                    if (ItemChecks.DetectType(StatementResult.params_[i]) == "cmd")
                                    { dataType = "string"; value = "\"" + value.Substring("() =>".Length).Trim().Replace("\"", "\\\"") + ";\""; }
                                }
                                catch { }
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
                        MetadataReference.CreateFromFile(typeof(SDL).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Windows.Forms.Form).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Net.NetworkInformation.Ping).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Net.NetworkInformation.IPStatus).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Net.NetworkInformation.IPGlobalProperties).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Threading.Tasks.Task).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Media.SoundPlayer).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Media.SystemSound).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Media.SystemSounds).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Drawing.Point).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Program).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(ItemChecks).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(VariableCode).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(UniversalVariables).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                    };

                    ScriptOptions scriptOptions = ScriptOptions.Default
                        .WithReferences(references)
                        .WithImports("System", "SDL2", "System.IO", "System.Threading", "System.Threading.Tasks", "System.Windows", "System.Media", "System.Drawing", "System.Drawing.Point", "System.Windows.Forms", "System.Collections.Generic", "System.Net", "System.Net.NetworkInformation", "Easy14_Programming_Language", "Easy14_Programming_Language.UniversalVariables");

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
                    throw new Exception($"Not valid statement;\n{e.Message}");
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

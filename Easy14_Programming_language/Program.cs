using Easy14_Programming_Language.Application_Code;
using Easy14_Programming_Language.Functions;
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
        public static string PathOfPackages = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 Packages");

        // Paths and file-related variables
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string optionsPath = Path.Combine(executingAssemblyPath, "Application Code", "options.ini");
        private static readonly string[] configFile = File.ReadAllLines(Path.Combine(executingAssemblyPath, optionsPath));
        private static readonly string version = Path.Combine(executingAssemblyPath, "Application Code", "currentVersion.txt");

        public enum Status
        {
            CSHARP_ERROR,
            CODE_ERROR,
            NORMAL,
            UNKNOWN
        }

        public static Status ProgramStatus = Status.NORMAL;

        static void Main(string[] args)
        {
            Console.WriteLine(string.Join(" ", args));
            string osName = $"{RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}";

            try
            {
                Console.WriteLine($"Easy14 {File.ReadAllLines(version)[0]} ({osName})");
            }
            catch
            {
                Console.WriteLine($"Easy14 {{Unknown Version}} ({osName})");
            }

            if (!Configuration.GetBoolOptionValue("UpdatesDisabled")) { UpdateChecker.CheckLatestVersion(); }

            Thread.Sleep(Configuration.GetIntOptionValue("delay") * 1000);

            Console.WriteLine("\n===== Easy14 =====\n");


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

            try
            {
                int windowHeight = Console.WindowHeight;
                int windowWidth = Console.WindowWidth;

                bool librariesDisabled = Convert.ToBoolean(Configuration.GetBoolOptionValue("disableLibraries"));

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
                ErrorReportor.ReportWarning("Configuration Setting", "No error occurred; Easy14 is using default settings.\n\n========");
            }

            while (true)
            {
                Console.Write(":>");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                switch (input)
                {
                    case "help":
                        return;

                    default:
                        Program compiler = new Program();
                        compiler.ExternalCompileCode(null, new string[] { input }, 0);
                        break;
                }
            }

        }

        public object ExternalCompileCode(string fileLoc = null, string[] textArray = null, int lineIDX = 0)
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

                var identifierPattern = @"[a-zA-Z_]\w*";
                var methodsPattern = @"(.*?\.)([A-Za-z]+)\((.*?)\);";
                var numberPattern = @"\d+";
                var operatorPattern = @"\+|-|\*|/";

                var combinedPattern = string.Join("|", methodsPattern, identifierPattern, numberPattern, operatorPattern);

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
                                parameters.Add(currentParameter.ToString().Trim());
                                currentParameter.Clear();
                            }
                            else
                            {
                                currentParameter.Append(c);
                            }
                        }

                        parameters.Add(currentParameter.ToString().Trim());

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
            string[] codeLines = new string[] { "" };

            List<string> linesList = new List<string>(codeLines != null ? codeLines : new string[] { "" });
            if (lineIDX != 0) linesList.RemoveRange(0, lineIDX);

            object result = "";

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

                    List<(List<string>, string, List<string>)> Statements = new();

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
                    Console.WriteLine("\nPlease use \"exit()\" or Ctrl+C to close the interactive console");
                    Console.ResetColor(); continue;
                }
                else if (currentLine.Trim().StartsWith("//"))
                {
                    continue;
                }
                else if (currentLine.Trim().StartsWith("if"))
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
                    textArray = RepeatLoop.Interperate(i, textArray.ToList());
                    i = 0;
                    continue;
                }
                else if (currentLine.Trim().StartsWith("import"))
                {
                    List<string> mainCode = new List<string>();
                    mainCode.AddRange(ImportFileStatement.Interpret(currentLine));
                    mainCode.AddRange(textArray.ToList().GetRange(1, textArray.ToList().Count - 1));
                    textArray = mainCode.ToArray();
                    i = i - 1;
                    continue;
                }
                else if (currentLine.Trim().StartsWith("method"))
                {
                    int indention = 0;
                    foreach (char c in currentLine)
                    {
                        if (c == ' ')
                        {
                            indention = indention + 1;
                        }
                        else if (c == '\t')
                        {
                            indention = indention + 3;
                        }
                        else
                        {
                            break;
                        }
                    }

                    string indent = "";
                    for (int i_ = 0; i_ < indention; i_++)
                    {
                        indent += " ";
                    }

                    if (!currentLine.EndsWith("();"))
                    {
                        string methodName = currentLine.Substring(indent.Length - 1 + "method".Length); // Implement GetMethodName to extract the method name
                        if (methodName.Contains(" "))
                        {
                            methodName = methodName.Substring(methodName.IndexOf(" "));
                        }
                        List<string> methodCode = new List<string>();
                        int startIndex = i; // Remember the starting index

                        for (int j = i + 1; j < textArray.Length; j++)
                        {
                            string line = "";
                            foreach (char code in textArray[j])
                            {
                                if (code == '\t')
                                {
                                    line += "   ";
                                }
                                else
                                {
                                    line += code;
                                }
                            }

                            string endWord = (indent + "end");
                            if (line == endWord)
                            {
                                // Found the end of the method, add it to the methods dictionary
                                MethodLoop.DefineMethod(methodName.Trim(), methodCode);
                                i = j; // Update the current line index
                                break;
                            }

                            methodCode.Add(line);
                        }
                        i = i + 1;
                        continue;
                    }
                    else if (currentLine.EndsWith("();"))
                    {
                        string methodName = currentLine.Substring(indent.Length + "method".Length, currentLine.Length - (indent.Length + "method".Length + "();".Length)).Trim();
                        if (MethodLoop.MethodExists(methodName))
                        {
                            MethodLoop.ExecuteMethod(methodName);
                        }
                    }
                }
                else if (currentLine.Trim().StartsWith("var"))
                {
                    string variableName = "";
                    string variableContents = "";

                    try
                    {
                        variableName = currentLine.Trim().Split("=")[0];
                        variableName = variableName.Substring(3).Trim();
                        variableContents = string.Join("=", currentLine.Trim().Split("=")[1..]).Trim();
                        variableContents = variableContents.Substring(0, variableContents.Length - 1);
                    }
                    catch
                    {
                        variableName = currentLine.Substring(3).Trim();
                        variableName = variableName.Substring(0, variableName.Length - 1);
                    }

                    if (currentLine.Contains("="))
                    {
                        VariableCode.DefineVariable(variableName, variableContents);
                    }
                    else
                    {
                        if (VariableCode.VariableExists(variableName))
                        {
                            result = VariableCode.variables[variableName];
                        }
                        else
                        {
                            ErrorReportor.ReportError("", $"Variable {variableName} doesnt exist!");
                        }
                    }
                }
                else
                {
                    if (IsExecutableCode(currentLine))
                    {
                        try
                        {
                            result = ExecuteFunctionWithNamespace(StatementResult);
                        }
                        catch
                        {
                            ErrorReportor.ReportError("Code Not Valid!", $"\'{currentLine}\' is not a valid code statement\n  {' ',-7}^ \n Error was located on Line {lineCount}");
                            break;
                        }
                    }
                    else
                    {
                        ErrorReportor.ReportError("Code Not Valid!", $"\'{currentLine}\' is not a valid code statement\n  {' ',-7}^ \n Error was located on Line {lineCount}");
                        break;
                    }
                }
                if (ProgramStatus.HasFlag(Status.CODE_ERROR) || ProgramStatus.HasFlag(Status.CODE_ERROR))
                {
                    ProgramStatus = Status.NORMAL;
                    return "";
                }
            }
            return result;
        }

        private static bool IsExecutableCode(string currentLine)
        {
            return currentLine != "}" &&
                   currentLine != "break" &&
                   currentLine != "return" &&
                   !currentLine.StartsWith("//");
        }

        public static object ExecuteFunctionWithNamespace((List<string> classes, string method, List<string> params_) StatementResult)
        {
            List<string> theClassesOfTheLine = StatementResult.classes;
            string theMethodOfTheLine = StatementResult.method;
            List<string> paramsGiven = StatementResult.params_;

            string classHierarchy = string.Join("/", theClassesOfTheLine);

            string methodFolderPath = Path.Combine(PathOfPackages, classHierarchy);
            string codeFilePath = Path.Combine(methodFolderPath, $"{theMethodOfTheLine}.cs");

            if (File.Exists(codeFilePath))
            {
                string code = File.ReadAllText(codeFilePath);
                List<string> codeSplitIntoLines = File.ReadAllLines(codeFilePath).ToList();

                try
                {
                    if (codeSplitIntoLines[0].StartsWith("//_params = "))
                    {
                        string _paramsDeclareLine = codeSplitIntoLines[0];
                        List<string> paramsRequired = codeSplitIntoLines[0].Substring("//_params = ".Length).Split(",").ToList();
                        if (paramsGiven.Count > paramsRequired.Count)
                        {
                            paramsGiven = paramsGiven.Take(paramsRequired.Count).ToList();
                        }
                        else if (paramsGiven.Count < paramsRequired.Count)
                        {
                            paramsGiven = paramsGiven.Take(paramsRequired.Count).ToList();
                            for (int i = 0; i < (paramsRequired.Count - paramsGiven.Count); i++)
                            {
                                paramsGiven.Add("\"\"");
                                StatementResult.params_.Add("\"\"");
                            }
                        }

                        List<string> usingReferences = new();
                        List<string> restOfCode = new();
                        codeSplitIntoLines.Remove(_paramsDeclareLine);

                        for (int i = 0; i < paramsRequired.Count; i++)
                        {
                            string dataType = "var";
                            string value = StatementResult.params_[i];
                            if (value != "")
                            {
                                dataType = ItemChecks.DetectType(StatementResult.params_[i]);
                                if (dataType == "str")
                                {
                                    dataType = "string";
                                    value = "\"\\\"" + value.Substring(1, value.Length - 2) + "\\\"\"";
                                }
                                else if (dataType == "cmd")
                                {
                                    dataType = "string"; 
                                    value = "\"" + value.Substring("() =>".Length).Trim().Replace("\"", "\\\"") + ";\"";
                                }
                                /*
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
                                catch { }*/
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
                        MetadataReference.CreateFromFile(typeof(VariableCode).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(UniversalVariables).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                    };

                    ScriptOptions scriptOptions = ScriptOptions.Default
                        .WithReferences(references)
                        .WithImports("System", "SDL2", "System.IO", "System.Threading", "System.Threading.Tasks", "System.Windows", "System.Media", "System.Drawing", "System.Drawing.Point", "System.Windows.Forms", "System.Collections.Generic", "System.Net", "System.Net.NetworkInformation", "Easy14_Programming_Language", "Easy14_Programming_Language.UniversalVariables");

                    code = code + $"{Environment.NewLine}Environment.Exit(0);";
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
                    ErrorReportor.ReportCSharpError("Package Running Error", "An Error Occurred while running the Easy14 Package (C# Error)");
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

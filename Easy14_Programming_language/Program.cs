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
            string versionName = "{Unknown Version}";

            try { versionName = File.ReadAllLines(version)[0]; } catch { }

            Console.WriteLine($"Easy14 {versionName} ({osName})");

            if (!Configuration.GetBoolOptionValue("UpdatesDisabled"))
            {
                UpdateChecker.CheckLatestVersion();
            }

            if (!string.IsNullOrEmpty(Configuration.GetStringOptionValue("packagePath")))
            {
                PathOfPackages = Configuration.GetStringOptionValue("packagePath");
            }

            Thread.Sleep(Configuration.GetIntOptionValue("delay") * 1000);

            Console.WriteLine("\n===== Easy14 =====\n");


            if (args.Length != 0)
            {
                if (args[0].ToLower() == "/intro")
                {
                    IntroductionCode.IntroCode();
                }
                else if (File.Exists(string.Join(" ", args[0..])) == true)
                {
                    CompileCode(File.ReadAllLines(string.Join(" ", args[0..])));
                    return;
                }
            }

            try
            {
                int windowHeight = Console.WindowHeight;
                int windowWidth = Console.WindowWidth;

                bool librariesDisabled = Convert.ToBoolean(Configuration.GetBoolOptionValue("disableLibraries"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ErrorReportor.ReportWarning("Configuration Setting", "Easy14 is using default settings, as it cant find options.ini file, or some other error.\n\n========");
            }

            Program compiler = new Program();

            while (true)
            {
                Console.Write(":>");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) { continue; }

                switch (input)
                {
                    case "help":
                        return;

                    default:
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


        public enum TokenType
        {
            Class,
            Method,
            Params,
            Number,
            Operator,
            Identifier,
            Unknown
        }

        public class Token
        {
            public string Value { get; set; }
            public TokenType Tag { get; set; }

            public Token(string value, TokenType tag)
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

                        tokens.Add(new Token(classPart, TokenType.Class));
                        tokens.Add(new Token(methodPart, TokenType.Method));
                        foreach (string param in parameters)
                        {
                            tokens.Add(new Token(param, TokenType.Params));
                        }
                    }
                    else
                    {
                        TokenType tag = DetermineTag(value); // Implement a function to determine the tag based on the matched value
                        tokens.Add(new Token(value, tag));
                    }
                }

                return tokens;
            }

            private TokenType DetermineTag(string value)
            {
                if (Regex.IsMatch(value, @"[a-zA-Z_]\w*"))
                { return TokenType.Identifier; }

                else if (Regex.IsMatch(value, @"\d+"))
                { return TokenType.Number; }

                else if (Regex.IsMatch(value, @"\+|-|\*|/"))
                { return TokenType.Operator; }

                else { return TokenType.Unknown; }
            }
        }

        public static object CompileCode(string[] textArray = null, int lineIDX = 0)
        {
            int lineCount = 0;
            object result = "";

            for (int i = 0; i < textArray.Length; i++)
            {
                if (string.IsNullOrEmpty(textArray[i].Trim())) { continue; }

                //var StatementResult = CommandParser.SplitCommand(textArray[i]);

                if (double.TryParse(textArray[i].ToCharArray(), out _) == true)
                {
                    try { return Convert.ToDouble(new DataTable().Compute(textArray[i], null)); }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
                else if (textArray[i].Trim().StartsWith("//"))
                {
                    continue;
                }
                else {
                    Tokenizer tokenizer = new Tokenizer();
                    List<Token> tokens = tokenizer.Tokenize(textArray[i]);

                    for (int index = 0; index < tokens.Count; index++)
                    {
                        List<(List<string>, string, List<string>)> Statements = new();

                        if (tokens[index].Tag == TokenType.Class)
                        {
                            // Parsing a Class
                            var className = tokens[index].Value.Split(".").ToList();
                            Statements.Add((className, null, null));

                            index++;

                            if (tokens[index].Tag == TokenType.Method)
                            {
                                // Parsing a Method inside a Class
                                var method = tokens[index].Value;
                                Statements.Add((className, method, null));
                                Statements.RemoveAt(0); // Remove the class info as it's now part of the method

                                index++;

                                if (tokens[index].Tag == TokenType.Params)
                                {
                                    // Parsing Method Parameters
                                    var parameters = tokens[index].Value.Split("|").ToList();
                                    Statements.Add((className, method, parameters));
                                    Statements.RemoveAt(0);

                                    index++;

                                    // Execute the function with namespace
                                    return ExecuteFunctionWithNamespace(Statements[0]);
                                }
                            }
                        }
                        else if (tokens[index].Tag == TokenType.Number)
                        {
                            // Parsing a numeric expression
                            string expression = "";

                            while (index < tokens.Count && (tokens[index].Tag == TokenType.Number || tokens[index].Tag == TokenType.Operator))
                            {
                                expression += tokens[index].Value; // Concatenate strings
                                index++; // Increment index
                            }

                            try
                            {
                                // Evaluate the numeric expression
                                return Convert.ToDouble(new DataTable().Compute(expression, null));
                            }
                            catch (Exception e)
                            {
                                return e.Message;
                            }
                        }
                    }
                }
                if (textArray[i].StartsWith("/*"))
                {
                    if (textArray[i].EndsWith("*/")) continue;
                    textArray = CommentCode.Interperate(i, textArray.ToList());
                    i = 0;
                    continue;
                }
                else if (textArray[i].StartsWith("if"))
                {
                    textArray = IfLoop.Interperate(i, textArray.ToList());
                    i = 0;
                    continue;
                }
                else if (textArray[i].StartsWith("while"))
                {
                    textArray = WhileLoop.Interperate(i, textArray.ToList());
                    i = 0;
                    continue;
                }
                else if (textArray[i].StartsWith("for"))
                {
                    textArray = RepeatLoop.Interperate(i, textArray.ToList());
                    i = 0;
                    continue;
                }
                else if (textArray[i].Trim().StartsWith("import"))
                {
                    List<string> mainCode = new List<string>();
                    mainCode.AddRange(ImportFileStatement.Interpret(textArray[i]));
                    mainCode.AddRange(textArray.ToList().GetRange(1, textArray.ToList().Count - 1));
                    textArray = mainCode.ToArray();
                    i = i - 1;
                    continue;
                }
                else if (textArray[i].Trim().StartsWith("method"))
                {
                    int indention = 0;
                    foreach (char c in textArray[i])
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

                    if (!textArray[i].EndsWith("();"))
                    {
                        string methodName = textArray[i].Substring(indent.Length - 1 + "method".Length); // Implement GetMethodName to extract the method name
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
                    else if (textArray[i].EndsWith("();"))
                    {
                        string methodName = textArray[i].Substring(indent.Length + "method".Length, textArray[i].Length - (indent.Length + "method".Length + "();".Length)).Trim();
                        if (MethodLoop.MethodExists(methodName))
                        {
                            MethodLoop.ExecuteMethod(methodName);
                        }
                    }
                }
                else if (textArray[i].Trim().StartsWith("var"))
                {
                    string variableName;
                    string variableContents = "";
                    try
                    {
                        variableName = textArray[i].Trim().Split("=")[0];
                        variableName = variableName.Substring(3).Trim();
                        variableContents = string.Join("=", textArray[i].Trim().Split("=")[1..]).Trim();
                        variableContents = variableContents.Substring(0, variableContents.Length - 1);
                    }
                    catch
                    {
                        variableName = textArray[i].Substring(3).Trim();
                        variableName = variableName.Substring(0, variableName.Length - 1);
                    }

                    if (textArray[i].Contains("="))
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
                    if (IsExecutableCode(textArray[i]))
                    {
                        try
                        {
                            result = ExecuteFunctionWithNamespace(StatementResult);
                        }
                        catch
                        {
                            ErrorReportor.ReportError("Code Not Valid!", $"\'{textArray[i]}\' is not a valid code statement\n  {' ',-7}^ \n Error was located on Line {lineCount}");
                            break;
                        }
                    }
                    else
                    {
                        ErrorReportor.ReportError("Code Not Valid!", $"\'{textArray[i]}\' is not a valid code statement\n  {' ',-7}^ \n Error was located on Line {lineCount}");
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
                            }
                            else { dataType = "var"; value = "null"; }
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
                        MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),

                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),

                        MetadataReference.CreateFromFile(typeof(Program).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(ItemChecks).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(VariableCode).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(VariableCode).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(UniversalVariables).Assembly.Location),
                    };

                    ScriptOptions scriptOptions = ScriptOptions.Default
                        .WithReferences(references)
                        .WithImports("System", "SDL2", "System.IO", "System.Threading", "System.Threading.Tasks", "System.Windows", "System.Media", "System.Drawing", "System.Drawing.Point", "System.Windows.Forms", "System.Collections.Generic", "System.Net", "System.Net.NetworkInformation", "Easy14_Programming_Language", "Easy14_Programming_Language.UniversalVariables");

                    //code = code + $"{Environment.NewLine}Environment.Exit(0);";
                    var script = CSharpScript.Create(code, options: scriptOptions);
                    var result = script.RunAsync().Result;
                    if (result.Exception != null)
                    {
                        Console.WriteLine("Error occurred: " + result.Exception);
                    }
                    if (result.ReturnValue != null)
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

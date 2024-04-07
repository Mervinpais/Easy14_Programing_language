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
using System.Threading.Tasks;

namespace Easy14_Programming_Language
{
    public class Program
    {
        public static string pathOfPackages = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Easy14 Packages");
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string optionsPath = Path.Combine(executingAssemblyPath, "Application Code", "options.ini");
        private static readonly string[] configFile = File.ReadAllLines(Path.Combine(executingAssemblyPath, optionsPath));
        private static readonly string version = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(executingAssemblyPath).FullName).FullName).FullName).FullName, ".git", "HEAD");

        public enum Status
        {
            CSHARP_ERROR, CODE_ERROR, NORMAL, UNKNOWN
        }

        public static Status ProgramStatus = Status.NORMAL;

        static void RunChecks()
        {
            if (!Configuration.GetBoolOptionValue("UpdatesDisabled"))
                UpdateChecker.CheckLatestVersion();

            if (!string.IsNullOrEmpty(Configuration.GetStringOptionValue("packagePath")))
                pathOfPackages = Configuration.GetStringOptionValue("packagePath");

            if (Configuration.GetIntOptionValue("delay") != -1)
                Task.Delay(Configuration.GetIntOptionValue("delay") * 1000).Wait();

            if (Configuration.GetStringOptionValue("PreCompBaseCode") == "true")
            {
                Change.BackgroundColor(ConsoleColor.White);
                Change.ForegroundColor(ConsoleColor.Black);

                Console.WriteLine("Pre-compiling Base code... (Note: This will be optimised later)");

                PreCompileCodeClass.PrecompileCode();
            }
        }

        static void Main(string[] args)
        {
            RunChecks(); Console.ResetColor(); Console.Clear();


            string versionName = "{Unknown Version}";
            try 
                {   versionName = File.ReadAllLines(version)[0].Split("/")[2];   }
            catch (Exception ex) 
                {   Debugger.Error(message: $"{ex.Message}");   }

            Console.WriteLine($"{$"{RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}"}\nEasy14 {versionName}");

            if (args.Length > 0)
            {
                string filePath = string.Join(" ", args[0..]);
                if (File.Exists(filePath))
                {
                    CompileCode(File.ReadAllLines(filePath));
                    return;
                }
                else Debugger.Error("File not found", $"File \'{filePath}\' is not found!");
            }

            try
                {   bool librariesDisabled = Configuration.GetBoolOptionValue("disableLibraries");   }
            catch (Exception e)
                {   Console.WriteLine(e.Message);
                    Debugger.Warning(message: "Easy14 is using default settings due to error\n");   }

            InterpreterLoop();
        }

        public static void InterpreterLoop()
        {
            Program compiler = new Program();
            bool debugMode = false;
            while (true)
            {

                Console.Write(">>>");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                if (debugMode) DebugStats();

                switch (input)
                {
                    case "$help":
                        compiler.ExternalCompileCode(null, new string[] {
                            "Console Print { \"Help Guide!\" };",
                            "Console Print { \"\" };",
                            "Console Print { \"   - $help: Print this help guide \" };",
                            "Console Print { \"   - $copyright: Copyright rights to this product \" };",
                            "Console Print { \"   - $credits: Credits \" };",
                            "Console Print { \"   - debugMode: debugging stuff \" };",
                        });
                        break;

                    case "$copyright":
                        compiler.ExternalCompileCode(null, new string[] {
                            "Console Print { \"\" };",
                            "Console Print { \"Copyright (C) Mervinpais14 (formerly Mervinpaismakeswindows14) \" };",
                            "Console Print { \"   * MervinpaismakesWINDOWS14 is NOT affiliated with Microsoft or the Windows(TM) product\" };",
                            "Console Print { \"\" };",
                        });
                        break;

                    case "$credits":
                        compiler.ExternalCompileCode(null, new string[] {
                            "Console Print { \"\" };",
                            "Console Print { \" Thanks to;\" };",
                            "Console Print { \"   Mervin14 for the Easy14 Language Project\" };",
                            "Console Print { \"\" };",
                        });
                        break;
                    case "$debugMode":
                        debugMode = !debugMode;
                        compiler.ExternalCompileCode(textArray: [$"Console Print {{ \" Debug mode is now set to: {debugMode} \" }};"]);
                        break;

                    default:
                        compiler.ExternalCompileCode(textArray: [input]);
                        break;
                }
            }
        }

        public object ExternalCompileCode(string fileLoc = null, string[] textArray = null)
        {
            if (textArray == null && fileLoc != null)
                textArray = File.ReadAllLines(fileLoc.Trim()); 
            else if (textArray == null && fileLoc == null)
                textArray = [""];

            return CompileCode(textArray);
        }

        static public void DebugStats()
        {
            try
            {
                int xpos = Console.CursorLeft; int ypos = Console.CursorTop;
                Change.BackgroundColor(ConsoleColor.Gray); Change.ForegroundColor(ConsoleColor.Black);
                Change.CursorPos(0, 0); Console.Write(RuntimeInformation.FrameworkDescription); Change.CursorPos(xpos, ypos);
                Console.ResetColor();
            }
            catch (Exception ex) { Console.Write($"Failed to get debug info; crash details below;\n {ex.Message}"); }
        }

        public enum TokenType
        {
            Class, Method, Params, Number, Operator, Identifier, Unknown
        }

        public class Token
        {
            public string Value { get; set; }
            public TokenType Tag { get; set; }
            public int Position { get; set; }

            public Token(string value, TokenType tag, int position) { 
                Value = value;
                Tag = tag;
                Position = position;
            }
        }

        static List<string> SplitByComma(string input)
        {
            // Use regex to match commas outside quotes
            string pattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
            string[] result = Regex.Split(input, pattern);

            // Trim spaces from each element
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = result[i].Trim();
            }

            // Convert the string array to an object array
            List<string> objectArray = result.ToList();

            return objectArray;
        }

        public class Tokenizer
        {
            string identifierPattern = @"[a-zA-Z_]\w*";
            string methodsPattern = @"(.*?\ )([A-Za-z]+)\ \{(.*?)\};";
            string numberPattern = @"\d+";
            string operatorPattern = @"\+|-|\*|/";

            public List<Token> Tokenize(string input)
            {
                List<Token> tokens = new();

                var combinedPatterns = string.Join("|", methodsPattern, identifierPattern, numberPattern, operatorPattern);

                var matches = Regex.Matches(input, combinedPatterns);

                foreach (Match match in matches)
                {
                    string value = match.Value;

                    var methodMatch = Regex.Match(value, methodsPattern);
                    if (methodMatch.Success)
                    {
                        string classPart = methodMatch.Groups[1].Value.TrimEnd();
                        string methodPart = methodMatch.Groups[2].Value;
                        string paramsPart = methodMatch.Groups[3].Value;

                        List<string> parameters = new List<string>();
                        StringBuilder currentParameter = new StringBuilder();

                        parameters = SplitByComma(paramsPart);

                        parameters.Add(currentParameter.ToString().Trim());
                        parameters.RemoveAt(parameters.Count-1);

                        tokens.Add(new Token(classPart, TokenType.Class, 1));
                        tokens.Add(new Token(methodPart, TokenType.Method, 2));

                        for (int i = 0; i < parameters.Count; i++)
                        {
                            string param = parameters[i];
                            tokens.Add(new Token(param, TokenType.Params, 3+i));
                        }
                    }
                    else
                    {
                        TokenType tag = DetermineTag(value); // Implement a function to determine the tag based on the matched value
                        tokens.Add(new Token(value, tag, 1));
                    }
                }

                return tokens;
            }

            private TokenType DetermineTag(string value)
            {
                if (Regex.IsMatch(value, identifierPattern))
                    return TokenType.Identifier;

                else if (Regex.IsMatch(value, numberPattern))
                    return TokenType.Number;

                else if (Regex.IsMatch(value, operatorPattern))
                    return TokenType.Operator;

                else 
                    return TokenType.Unknown;
            }
        }

        static List<object> FunctionParser(string[] codeToExecute, int i, List<object> results)
        {
            Tokenizer tokenizer = new();
            List<Token> tokens = tokenizer.Tokenize(codeToExecute[i]);

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
                            var parameters = tokens[index].Value.Split(",").ToList();
                            Statements.Add((className, method, parameters));
                            Statements.RemoveAt(0);

                            index++;

                            // Execute the function with namespace
                            results.Add(ExecuteFunctionWithNamespace(Statements[0]));
                            return results;
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
                        results.Add(Convert.ToDouble(new DataTable().Compute(expression, null)));
                    }
                    catch (Exception e)
                    {
                        results.Add(e.Message);
                    }
                    break;
                }
            }
            return results;
        }

        static (string[] codeToExecute, int lineNumber, List<object> results) BaseFunctionParser(string[] codeToExecute, int i, List<object> results)
        {
            if (codeToExecute[i].StartsWith("/*"))
            {
                if (codeToExecute[i].EndsWith("*/")) return (codeToExecute, i, results);
                codeToExecute = CommentCode.Interperate(i, codeToExecute.ToList());
                i = 0;
                return (codeToExecute, i, results);
            }
            else if (codeToExecute[i].StartsWith("if"))
            {
                codeToExecute = IfLoop.Interperate(i, codeToExecute.ToList());
                i = 0;
                return (codeToExecute, i, results);
            }
            else if (codeToExecute[i].StartsWith("while"))
            {
                codeToExecute = WhileLoop.Interperate(i, codeToExecute.ToList());
                i = 0;
                return (codeToExecute, i, results);
            }
            else if (codeToExecute[i].StartsWith("for"))
            {
                codeToExecute = RepeatLoop.Interperate(i, codeToExecute.ToList());
                i = 0;
                return (codeToExecute, i, results);
            }
            else if (codeToExecute[i].Trim().StartsWith("import"))
            {
                List<string> mainCode =
                [
                    .. ImportFileStatement.Interpret(codeToExecute[i]),
                    .. codeToExecute.ToList().GetRange(1, codeToExecute.ToList().Count - 1),
                ];
                codeToExecute = mainCode.ToArray();
                i = i - 1;
                return (codeToExecute, i, results);
            }
            else if (codeToExecute[i].Trim().StartsWith("method"))
            {
                int indention = 0;

                foreach (char c in codeToExecute[i])
                {
                    if (c == ' ')
                        indention = indention + 1;

                    else if (c == '\t')
                        indention = indention + 3;

                    else
                        break;
                }

                string indent = "";
                for (int i_ = 0; i_ < indention; i_++)
                {
                    indent += " ";
                }

                if (!codeToExecute[i].EndsWith("();"))
                {
                    string methodName = codeToExecute[i].Substring(indent.Length - 1 + "method".Length); // Implement GetMethodName to extract the method name
                    if (methodName.Contains(" "))
                    {
                        methodName = methodName.Substring(methodName.IndexOf(" "));
                    }
                    List<string> methodCode = new List<string>();
                    int startIndex = i; // Remember the starting index

                    for (int j = i + 1; j < codeToExecute.Length; j++)
                    {
                        string line = "";
                        foreach (char code in codeToExecute[j])
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
                    return (codeToExecute, i, results);
                }
                else if (codeToExecute[i].EndsWith("();"))
                {
                    string methodName = codeToExecute[i].Substring(indent.Length + "method".Length, codeToExecute[i].Length - (indent.Length + "method".Length + "();".Length)).Trim();
                    if (MethodLoop.MethodExists(methodName))
                    {
                        MethodLoop.ExecuteMethod(methodName);
                    }
                    return (codeToExecute, i, results);
                }
                return (codeToExecute, i, results);
            }
            else if (codeToExecute[i].Trim().StartsWith("var"))
            {
                string variableName;
                string variableContents = "";
                try
                {
                    variableName = codeToExecute[i].Trim().Split("=")[0];
                    variableName = variableName.Substring(3).Trim();
                    variableContents = string.Join("=", codeToExecute[i].Trim().Split("=")[1..]).Trim();
                    variableContents = variableContents.Substring(0, variableContents.Length - 1);
                }
                catch
                {
                    variableName = codeToExecute[i].Substring(3).Trim();
                    variableName = variableName.Substring(0, variableName.Length - 1);
                }

                if (codeToExecute[i].Contains("="))
                {
                    VariableCode.DefineVariable(variableName, variableContents);
                    return (codeToExecute, i, results);
                }
                else
                {
                    if (VariableCode.VariableExists(variableName))
                    {
                        results.Add(VariableCode.variables.FirstOrDefault(v => v.Name == variableName));
                        return (codeToExecute, i, results);
                    }
                    else
                    {
                        Debugger.Error("", $"Variable {variableName} doesnt exist!");
                        return (codeToExecute, i, results);
                    }
                }
            }
            else
            {
                //Debugger.Error("Code Not Valid!", $"\'{codeToExecute[i]}\' is not a valid code statement\n  {' ',-7}^ \n Error was located on Line {i + 1}");
                return (null, -1, null);
            }
        }

        /// <summary>
        /// Main Way to compile code, All Code will run through this, from easy14 to c#
        /// </summary>
        /// <returns>The Return value of whatever code was executed</returns>
        public static List<object> CompileCode(string[] codeToExecute = null)
        {
            List<object> results = new List<object>() { };

            for (int lineNumber = 0; lineNumber < codeToExecute.Length; lineNumber++)
            {
                if (string.IsNullOrEmpty(codeToExecute[lineNumber].Trim())) { continue; }

                //var StatementResult = CommandParser.SplitCommand(textArray[i]);

                if (double.TryParse(codeToExecute[lineNumber].ToCharArray(), out _) == true)
                {
                    try
                    {
                        results.Add(Convert.ToDouble(new DataTable().Compute(codeToExecute[lineNumber], null)));
                    }
                    catch (Exception ex)
                    {
                        results.Add(ex.Message);
                    }
                }
                else if (codeToExecute[lineNumber].Trim().StartsWith("//")) { continue; }
                else
                {
                    var parserResult = BaseFunctionParser(codeToExecute, lineNumber, results);
                    int noLine = -1;
                    if (parserResult.lineNumber == noLine)
                    {
                        results = FunctionParser(codeToExecute, lineNumber, results);
                    }
                    else
                    {
                        codeToExecute = parserResult.codeToExecute;
                        lineNumber = parserResult.lineNumber;
                        results = parserResult.results;
                    }
                }

                if (results.Count! > 0) continue;

                if (ProgramStatus.HasFlag(Status.CODE_ERROR) || ProgramStatus.HasFlag(Status.CSHARP_ERROR))
                {
                    ProgramStatus = Status.NORMAL;
                    continue;
                }
            }
            return results;
        }

        public class NamespaceFunctionParserResult
        {
            public List<string> ParamsGiven { get; set; }
            public (List<string> Classes, string Method, List<string> Params) StatementResult { get; set; }
            public List<string> CodeSplitIntoLines { get; set; }
            public string Code { get; set; }
        }

        public static NamespaceFunctionParserResult NamespaceFunctionParamParser
            (List<string> paramsGiven, (List<string> classes, string method, List<string> params_) statementResult, List<string> codeSplitIntoLines, string code)
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
                    statementResult.params_.Add("\"\"");
                }
            }

            List<string> usingReferences = new();
            List<string> restOfCode = new();
            codeSplitIntoLines.Remove(_paramsDeclareLine);

            for (int i = 0; i < paramsRequired.Count; i++)
            {
                string dataType = "var";
                string value = statementResult.params_[i];
                if (value != "")
                {
                    dataType = ItemChecks.DetectType(statementResult.params_[i]);
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
            return new NamespaceFunctionParserResult
            {
                ParamsGiven = paramsGiven,
                StatementResult = statementResult,
                CodeSplitIntoLines = codeSplitIntoLines,
                Code = code
            };
        }

        public static object ExecuteFunctionWithNamespace((List<string> classes, string method, List<string> params_) StatementResult)
        {
            List<string> theClassesOfTheLine = StatementResult.classes;
            string theMethodOfTheLine = StatementResult.method;
            List<string> paramsGiven = StatementResult.params_;

            string classHierarchy = string.Join("/", theClassesOfTheLine);

            string methodFolderPath = Path.Combine(pathOfPackages, classHierarchy);
            string codeFilePath = Path.Combine(methodFolderPath, $"{theMethodOfTheLine}.cs");

            if (File.Exists(codeFilePath))
            {
                string code = File.ReadAllText(codeFilePath);
                List<string> codeSplitIntoLines = File.ReadAllLines(codeFilePath).ToList();

                try
                {
                    if (codeSplitIntoLines[0].StartsWith("//_params = "))
                    {
                        var ParamParserResult = NamespaceFunctionParamParser(paramsGiven, StatementResult, codeSplitIntoLines, code);
                        paramsGiven = ParamParserResult.ParamsGiven;
                        StatementResult = ParamParserResult.StatementResult;
                        codeSplitIntoLines = ParamParserResult.CodeSplitIntoLines;
                        code = ParamParserResult.Code;
                    }

                    var references = new List<MetadataReference>
                    {
                        MetadataReference.CreateFromFile(typeof(DataTable).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(SDL).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Linq.EnumerableQuery).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(System.Linq.Queryable).Assembly.Location),
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
                        MetadataReference.CreateFromFile(typeof(UniversalVariables).Assembly.Location),
                    };

                    string[] imports = {
                        "System",
                        "SDL2",
                        "System.IO",
                        "System.Threading",
                        "System.Threading.Tasks",
                        "System.Windows",
                        "System.Media",
                        "System.Drawing",
                        "System.Drawing.Point",
                        "System.Windows.Forms",
                        "System.Linq",
                        "System.Collections.Generic",
                        "System.Net",
                        "System.Net.NetworkInformation",
                        "Easy14_Programming_Language",
                    };

                    ScriptOptions scriptOptions = ScriptOptions.Default
                        .WithReferences(references)
                        .WithImports(imports);

                    //code = code + $"{Environment.NewLine}Environment.Exit(0);";
                    var script = CSharpScript.Create(code, options: scriptOptions);
                    //var result = script.RunAsync().Result;
                    //if (result.Exception != null)
                    //{
                    //    Console.WriteLine("Error occurred: " + result.Exception);
                    //}
                    //if (result.ReturnValue != null)
                    //{
                    //    var returnValue = result.ReturnValue;
                    //    return returnValue;
                    //}

                    string fileToLoad = $"{executingAssemblyPath}\\Precompiled\\{string.Join("\\", theClassesOfTheLine)}\\{theMethodOfTheLine}.dll";
                    var assembly = Assembly.LoadFile(fileToLoad);

                    //foreach (var loadedType in assembly.GetTypes())
                    //{
                    //    Console.WriteLine(loadedType.FullName);
                    //}


                    // Find the type containing the method
                    var type = assembly.GetType("Submission#0+MyClass"); // No need for the namespace in this case

                    var methods = type.GetMethods();

                    // Create an instance of the type (assuming it's a static class)
                    var instance = Activator.CreateInstance(type);

                    // Find the PrintLine method
                    var method = type.GetMethod(methods[0].Name);

                    var parameters = method.GetParameters();
                    if (parameters.Length > 0)
                    {
                        List<object> inputValues = new List<object>(StatementResult.params_);

                        // Call the method with the collected parameters
                        method.Invoke(instance, inputValues.ToArray());
                    }
                    else
                    {
                        // Call the method with no parameters
                        method.Invoke(instance, null);
                    }

                }
                catch (Exception e)
                {
                    Debugger.CS_Error("Package Running Error", "An Error Occurred while running the Easy14 Package (C# Error)");
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

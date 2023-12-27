using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis;
using SDL2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Easy14_Programming_Language.Program;
using System.Reflection;

namespace Easy14_Programming_Language.Application_Code
{
    public static class PreCompileCodeClass
    {
        public static string pathOfPackages = Program.pathOfPackages;
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void PrecompileCode()
        {
            List<(List<string> theClassesOfTheLine, string theMethodOfTheLine, List<string> paramsGiven)> statementResult = new List<(List<string> theClassesOfTheLine, string theMethodOfTheLine, List<string> paramsGiven)>();
            statementResult = new List<(List<string> theClassesOfTheLine, string theMethodOfTheLine, List<string> paramsGiven)>() {
                (new List<string> { "Console" }, "Print", new List<string> { "\"\"" }),
                (new List<string> { "Console" }, "Input", new List<string> { "\"\\\\x\"" })
            };

            if (!Directory.Exists(Path.Combine(executingAssemblyPath, "Precompiled")))
            {
                Directory.CreateDirectory(Path.Combine(executingAssemblyPath, "Precompiled"));
            }

            foreach ((List<string> theClassesOfTheLine, string theMethodOfTheLine, List<string> paramsGiven) e in statementResult)
            {
                runCode(e.theClassesOfTheLine, e.theMethodOfTheLine, e.paramsGiven);
            }

            void runCode(List<string> theClassesOfTheLine, string theMethodOfTheLine, List<string> paramsGiven)
            {
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
                            (List<string> classes, string method, List<string> params_) statementResult = (theClassesOfTheLine, theMethodOfTheLine, paramsGiven);
                            var ParamParserResult = NamespaceFunctionParamParser(paramsGiven, statementResult, codeSplitIntoLines, code);
                            paramsGiven = ParamParserResult.ParamsGiven;
                            statementResult = ParamParserResult.StatementResult;
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
                        "Easy14_Programming_Language.UniversalVariables"
                    };

                        ScriptOptions scriptOptions = ScriptOptions.Default
                            .WithReferences(references)
                            .WithImports(imports);

                        //code = code + $"{Environment.NewLine}Environment.Exit(0);";
                        var script = CSharpScript.Create(code, scriptOptions);
                        //var result = script.RunAsync().Result;
                        //if (result.Exception != null)
                        //{
                        //    Console.WriteLine("Error occurred: " + result.Exception);
                        //}
                        //if (result.ReturnValue != null)
                        //{
                        //    var returnValue = result.ReturnValue;
                        //}
                        
                        if (!File.Exists(Path.Combine(executingAssemblyPath, "Precompiled", theMethodOfTheLine + ".dll")))
                        {
                            var compilation = script.GetCompilation();
                            using (var stream = new MemoryStream())
                            {
                                var emitResult = compilation.Emit(stream);
                                if (emitResult.Success)
                                {
                                    File.WriteAllBytes(Path.Combine("Precompiled", theMethodOfTheLine + ".dll"), stream.ToArray());
                                }
                                else
                                {
                                    // Handle compilation errors
                                    foreach (var diagnostic in emitResult.Diagnostics)
                                    {
                                        Console.WriteLine(diagnostic);
                                    }
                                }
                            }
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
            }
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
    }
}
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
using System.ComponentModel.DataAnnotations;

namespace Easy14_Programming_Language.Application_Code
{
    public static class PreCompileCodeClass
    {
        public static string pathOfPackages = Program.pathOfPackages;
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void PrecompileCode()
        {
            List<(List<string> theClassesOfTheLine, string theMethodOfTheLine, List<string> paramsGiven)> statementResult = new List<(List<string> theClassesOfTheLine, string theMethodOfTheLine, List<string> paramsGiven)>();

            bool alreadyPrecomp = true;
            List<string> precompDirs = (from string e in Directory.GetDirectories(Path.Combine(executingAssemblyPath, "Precompiled"))
                                        select e.Split("\\")[e.Split("\\").Length - 1]).ToList();

            foreach (string dir in Directory.GetDirectories(pathOfPackages))
            {
                string dir_ = dir.Split("\\")[dir.Split("\\").Length - 1];
                if (dir_.StartsWith(".")) continue;
                if (!precompDirs.Contains(dir_))
                {
                    alreadyPrecomp = false;
                    break;
                }

                // Check if the same files are there between the two subfolders
                string precompiledDirPath = Path.Combine(executingAssemblyPath, "Precompiled", dir_);
                string currentDirPath = Path.Combine(pathOfPackages, dir_);

                string[] precompiledFiles = Directory.GetFiles(precompiledDirPath).Select(Path.GetFileNameWithoutExtension).ToArray();
                string[] currentFiles = Directory.GetFiles(currentDirPath).Select(Path.GetFileNameWithoutExtension).ToArray();

                if (!precompiledFiles.SequenceEqual(currentFiles))
                {
                    alreadyPrecomp = false;
                    break;
                }
            }

            if (alreadyPrecomp)
            {
                return;
            }

            foreach (string pathOfPackage in Directory.GetDirectories(pathOfPackages))
            {
                if (pathOfPackage.Split("\\")[pathOfPackage.Split("\\").Length-1].StartsWith(".")) continue;
                foreach (string files in Directory.GetFiles(pathOfPackage))
                {
                    if (files.EndsWith(".cs"))
                    {
                        statementResult.Add((new List<string> { pathOfPackage }, files.Substring(0, files.Length - 3), new List<string> { }));
                    }
                }
            }

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
                        //if (codeSplitIntoLines[0].StartsWith("//_params = "))
                        //{
                        //    (List<string> classes, string method, List<string> params_) statementResult = (theClassesOfTheLine, theMethodOfTheLine, paramsGiven);
                        //    var ParamParserResult = NamespaceFunctionParamParser(paramsGiven, statementResult, codeSplitIntoLines, code);
                        //    paramsGiven = ParamParserResult.ParamsGiven;
                        //    statementResult = ParamParserResult.StatementResult;
                        //    codeSplitIntoLines = ParamParserResult.CodeSplitIntoLines;
                        //    code = ParamParserResult.Code;
                        //}

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
                        MetadataReference.CreateFromFile(typeof(System.Text.RegularExpressions.Regex).Assembly.Location),

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
                        "System.Text",
                        "System.Text.RegularExpressions",
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

                        string currPath = Path.Combine(executingAssemblyPath, "Precompiled");
                        foreach (string @class in theClassesOfTheLine)
                        {
                            if (!Directory.Exists(Path.Combine(currPath, @class.Split("\\")[@class.Split("\\").Length-1])))
                            {
                                Directory.CreateDirectory(Path.Combine(currPath, @class.Split("\\")[@class.Split("\\").Length - 1]));
                            }
                            currPath = Path.Combine(currPath, @class.Split("\\")[@class.Split("\\").Length - 1]);
                        }

                        if (!File.Exists(Path.Combine(currPath, theMethodOfTheLine + ".dll")))
                        {
                            var compilation = script.GetCompilation();
                            using (var stream = new MemoryStream())
                            {
                                var emitResult = compilation.Emit(stream);
                                if (emitResult.Success)
                                {
                                    File.WriteAllBytes(Path.Combine(currPath, theMethodOfTheLine.Split("\\")[theMethodOfTheLine.Split("\\").Length - 1] + ".dll"), stream.ToArray());
                                }
                                else
                                {
                                    // Handle compilation errors
                                    foreach (var diagnostic in emitResult.Diagnostics)
                                    {
                                        Console.WriteLine(diagnostic);
                                    }
                                    Change.BackgroundColor(ConsoleColor.Red);
                                    Change.ForegroundColor(ConsoleColor.White);
                                    Console.WriteLine($"1 or More errors occured while pre-compiling code, this package [\"{theMethodOfTheLine}\"] of class(s) [\"{string.Join("", theClassesOfTheLine)}\"] has errors, want to continue with the remaining packages?");
                                    Console.ResetColor();
                                    Console.Write("(y/n)> ");
                                    if (Console.ReadLine() == "n")
                                    {
                                        Environment.Exit(-1);
                                    }
                                }
                            }
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
                    Debug.WriteLine($"The method '{theMethodOfTheLine.Split("\\")[theMethodOfTheLine.Split("\\").Length - 1]}' for class '{classHierarchy}' was not found.");
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
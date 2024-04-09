/*
Cache Invalidation System:

The goal of this system is to optimize the compilation process by implementing cache invalidation.
The main idea is to compare the modification timestamps of files in the main directory with their
corresponding precompiled versions in the output directory. If a file in the main directory is newer
than its precompiled counterpart, it indicates that the file has been modified and needs to be 
recompiled. By selectively recompiling only the outdated files, we can improve build times and 
development efficiency.

Steps to implement:
1. Traverse through each file in the main directory.
2. Check if a precompiled version of the file exists in the output directory.
3. Compare the modification timestamps of the main directory file and its precompiled counterpart.
4. If the main directory file is newer, mark it for recompilation.
5. Implement logic to selectively recompile outdated files.
6. Test and validate the cache invalidation system to ensure correctness and efficiency.

Note: Remove this comment once the cache invalidation system is fully implemented.
*/





















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
using static Easy14_Programming_Language.Program;

namespace Easy14_Programming_Language.Application_Code
{
    class FileInfos
    {
        public string fileLoc { get; set; }
        public string fileName { get; set; }
        public string[] directories { get; set; }
    }

    public static class PreCompileCodeClass
    {
        public static string pathOfPackages = Program.pathOfPackages;
        private static readonly string executingAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void PrecompileCode()
        {
            List<(List<string> theClassesOfTheLine, string theMethodOfTheLine, List<string> paramsGiven)> statementResult = new List<(List<string> theClassesOfTheLine, string theMethodOfTheLine, List<string> paramsGiven)>();

            bool alreadyPrecomp = true;
            List<string> precompDirs = Directory.GetDirectories(Path.Combine(executingAssemblyPath, "Precompiled"))
                                    .Select(directory => Path.GetFileName(directory))
                                    .ToList();

            string PrecompiledFolderpath = Path.Combine(executingAssemblyPath, "Precompiled");
            if (!Directory.Exists(PrecompiledFolderpath))
            {
                Directory.CreateDirectory(PrecompiledFolderpath);
            }

            List<FileInfos> filesToRecompile = new List<FileInfos>();//fileLoc, fileName, directories

            void CheckFilesAreOutDated(string FileLoc)
            {
                foreach (string dir in Directory.GetDirectories(FileLoc))
                {
                    string directoryName = dir.Split("\\")[dir.Split("\\").Length - 1];

                    if (directoryName.StartsWith('.')) continue;

                    foreach (string file in Directory.GetFiles(dir))
                    {
                        string fileName = file.Split("\\")[file.Split("\\").Length - 1];
                        fileName = fileName.Replace(".cs", ".dll");
                        if (Directory.Exists(file))
                        {
                            CheckFilesAreOutDated(file);
                        }

                        string PreCompFileLoc = Path.Combine(PrecompiledFolderpath, directoryName, fileName);
                        if (File.Exists(PreCompFileLoc))
                        {
                            if (File.GetLastWriteTime(file) > File.GetLastWriteTime(PreCompFileLoc))
                            {
                                Console.WriteLine($"File \'{fileName}\' needs recompiling");
                                filesToRecompile.Add(new FileInfos
                                {
                                    fileLoc = file,
                                    fileName = fileName,
                                    directories = Path.Combine(pathOfPackages, directoryName).Split("\\")
                                });
                            }
                            else
                            {
                                Console.WriteLine($"Precompiled File \'{fileName}\' is up to date");
                            }
                        }
                    }
                }
            }


            CheckFilesAreOutDated(pathOfPackages);
            //return;
            foreach (FileInfos file in filesToRecompile)
            {
                runCode(file);
            }

            void runCode(FileInfos fileInfos)
            {

                string methodFolderPath = Path.Combine(pathOfPackages, string.Join("\\", fileInfos.directories));
                string codeFilePath = Path.Combine(methodFolderPath, $"{fileInfos.fileName.Replace(".dll", ".cs")}");

                if (File.Exists(codeFilePath))
                {
                    string code = File.ReadAllText(codeFilePath);
                    List<string> codeSplitIntoLines = File.ReadAllLines(codeFilePath).ToList();

                    try
                    {
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

                        var script = CSharpScript.Create(code, scriptOptions);

                        string currPath = Path.Combine(executingAssemblyPath, "Precompiled");
                        string precompDirectoriesClasses_ = string.Join("\\", fileInfos.directories);
                        precompDirectoriesClasses_ = precompDirectoriesClasses_.Replace(pathOfPackages + "\\", "");
                        string[] precompDirectoriesClasses_2 = precompDirectoriesClasses_.Split("\\");

                        foreach (string @class in precompDirectoriesClasses_2)
                        {
                            if (!Directory.Exists(Path.Combine(currPath, @class.Split("\\")[@class.Split("\\").Length - 1])))
                            {
                                Directory.CreateDirectory(Path.Combine(currPath, @class.Split("\\")[@class.Split("\\").Length - 1]));
                            }
                            currPath = Path.Combine(currPath, @class.Split("\\")[@class.Split("\\").Length - 1]);
                        }

                        var compilation = script.GetCompilation();
                        using (var stream = new MemoryStream())
                        {
                            var emitResult = compilation.Emit(stream);
                            if (emitResult.Success)
                            {
                                File.WriteAllBytes(Path.Combine(currPath, fileInfos.fileName.Split("\\")[fileInfos.fileName.Split("\\").Length - 1]), stream.ToArray());
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
                                Console.WriteLine($"1 or More errors occured while pre-compiling code, this package [\"{fileInfos.fileName.Replace(".dll", "")}\"] of class(s) [\"{string.Join("", string.Join("\\", fileInfos.directories))}\"] has errors, want to continue with the remaining packages?");
                                Console.ResetColor();
                                Console.Write("(y/n)> ");
                                if (Console.ReadLine() == "n")
                                {
                                    Environment.Exit(-1);
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
                    Debug.WriteLine($"The method '{fileInfos.fileName.Replace(".dll", "").Split("\\")[fileInfos.fileName.Replace(".dll", "").Split("\\").Length - 1]}' for class '{string.Join("\\", fileInfos.directories)}' was not found.");
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
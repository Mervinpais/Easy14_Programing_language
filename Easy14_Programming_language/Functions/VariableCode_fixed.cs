using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class VariableCode
    {
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);

        public static void Interperate(string code_part, string[] lines, int line_count)
        {
            string code_part_unedited = code_part;
            code_part = code_part.TrimStart().Substring(3).TrimStart();
            var textToPrint = code_part;

            if (textToPrint.Split(" ")[1].Contains("="))
            {
                string varName = textToPrint.Substring(0, textToPrint.IndexOf("=")).ToString().TrimStart().TrimEnd();
                string varContent = textToPrint.Substring(textToPrint.IndexOf("="), textToPrint.Length - textToPrint.IndexOf("=")).ToString();

                char[] varName_charArray = varName.ToCharArray();
                if (char.IsDigit(varName_charArray[0]))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; You cant have numbers at the start of a variable name");
                    Console.ResetColor();
                }

                if (varName.Contains("/") || varName.Contains("\\") || varName.Contains(":") || varName.Contains("*") || varName.Contains("?") || varName.Contains("\"") || varName.Contains("<") || varName.Contains(">") || varName.Contains("|") || varName.Contains(";") || varName.Contains("{") || varName.Contains("}"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; You cant have Special Letters in the variable name");
                    Console.ResetColor();
                    return;
                }
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP");
                varContent = varContent.Substring(1).TrimStart();

                var varContent_clone = varContent.Replace("=", "").TrimStart().ToLower();

                bool doesContainMathFunctions = varContent_clone.StartsWith("cos") || varContent_clone.StartsWith("sin") || varContent_clone.StartsWith("tan") || varContent_clone.StartsWith("abs");

                if (varContent.StartsWith("random.range(") && varContent.EndsWith(");"))
                {
                    string text = varContent;
                    text = text.Replace("random.range(", "");
                    text = text.Replace(")", "");
                    int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                    int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                    Random rnd = new Random();
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", Random_RandomRange.Interperate(varContent, lines, null));
                }

                else if (varContent.StartsWith($"Console.input(") || varContent.StartsWith($"input(") && varContent.EndsWith(");"))
                {
                    var userInput = ConsoleInput.Interperate(code_part/*, lines, varName: varName*/);
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", userInput.ToString());
                }

                else if (varContent.Contains("+") || varContent.Contains("-") || varContent.Contains("/") || varContent.Contains("*") || varContent.Contains("%") && (varContent.StartsWith("\"") && varContent.EndsWith("\"")))
                {
                    varContent = varContent.Substring(0, varContent.Length - 1);
                    try
                    {
                        double result = Convert.ToDouble(new DataTable().Compute(varContent, null));
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", result.ToString());
                    }
                    catch
                    {
                        ThrowErrorMessage.sendErrMessage($"Uh oh, the value you wanted to calculate won't work! (check if the value is a string and change it to an integer)", null, "error");
                    }
                }
                else if (varContent.Contains("=="))
                {
                    if (doesContainMathFunctions) return;
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", isEqualTo.Interperate(varContent, varName).ToString());
                }
                else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("cos"))
                {
                    var result = Math_Cos.Interperate(varContent, line_count, varName);
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", result.ToString());
                }
                else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sin"))
                {
                    var result = Math_Sin.Interperate(varContent, line_count, varName);
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", result.ToString());
                }
                else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("tan"))
                {
                    var result = Math_Tan.Interperate(varContent, line_count, varName);
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", result.ToString());
                }
                else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("abs"))
                {
                    var result = Math_Abs.Interperate(varContent);
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", result.ToString());
                }
                else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sq"))
                {
                    var result = Math_Square.Interperate(varContent, line_count, varName);
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", result.ToString());
                }
                else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sqrt"))
                {
                    var result = Math_SquareRoot.Interperate(varContent, line_count, varName);
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", result.ToString());
                }
                else if (varContent.StartsWith("\"") && varContent.EndsWith("\";"))
                {
                    varContent = varContent.Substring(1, varContent.Length - 1);
                    varContent = varContent.Substring(0, varContent.Length - 2);
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", varContent);
                }
                else if (int.TryParse(varContent.Substring(0, varContent.Length - 1), out _) == true)
                {
                    varContent = varContent.Substring(0, varContent.Length - 1);
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}", varContent);
                }
                else
                {
                    string funcLine = null;
                    try
                    {
                        funcLine = varContent.Substring(0, varContent.IndexOf("("));
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: " + varContent + " is not a valid line.");
                        Console.ResetColor();
                        return;
                    }
                    if (funcLine.Contains("."))
                    {
                        string[] allNamespacesAvaiable_array = Directory.GetDirectories(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net7.0-windows", "").Replace("\\bin\\Release\\net7.0-windows", "") + "\\Functions");
                        List<string> allNamespacesAvaiable_list = new List<string>(allNamespacesAvaiable_array);
                        List<string> allNamespacesAvaiable_list_main = new List<string>();
                        foreach (string item in allNamespacesAvaiable_list)
                        {
                            allNamespacesAvaiable_list_main.Add(item[(item.LastIndexOf("\\") + 1)..]);
                        }
                        allNamespacesAvaiable_array = allNamespacesAvaiable_list_main.ToArray();
                        string theNamespaceOfTheLine = varContent.Split(".")[0];
                        if (allNamespacesAvaiable_array.Contains(theNamespaceOfTheLine))
                        {
                            int index = Array.IndexOf(allNamespacesAvaiable_array, theNamespaceOfTheLine);
                            string theClassOfTheLine = varContent.Split(".")[0];
                            string theFunctionOfTheLine = varContent.Split(".")[1];
                            string params_str = varContent.Replace($"{theNamespaceOfTheLine}.{theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("))}(", "");
                            params_str = params_str.Substring(0, params_str.Length - 2);
                            string[] params_ = { };

                            try { params_ = params_str.Split(","); }
                            catch { /* Now we know this is a method without parameters */}
                            string theFunctionOfTheLine_params = theFunctionOfTheLine;
                            theFunctionOfTheLine = theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("));

                            //theFunctionOfTheLine = theFunctionOfTheLine.Replace("(", "").Replace(");", "");

                            string[] code =
                            {
                                $"string theFunctionOfTheLine = \"{theFunctionOfTheLine}\";",
                                $"string[] params_ = {{{string.Join("\"" + System.Environment.NewLine + "\"", params_)}}};",
                                "System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + \"\\\\EASY14_Variables_TEMP\");",
                                $"var content = Easy14_Programming_Language.{theFunctionOfTheLine}.Interperate({string.Join(",", params_)});",
                                "System.IO.File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + \"\\\\EASY14_Variables_TEMP\\\\" + $"{varName}" + "\", content);"
                            };

                            try
                            {
                                CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                            }
                            catch (Exception e)
                            {
                                CSharpErrorReporter.ConsoleLineReporter.Error("Error: The function you are trying to use returned an Error\n" + e.Message, "The function returned an Error");
                            }
                            return;
                        }
                    }
                    else if (!funcLine.Contains("."))
                    {
                        string[] allNamespacesAvaiable_array = Directory.GetDirectories(strWorkPath.Replace("\\bin\\Debug\\net7.0-windows", "").Replace("\\bin\\Release\\net7.0-windows", "") + "\\Functions");
                        List<string> allNamespacesAvaiable_list = new List<string>(allNamespacesAvaiable_array);
                        List<string> allNamespacesAvaiable_list_main = new List<string>();
                        foreach (string item in allNamespacesAvaiable_list)
                        {
                            allNamespacesAvaiable_list_main.Add(item[(item.LastIndexOf("\\") + 1)..]);
                        }
                        allNamespacesAvaiable_array = allNamespacesAvaiable_list_main.ToArray();
                        string theFunctionOfTheLine = varContent;
                        int index = Array.IndexOf(allNamespacesAvaiable_array, theFunctionOfTheLine);
                        string params_str = varContent.Replace($"{theFunctionOfTheLine}.{theFunctionOfTheLine.Substring(0, theFunctionOfTheLine.IndexOf("("))}(", "");
                        params_str = params_str.Substring(1, params_str.Length - 2);
                        params_str = params_str.Substring(params_str.IndexOf("("), params_str.Length - params_str.IndexOf("("));
                        params_str = params_str.Substring(1, params_str.Length - 2);
                        string[] params_ = { };

                        try { params_ = params_str.Split(","); }
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
                                $"string theFunctionOfTheLine = \"{theFunctionOfTheLine}\";",
                                $"string[] params_ = {{{string.Join("\"" + System.Environment.NewLine + "\"", params_)}}};",
                                "System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + \"\\\\EASY14_Variables_TEMP\");",
                                $"var content = System.Convert.ToString(Easy14_Programming_Language.{theFunctionOfTheLine}.Interperate({string.Join("\",\"", params_)}));",
                                "System.IO.File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + \"\\\\EASY14_Variables_TEMP\\\\" + $"{varName}" + "\", content);"
                        };
                        try
                        {
                            CSharpScript.RunAsync(string.Join(Environment.NewLine, code), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error: The function you are trying to use returned an Error");
                            Console.WriteLine($"\n{e.Message}");
                            Console.ResetColor();
                        }
                        return;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    //Console.WriteLine(varContent);
                    Console.WriteLine($"ERROR; '{code_part}' is not a vaild value of any data-type\n  Error was located on Line {line_count}");
                }
            }
            else
            {
                string varName = textToPrint.ToString();
                varName = varName.TrimStart().TrimEnd();
                char[] varName_charArray = varName.ToCharArray();
                if (char.IsDigit(varName_charArray[0]))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; You cant have numbers at the start of a variable name");
                }
                else
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP\{varName}", null);
                }
            }
        }
    }

    public class AssignStatement
    {
        public string Variable { get; set; }
        public int Value { get; set; }
    }

}
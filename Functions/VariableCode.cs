using System;
using System.IO;

namespace Easy14_Programming_Language
{
    class VariableCode
    {
        Program prog = new Program();
        //Above needed as functions like 'compileCode' in Program.cs cant be accessed here and instead of copying it to other functions, just make an object of it 
        //and use it's 'compileCode_forOtherFiles' function to get 'compileCode' (because 'compileCode' is static, we need another function that is not static to access
        //the static function, kinda smart in my opinion), its kind of a bad way of doing it but it the easy way and has no error with it :|

        /// <summary>
        /// It takes a string, an array of strings, and an integer, and does something with them.
        /// </summary>
        /// <param name="code_part">The part of the code that is being Interperated.</param>
        /// <param name="lines">The lines of code</param>
        /// <param name="line_count">The line number of the code_part</param>
        public void Interperate(string code_part, string[] lines, int line_count)
        {
            string code_part_unedited = code_part;
            code_part = code_part.TrimStart();
            code_part = code_part.Substring(3);

            var textToPrint = code_part;
            if (textToPrint.Contains("="))
            {
                string varName = textToPrint.Substring(0, textToPrint.IndexOf("=")).ToString();
                varName = varName.TrimStart().TrimEnd();
                string varContent = textToPrint.Substring(textToPrint.IndexOf("="), textToPrint.Length - textToPrint.IndexOf("=")).ToString();

                char[] varName_charArray = varName.ToCharArray();
                if (char.IsDigit(varName_charArray[0]))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; You cant have numbers at the start of a variable name");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (varName.Contains("/") || varName.Contains(@"\") || varName.Contains(":") || varName.Contains("*") || varName.Contains("?") || varName.Contains("\"") || varName.Contains("<") || varName.Contains(">") || varName.Contains("|") || varName.Contains(";") || varName.Contains("{") || varName.Contains("}"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; You cant have Special Letters in the variable name");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Directory.CreateDirectory(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                            + $"\\EASY14_Variables_TEMP"
                    );
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
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", Convert.ToString(rnd.Next(number1, number2)));
                    }

                    else if (varContent.StartsWith($"Console.input(") || varContent.StartsWith($"input(") && varContent.EndsWith(");"))
                    {
                        ConsoleInput conInput = new ConsoleInput();
                        var userInput = conInput.Interperate(code_part, lines, varName: varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", userInput.ToString());
                    }

                    else if (varContent.Contains("+"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;

                        Math_Add math_add = new Math_Add();
                        var result = math_add.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Contains("-"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;

                        Math_Subtract math_sub = new Math_Subtract();
                        var result = math_sub.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Contains("/"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;

                        Math_Divide math_div = new Math_Divide();
                        var result = math_div.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Contains("*"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;

                        Math_Multiply math_multiply = new Math_Multiply();
                        var result = math_multiply.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Contains("%"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;

                        Math_Module math_mod = new Math_Module();
                        var result = math_mod.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Contains("=="))
                    {
                        if (doesContainMathFunctions) return;

                        isEqualTo math_equals = new isEqualTo();
                        var result = math_equals.Interperate(varContent, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("cos"))
                    {
                        Math_Cos math_cos = new Math_Cos();
                        var result = math_cos.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sin"))
                    {
                        Math_Sin math_sin = new Math_Sin();
                        var result = math_sin.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("tan"))
                    {
                        Math_Tan math_tan = new Math_Tan();
                        var result = math_tan.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("abs"))
                    {
                        Math_Abs math_abs = new Math_Abs();
                        var result = math_abs.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sq"))
                    {
                        Math_Square math_sq = new Math_Square();
                        var result = math_sq.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sqrt"))
                    {
                        Math_SquareRoot math_sqrt = new Math_SquareRoot();
                        var result = math_sqrt.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.StartsWith('"'.ToString()) && varContent.EndsWith("\";"))
                    {
                        varContent = varContent.Substring(1, varContent.Length - 2);
                        varContent = varContent.Substring(0, varContent.Length - 2);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", varContent);
                    }
                    else if (int.TryParse(varContent.Substring(0, varContent.Length - 1), out _) == true)
                    {
                        varContent = varContent.Substring(0, varContent.Length - 1);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", varContent);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        //Console.WriteLine(varContent);
                        Console.WriteLine($"ERROR; '{code_part}' is not a vaild value of any data-type\n  Error was located on Line {line_count}");
                    }
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
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP\{varName}.txt", null);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        /// <param name="code_part">The part of the code that is being interperated.</param>
        /// <param name="lines">The lines of code</param>
        /// <param name="line_count">The line number of the code_part</param>
        public void interperate(string code_part, string[] lines, int line_count)
        {
            string endOfStatementCode = ")";
            string[] configFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "") + "\\Application Code\\options.ini");
            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons"))
                    endOfStatementCode.Equals(line.EndsWith("true") ? endOfStatementCode = ";" : endOfStatementCode = ")");
                    break;
            }

            string code_part_unedited = code_part;
            code_part = code_part.TrimStart();
            code_part = code_part.Substring(3);
            //code_part = code_part.Substring(0, code_part.Length - 1);
            var textToPrint = code_part;
            if (textToPrint.Contains("="))
            {
                string varName = textToPrint.Substring(0, textToPrint.IndexOf("=")).ToString();
                varName = varName.TrimStart().TrimEnd();
                string varContent = textToPrint.Substring(textToPrint.IndexOf("="), textToPrint.Length - textToPrint.IndexOf("=")).ToString();
                
                
                /* Checking if the variable name starts with a number. */
                if (
                    varName.StartsWith("0") || varName.StartsWith("1") || varName.StartsWith("2") || varName.StartsWith("3") || varName.StartsWith("4") || varName.StartsWith("5") || varName.StartsWith("6") || varName.StartsWith("7") || varName.StartsWith("8") || varName.StartsWith("9")
                    )
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; You cant have numbers at the start of a variable name");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                /* Checking if the variable name contains any of the special characters. */
                if (
                    varName.Contains("/") || varName.Contains(@"\") || varName.Contains(":") || varName.Contains("*") || varName.Contains("?") || varName.Contains("\"") || varName.Contains("<") || varName.Contains(">") || varName.Contains("|") || varName.Contains(";") || varName.Contains("{") || varName.Contains("}")
                    )
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
                    
                    //Below 2 lines of code will be used later in the code, its just for not needing to copy and paste alot
                    var varContent_clone = varContent.Replace("=", "").TrimStart().ToLower();
                    
                    /* Checking if the variable content starts with any of the math functions. */
                    bool doesContainMathFunctions = varContent_clone.StartsWith("cos") || varContent_clone.StartsWith("sin") || varContent_clone.StartsWith("tan") || varContent_clone.StartsWith("abs");

                    /* Checking if the variable content starts with "random.range(" and ends with ")" or ";" (depending on
                    the end of statement code). If it does, it will replace "random.range(" with nothing, and replace
                    ")" with nothing. Then it will get the first number, and the second number. Then it will create a
                    random number between the two numbers. Then it will write the random number to a file. */
                    if (varContent.StartsWith("random.range(") && varContent.EndsWith(endOfStatementCode == ")" ? "" : ";"))
                    {
                        string text = varContent;
                        text = text.Replace("random.range(", "");
                        text = text.Replace(")", "");
                        int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                        int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                        Random rnd = new Random();
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", Convert.ToString(rnd.Next(number1, number2)));
                    }
                    
                    /* Checking if the code is a console input. */
                    else if (varContent.StartsWith($"Console.input(") || varContent.StartsWith($"input(") && varContent.EndsWith(endOfStatementCode == ")" ? "" : ";"))
                    {
                        ConsoleInput conInput = new ConsoleInput();
                        conInput.interperate(code_part, null, null, varName);
                    }

                    else if (varContent.Contains("+"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;

                        Math_Add math_add = new Math_Add();
                        var result = math_add.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt",result.ToString());
                    }
                    else if (varContent.Contains("-"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;
                        
                        Math_Subtract math_sub = new Math_Subtract();
                        var result = math_sub.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Contains("/"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;

                        Math_Divide math_div = new Math_Divide();
                        var result = math_div.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Contains("*"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;

                        Math_Multiply math_multiply = new Math_Multiply();
                        var result = math_multiply.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Contains("%"))
                    {
                        if (doesContainMathFunctions) return;
                        if (varContent.Contains("\"")) return;

                        Math_Module math_mod = new Math_Module();
                        var result = math_mod.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Contains("=="))
                    {
                        if (doesContainMathFunctions) return;
                        
                        Math_Equals math_equals = new Math_Equals();
                        var result = math_equals.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("cos"))
                    {
                        Math_Cos math_cos = new Math_Cos();
                        var result = math_cos.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=","").TrimStart().ToLower().StartsWith("sin"))
                    {
                        Math_Sin math_sin = new Math_Sin();
                        var result = math_sin.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("tan"))
                    {
                        Math_Tan math_tan = new Math_Tan();
                        var result = math_tan.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("abs"))
                    {
                        Math_Abs math_abs = new Math_Abs();
                        var result = math_abs.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sq"))
                    {
                        Math_Square math_sq = new Math_Square();
                        var result = math_sq.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sqrt"))
                    {
                        Math_SquareRoot math_sqrt = new Math_SquareRoot();
                        var result = math_sqrt.interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.StartsWith('"'.ToString()) && varContent.EndsWith(endOfStatementCode == ")" ? "\";" : "\""))
                    {
                        if (endOfStatementCode != ")")
                            varContent = varContent.Substring(1);
                        else if (endOfStatementCode == ")")
                            varContent = varContent.Substring(1, varContent.Length - 2);
                        varContent = varContent.Substring(0, endOfStatementCode == ")" ? varContent.Length - 1  : varContent.Length - 2);
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
                if (varName.StartsWith("0")
                    || varName.StartsWith("1")
                    || varName.StartsWith("2")
                    || varName.StartsWith("3")
                    || varName.StartsWith("4")
                    || varName.StartsWith("5")
                    || varName.StartsWith("6")
                    || varName.StartsWith("7")
                    || varName.StartsWith("8")
                    || varName.StartsWith("9"))
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
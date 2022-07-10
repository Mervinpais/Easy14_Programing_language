using System;
using System.Data;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class VariableCode
    {
        //Above needed as functions like 'compileCode' in Program.cs cant be accessed here and instead of copying it to other functions, just make an object of it 
        //and use it's 'compileCode_forOtherFiles' function to get 'compileCode' (because 'compileCode' is static, we need another function that is not static to access
        //the static function, kinda smart in my opinion), its kind of a bad way of doing it but it the easy way and has no error with it :|

        /// <summary>
        /// It takes a string, an array of strings, and an integer, and does something with them.
        /// </summary>
        /// <param name="code_part">The part of the code that is being Interperated.</param>
        /// <param name="lines">The lines of code</param>
        /// <param name="line_count">The line number of the code_part</param>
        public static void Interperate(string code_part, string[] lines, int line_count)
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
                        var userInput = ConsoleInput.Interperate(code_part, lines, varName: varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", userInput.ToString());
                    }

                    else if (varContent.Contains("+") || varContent.Contains("-") || varContent.Contains("/") || varContent.Contains("*") || varContent.Contains("%"))
                    {
                        varContent = varContent.Substring(0, varContent.Length - 1);
                        try
                        {
                            double result = Convert.ToDouble(new DataTable().Compute(varContent, null));
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                        }
                        catch
                        {
                            ThrowErrorMessage.sendErrMessage($"Uh oh, the value you wanted to calculate won't work! (check if the value is a string and change it to an integer)", null, "error");
                        }
                    }
                    else if (varContent.Contains("=="))
                    {
                        if (doesContainMathFunctions) return;
                        var result = isEqualTo.Interperate(varContent, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("cos"))
                    {
                        var result = Math_Cos.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sin"))
                    {
                        var result = Math_Sin.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("tan"))
                    {
                        var result = Math_Tan.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("abs"))
                    {
                        var result = Math_Abs.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sq"))
                    {
                        var result = Math_Square.Interperate(varContent, line_count, varName);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{varName}.txt", result.ToString());
                    }
                    else if (varContent.Replace("=", "").TrimStart().ToLower().StartsWith("sqrt"))
                    {
                        var result = Math_SquareRoot.Interperate(varContent, line_count, varName);
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
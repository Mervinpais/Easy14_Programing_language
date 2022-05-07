using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;
using System.Linq;
using System.Management;

namespace Easy14_Coding_Language
{
    class VARIABLE_var
    {
        Program prog = new Program();
        //Above needed as functions like 'compileCode' in Program.cs cant be accessed here and instead of copying it to other functions, just make an object of it 
        //and use it's 'compileCode_forOtherFiles' function to get 'compileCode' (because 'compileCode' is static, we need another function that is not static to access
        //the static function, kinda smart in my opinion), its kind of a bad way of doing it but it the easy way and has no error with it :|
        public void interperate(string code_part, string[] lines, int line_count)
        {
            string code_part_unedited = code_part;
            code_part = code_part.TrimStart();
            code_part = code_part.Substring(3);
            code_part = code_part.Substring(0, code_part.Length - 1);
            var textToPrint = code_part;
            if (textToPrint.Contains("="))
            {
                string varName = textToPrint.Substring(0, textToPrint.IndexOf("=")).ToString();
                varName = varName.TrimStart().TrimEnd();
                string varContent = textToPrint.Substring(textToPrint.IndexOf("="), textToPrint.Length - textToPrint.IndexOf("=")).ToString();
                //Console.WriteLine(varContent);
                if (
                    varName.StartsWith("0")
                    || varName.StartsWith("1")
                    || varName.StartsWith("2")
                    || varName.StartsWith("3")
                    || varName.StartsWith("4")
                    || varName.StartsWith("5")
                    || varName.StartsWith("6")
                    || varName.StartsWith("7")
                    || varName.StartsWith("8")
                    || varName.StartsWith("9")
                )
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; You cant have numbers at the start of a variable name");
                }
                else
                {
                    Directory.CreateDirectory(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                            + @$"\EASY14_Variables_TEMP"
                    );
                    varContent = varContent.Substring(1).TrimStart();
                    if (varContent.StartsWith("random.range(") && varContent.EndsWith($")"))
                    {
                        string text = varContent;
                        text = text.Replace("random.range(", "");
                        text = text.Replace(")", "");
                        int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                        int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                        Random rnd = new Random();
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP\{varName}.txt", Convert.ToString(rnd.Next(number1, number2)));
                    }
                    else if (varContent.StartsWith($"Console.input(") || varContent.StartsWith($"input(") && varContent.EndsWith($")"))
                    {
                        if (varContent.StartsWith($"input("))
                        {
                            IList<string> lines_Ilist = new List<string>(lines);
                            //Console.WriteLine(String.Join(Environment.NewLine, lines_Ilist));
                            bool foundUsing = false;
                            //Finally got thw time to learn Linq and its just like sql query :D
                            var TheUsingFound = from l in lines_Ilist
                                                where l == ("using Console;")
                                                select l;
                            //Console.WriteLine(String.Join(Environment.NewLine, TheUsingFound));
                            if (TheUsingFound.ToArray()[0] == ("using Console;"))
                            {
                                foundUsing = true;
                                //Console.WriteLine(foundUsing);
                            }
                            else
                            {
                                foundUsing = false;
                                //Console.WriteLine(foundUsing);
                            }

                            if (TheUsingFound.ToArray().Length > 1)
                            {

                            }
                            /*foreach (string x in lines)
                            {
                                if (x == varContent)
                                {
                                    break;
                                }
                                if (x.StartsWith("using"))
                                {
                                    if (x == "using Console;")
                                    {
                                        foundUsing = true;
                                        break;
                                    }
                                }
                            }*/
                            if (foundUsing == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(
                                    $"ERROR; The Using 'Console' wasnt referenced to use 'input' without its reference  (Use Console.input(\"*Text*\") to fix this error :)"
                                );
                            }
                        }
                        else if (varContent.StartsWith($"Console.input(")) { }
                        string input_line_ = varContent;
                        input_line_ = input_line_.TrimStart();
                        if (varContent.StartsWith("Console.input("))
                        {
                            input_line_ = input_line_.Substring(14);
                        }
                        else if (varContent.StartsWith("input("))
                        {
                            input_line_ = input_line_.Substring(6);
                        }
                        string input_textToPrint = input_line_;
                        if (input_textToPrint.StartsWith('"'.ToString()) && input_textToPrint.EndsWith('"'.ToString()))
                        {
                            Console.WriteLine(input_textToPrint);
                        }
                        else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                        {
                            string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                            foreach (string file in files)
                            {
                                if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == input_textToPrint)
                                {
                                    var contentInFile = File.ReadAllText(file);
                                    Console.WriteLine(contentInFile.ToString());
                                    break;
                                }
                            }
                        }
                        Console.Write(">");
                        string textFromUser = Console.ReadLine();
                        File.WriteAllText(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                                + @$"\EASY14_Variables_TEMP\{varName}.txt",
                            textFromUser
                        );
                    }
                    else if (varContent.Contains("+"))
                    {
                        Math_Add math_add = new Math_Add();
                        math_add.interperate(varContent, line_count, varName);
                    }
                    else if (varContent.Contains("-"))
                    {
                        Math_Subtract math_sub = new Math_Subtract();
                        math_sub.interperate(varContent, line_count, varName);
                    }
                    else if (varContent.Contains("/"))
                    {
                        Math_Divide math_div = new Math_Divide();
                        math_div.interperate(varContent, line_count, varName);
                    }
                    else if (varContent.Contains("*"))
                    {
                        Math_Multiply math_multiply = new Math_Multiply();
                        math_multiply.interperate(varContent, line_count, varName);
                    }
                    else if (varContent.Contains("%"))
                    {
                        Math_Module math_mod = new Math_Module();
                        math_mod.interperate(varContent, line_count, varName);
                    }
                    else if (varContent.Contains("=="))
                    {
                        Math_Equals math_equals = new Math_Equals();
                        math_equals.interperate(varContent, line_count, varName);
                    }
                    else if (varContent.Replace("=","").TrimStart().ToLower().StartsWith("cos"))
                    {
                        Math_Cos math_cos = new Math_Cos();
                        math_cos.interperate(varContent, line_count, varName);
                    }
                    else if (varContent.StartsWith($"Math.Add(") || varContent.StartsWith($"Add(") && varContent.EndsWith($")"))
                    {
                        if (varContent.StartsWith($"Add("))
                        {
                            bool foundUsing = false;
                            foreach (string x in lines)
                            {
                                if (x == varContent)
                                {
                                    break;
                                }
                                if (x.StartsWith("using"))
                                {
                                    if (x == "using Math;")
                                    {
                                        foundUsing = true;
                                        break;
                                    }
                                }
                            }
                            if (foundUsing == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(
                                    $"ERROR; The Using 'Math' wasnt referenced to use 'Add' without its reference  (Use Console.input(\"*Text*\") to fix this error :)"
                                );
                            }
                        }
                        else if (varContent.StartsWith($"Math.Add(")) { }
                        string input_line_ = varContent;
                        input_line_ = input_line_.TrimStart();
                        if (varContent.StartsWith("Math.Add("))
                        {
                            input_line_ = input_line_.Substring(9);
                        }
                        else if (varContent.StartsWith("Add("))
                        {
                            input_line_ = input_line_.Substring(4);
                        }
                        string input_textToPrint = input_line_;
                        if (input_textToPrint.StartsWith('"'.ToString()) && input_textToPrint.EndsWith('"'.ToString()))
                        {
                            Console.WriteLine(input_textToPrint);
                        }
                        else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                        {
                            string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                            foreach (string file in files)
                            {
                                if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == input_textToPrint)
                                {
                                    var contentInFile = File.ReadAllText(file);
                                    Console.WriteLine(contentInFile.ToString());
                                    break;
                                }
                            }
                        }
                        Console.Write(">");
                        string textFromUser = Console.ReadLine();
                        File.WriteAllText(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                                + @$"\EASY14_Variables_TEMP\{varName}.txt",
                            textFromUser
                        );
                    }
                    else if (varContent.StartsWith('"'.ToString()) && varContent.EndsWith('"'.ToString()))
                    {
                        varContent = varContent.Substring(1);
                        varContent = varContent.Substring(0, varContent.Length - 1);
                        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP\{varName}.txt", varContent);
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
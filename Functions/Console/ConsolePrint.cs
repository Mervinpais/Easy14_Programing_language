using System;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    class ConsolePrint
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(strWorkPath + "..\\..\\..\\..\\Application Code", "options.ini"));

        public void interperate(string code_part, string[] textArray = null, string fileloc = null, int lineNumber = -1)
        {
            string endOfStatementCode = ")";
            
            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons"))
                {
                    endOfStatementCode.Equals(line.EndsWith("true") ? endOfStatementCode = ");" : endOfStatementCode = ")");
                    break;
                }
            }
            string code_part_unedited = code_part;
            string textToPrint;

            code_part = code_part.TrimStart();
            bool foundUsing = false;
            
            if (code_part.StartsWith("print("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to print()");
                    Console.ResetColor();
                    return;
                }
                
                foreach (string x in someLINEs)
                {
                    if (x.TrimStart().TrimEnd() == "using Console;")
                    {
                        foundUsing = true;
                        break;
                    }
                    if (x.TrimStart().TrimEnd() == code_part)
                    {
                        break;
                    }
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'print' without its reference  (Use Console.print(\"*Text To Print*\") to fix this error :)");
                    Console.ResetColor();
                    return;
                }
            }
            else if (code_part.StartsWith($"Console.print(")) { }

            if (code_part_unedited.StartsWith($"Console.print("))
                code_part = code_part.Substring(14);
            else if (code_part_unedited.StartsWith($"print("))
                code_part = code_part.Substring(6);

            if (endOfStatementCode == ")")
                code_part = code_part.Substring(0, code_part.Length - 2);
            else
                code_part = code_part.Substring(0, code_part.Length - 1);

            textToPrint = code_part;
            bool isAString = false;
            
            int textToPrint_int;
            bool isAnInt = false;
            bool isAnInt2 = false;

            double textToPrint_double;
            bool isADouble = false;
            bool isADouble2 = false;
            
            isAString = ((textToPrint.StartsWith("\"") || textToPrint.StartsWith("cl\"")) && textToPrint.EndsWith("\""));
            if (textToPrint == "")
            {
                Console.WriteLine("");
                return;
            }

            string num1_toCheck = null;
            string num2_toCheck = null;
            try
            {
                num1_toCheck = textToPrint.Substring(0, (textToPrint.Length / 2 + 1) - 1);
                num2_toCheck = textToPrint.Substring((textToPrint.Length / 2 - 1) + 2);
                num1_toCheck = num1_toCheck.TrimEnd().TrimStart();
                num2_toCheck = num2_toCheck.TrimEnd().TrimStart();

                isAnInt = int.TryParse(num1_toCheck, out textToPrint_int);
                isAnInt2 = int.TryParse(num2_toCheck, out textToPrint_int);

                isADouble = double.TryParse(num1_toCheck, out textToPrint_double);
                isADouble2 = double.TryParse(num2_toCheck, out textToPrint_double);
            }
            catch
            {
                
            }


            if (textToPrint == "Time.Now")
            {
                Console.WriteLine(DateTime.Now);
            }
            else if (!isAString && (isAnInt || isADouble) && !textToPrint.Contains("+") && !textToPrint.Contains("-") && !textToPrint.Contains("*") && !textToPrint.Contains("/") && !textToPrint.Contains("%") && !textToPrint.Contains("=="))
            {
                Console.WriteLine(textToPrint.ToString());
            }
            else if (!isAString && (!isAnInt && !isADouble) && textToPrint.TrimEnd().TrimStart().StartsWith("true") || textToPrint.TrimEnd().TrimStart().StartsWith("false")) {
                try {
                    Console.WriteLine(Convert.ToBoolean(textToPrint.TrimEnd().TrimStart()));
                }
                catch {
                    ExceptionSender ExSend = new ExceptionSender();
                    string[] errorText = {
                        $"ERROR; Trying to convert \"{textToPrint}\" to a boolean failed!\n"
                    };
                    ExSend.SendException("0x000BC3", errorText);
                }
            }
            else if (textToPrint.StartsWith("random.range(") && textToPrint.EndsWith(")"))
            {
                //Random range is just a random function, it will automatically get converted to a string cuz why not :)
                bool isAString2 = textToPrint.Substring(0, textToPrint.Length).Substring(13).StartsWith("\"") && textToPrint.Substring(0, textToPrint.Length).Substring(13).EndsWith("\"");
                if (isAString2)
                {
                    ExceptionSender exceptionSender = new ExceptionSender();
                    exceptionSender.SendException("0x0000B3");
                    return;
                }
                string text = textToPrint;
                text = text.Substring(13);
                text = text.Substring(0, text.Length - 1);
                int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                Random rnd = new Random();
                Console.WriteLine(rnd.Next(number1, number2));
            }
            
            //=======================START OF MATH FUNCTIONS==========================\\
            else if (textToPrint.Contains("+") && textToPrint.Count(f => (f == '+')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, (textToPrint.Length / 2 + 1) - 1);
                var num2 = textToPrint.Substring((textToPrint.Length / 2 - 1) + 2);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var number1 = 0.0;
                var number2 = 0.0;

                var contentInFile = "<null>";
                var contentInFile2 = "<null>";

                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
                {
                    if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                    {
                        string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                        foreach (string file in files)
                        {
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num1)
                            {
                                contentInFile = File.ReadAllText(file);
                            }
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num2)
                            {
                                contentInFile2 = File.ReadAllText(file);
                            }
                        }
                    }
                }

                if (!isAnInt && !isADouble && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(contentInFile);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(contentInFile);
                }
                else
                {
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(num1);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(num1);
                }
                if (!isAnInt2 && !isADouble2 && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile2, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile2, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number2 = Convert.ToInt32(contentInFile2);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(contentInFile2);
                }
                else
                {
                    if (isAnInt2 && !isADouble2) //if the 2nd integer is an Integer and not a decimal
                        number2 = Convert.ToInt32(num2);
                    else if ((!isAnInt2 || isAnInt2) && isADouble2)//if the Second number is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(num2);
                }

                var result = number1 + number2;
                Console.WriteLine(result);
            }
            else if (textToPrint.Contains("-") && textToPrint.Count(f => (f == '-')) == 1 && !isAString && (isAnInt && isADouble))
            {
                var num1 = textToPrint.Substring(0, (textToPrint.Length / 2 + 1) - 1);
                var num2 = textToPrint.Substring((textToPrint.Length / 2 - 1) + 2);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var number1 = 0.0;
                var number2 = 0.0;

                var contentInFile = "<null>";
                var contentInFile2 = "<null>";

                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
                {
                    if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                    {
                        string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                        foreach (string file in files)
                        {
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num1)
                            {
                                contentInFile = File.ReadAllText(file);
                            }
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num2)
                            {
                                contentInFile2 = File.ReadAllText(file);
                            }
                        }
                    }
                }

                if (!isAnInt && !isADouble && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(contentInFile);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(contentInFile);
                }
                else
                {
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(num1);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(num1);
                }
                if (!isAnInt2 && !isADouble2 && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile2, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile2, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number2 = Convert.ToInt32(contentInFile2);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(contentInFile2);
                }
                else
                {
                    if (isAnInt2 && !isADouble2) //if the 2nd integer is an Integer and not a decimal
                        number2 = Convert.ToInt32(num2);
                    else if ((!isAnInt2 || isAnInt2) && isADouble2)//if the Second number is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(num2);
                }

                var result = number1 - number2;
                Console.WriteLine(result);
            }
            else if (textToPrint.Contains("*") && textToPrint.Count(f => (f == '*')) == 1 && !isAString && (isAnInt && isADouble))
            {
                var num1 = textToPrint.Substring(0, (textToPrint.Length / 2 + 1) - 1);
                var num2 = textToPrint.Substring((textToPrint.Length / 2 - 1) + 2);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var number1 = 0.0;
                var number2 = 0.0;

                var contentInFile = "<null>";
                var contentInFile2 = "<null>";

                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
                {
                    if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                    {
                        string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                        foreach (string file in files)
                        {
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num1)
                            {
                                contentInFile = File.ReadAllText(file);
                            }
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num2)
                            {
                                contentInFile2 = File.ReadAllText(file);
                            }
                        }
                    }
                }

                if (!isAnInt && !isADouble && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(contentInFile);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(contentInFile);
                }
                else
                {
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(num1);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(num1);
                }
                if (!isAnInt2 && !isADouble2 && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile2, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile2, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number2 = Convert.ToInt32(contentInFile2);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(contentInFile2);
                }
                else
                {
                    if (isAnInt2 && !isADouble2) //if the 2nd integer is an Integer and not a decimal
                        number2 = Convert.ToInt32(num2);
                    else if ((!isAnInt2 || isAnInt2) && isADouble2)//if the Second number is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(num2);
                }

                var result = number1 * number2;
                Console.WriteLine(result);
            }
            else if (textToPrint.Contains("/") && textToPrint.Count(f => (f == '/')) == 1 && !isAString && (isAnInt && isADouble))
            {
                var num1 = textToPrint.Substring(0, (textToPrint.Length / 2 + 1) - 1);
                var num2 = textToPrint.Substring((textToPrint.Length / 2 - 1) + 2);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var number1 = 0.0;
                var number2 = 0.0;

                var contentInFile = "<null>";
                var contentInFile2 = "<null>";

                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
                {
                    if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                    {
                        string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                        foreach (string file in files)
                        {
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num1)
                            {
                                contentInFile = File.ReadAllText(file);
                            }
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num2)
                            {
                                contentInFile2 = File.ReadAllText(file);
                            }
                        }
                    }
                }

                if (!isAnInt && !isADouble && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(contentInFile);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(contentInFile);
                }
                else
                {
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(num1);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(num1);
                }
                if (!isAnInt2 && !isADouble2 && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile2, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile2, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number2 = Convert.ToInt32(contentInFile2);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(contentInFile2);
                }
                else
                {
                    if (isAnInt2 && !isADouble2) //if the 2nd integer is an Integer and not a decimal
                        number2 = Convert.ToInt32(num2);
                    else if ((!isAnInt2 || isAnInt2) && isADouble2)//if the Second number is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(num2);
                }

                var result = number1 / number2;
                Console.WriteLine(result);
            }
            else if (textToPrint.Contains("%") && textToPrint.Count(f => (f == '%')) == 1 && !isAString && (isAnInt && isADouble))
            {
                var num1 = textToPrint.Substring(0, (textToPrint.Length / 2 + 1) - 1);
                var num2 = textToPrint.Substring((textToPrint.Length / 2 - 1) + 2);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var number1 = 0.0;
                var number2 = 0.0;

                var contentInFile = "<null>";
                var contentInFile2 = "<null>";

                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
                {
                    if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                    {
                        string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                        foreach (string file in files)
                        {
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num1)
                            {
                                contentInFile = File.ReadAllText(file);
                            }
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == num2)
                            {
                                contentInFile2 = File.ReadAllText(file);
                            }
                        }
                    }
                }

                if (!isAnInt && !isADouble && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(contentInFile);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(contentInFile);
                }
                else
                {
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number1 = Convert.ToInt32(num1);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number1 = Convert.ToDouble(num1);
                }
                if (!isAnInt2 && !isADouble2 && !isAString)
                {
                    isAnInt = int.TryParse(contentInFile2, out textToPrint_int);
                    isADouble = double.TryParse(contentInFile2, out textToPrint_double);
                    if (isAnInt && !isADouble) //if is an Integer but not a decimal
                        number2 = Convert.ToInt32(contentInFile2);
                    else if ((!isAnInt || isAnInt) && isADouble) //if is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(contentInFile2);
                }
                else
                {
                    if (isAnInt2 && !isADouble2) //if the 2nd integer is an Integer and not a decimal
                        number2 = Convert.ToInt32(num2);
                    else if ((!isAnInt2 || isAnInt2) && isADouble2)//if the Second number is a decimal regardless of if it is an int/not
                        number2 = Convert.ToDouble(num2);
                }

                var result = number1 % number2;
                Console.WriteLine(result);
            }
            //=======================END OF MATH FUNCTIONS==========================\\
            else if (textToPrint.Contains("==") && textToPrint.Count(f => (f == '=')) == 2)
            {
                //basically an if statement in a print statement and the result is either true or false, nothing in between or there will be an error
                var comparer = textToPrint.Substring(0, textToPrint.IndexOf("==") - 1);
                var comparee = textToPrint.Substring(textToPrint.IndexOf("==") + 2);
                comparer = comparer.TrimEnd().TrimStart();
                comparee = comparee.TrimEnd().TrimStart();
                var result = comparer == comparee;
                Console.WriteLine(result);
            }
            else if (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\"") && isAString && !isAnInt)
            {
                Console.WriteLine(textToPrint.Substring(0, textToPrint.Length - 1).Substring(1));
            }
            else if (textToPrint.StartsWith("cl\"") && textToPrint.EndsWith("\"") && isAString && !isAnInt)
            {
                textToPrint = textToPrint.Substring(2).TrimStart();
                Console.Write(textToPrint.Substring(0, textToPrint.Length - 1).Substring(1));
            }
            else if (!isAString && !isAnInt)
            {
                if (textToPrint.StartsWith("\"") && !textToPrint.EndsWith("\"") || !textToPrint.StartsWith("\"") && textToPrint.EndsWith("\""))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Mismatching Double Quotes for printing a string");
                    Console.WriteLine("     Error at line: " + lineNumber);
                    Console.WriteLine("     The Line with Error: " + code_part_unedited);
                    Console.ResetColor();
                    return;
                }
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\\EASY14_Variables_TEMP"))
                {
                    if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\\EASY14_Variables_TEMP").Length != 0)
                    {
                        string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP");
                        foreach (string file in files)
                        {
                            if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == textToPrint)
                            {
                                var contentInFile = File.ReadAllText(file);
                                Console.WriteLine(contentInFile.ToString());
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                ThrowErrorMessage tErM = new ThrowErrorMessage();
                string unknownLine = "<unknownLineNumber>";
                var returnLineNumber = lineNumber > -1 ? lineNumber.ToString() : unknownLine;
                string[] errorText = {" Your syntax/parameters were incorrect!", $"  at line {returnLineNumber}", $"at line {code_part_unedited}\n"};
                tErM.sendErrMessage(null, errorText, "error");
            }
            //}
        }
    }
}
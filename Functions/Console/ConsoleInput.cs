using System;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    class ConsoleInput
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));
        
        public void Interperate(string code_part, string[] textArray, string fileloc, string varName, int lineNumber = -1)
        {
            string endOfStatementCode = ")";
            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons")) {
                    endOfStatementCode.Equals(line.EndsWith("true") ? endOfStatementCode = ");" : endOfStatementCode = ")");
                }
                break;
            }

            string code_part_unedited = code_part;
            string textToPrint;
            
            bool foundUsing = false;
            if (code_part.StartsWith($"input("))
            {
                string[] someLines = null;
                if (textArray == null && fileloc != null) someLines = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLines = textArray;
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to input()");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                foreach (string x in someLines)
                {
                    if (x == code_part)
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
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'input' without its reference  (Use Console.input(\"*Text*\") to fix this error :)");
                    Console.ResetColor();
                    return;
                }
            }
            else if (code_part.StartsWith($"Console.input(")) { }

            if (code_part.IndexOf("=") > 0)
            {
                code_part = code_part.Substring(code_part.IndexOf("="));
            }
            code_part = code_part_unedited.TrimStart();
            code_part = code_part.TrimStart();
            if (code_part.StartsWith("Console."))
                code_part = code_part.Substring(14);
            else
                code_part = code_part.Substring(6);

            code_part = code_part.Substring(0, code_part.Length - 1);
            if (code_part.EndsWith(")"))
            {
                code_part = code_part.Substring(0, code_part.Length - 1);
            }

            textToPrint = code_part;
            if (code_part == "")
            {
                Console.Write(">");
                Console.ReadLine();
                return;
            }
            bool isAString = false;

            int textToPrint_int;
            bool isAnInt = false;
            bool isAnInt2 = false;

            double textToPrint_double;
            bool isADouble = false;
            bool isADouble2 = false;
            /*if (foundUsing == true)
            {*/
            isAString = (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\""));
            //Console.WriteLine(textToPrint.Length / 2);
            var num1_toCheck = textToPrint.Substring(0, (textToPrint.Length / 2 + 1) - 1);
            var num2_toCheck = textToPrint.Substring((textToPrint.Length / 2 - 1) + 2);
            num1_toCheck = num1_toCheck.TrimEnd().TrimStart();
            num2_toCheck = num2_toCheck.TrimEnd().TrimStart();

            isAnInt = int.TryParse(num1_toCheck, out textToPrint_int);
            isAnInt2 = int.TryParse(num2_toCheck, out textToPrint_int);

            isADouble = double.TryParse(num1_toCheck, out textToPrint_double);
            isADouble2 = double.TryParse(num2_toCheck, out textToPrint_double);

            if (foundUsing == true && code_part_unedited.StartsWith("input") || foundUsing == false && code_part_unedited.StartsWith("Console.input"))
            {
                if (textToPrint == "Time.Now")
                {
                    Console.WriteLine(DateTime.Now);
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                }
                else if (!isAString && (isAnInt || isADouble) && !textToPrint.Contains("+") && !textToPrint.Contains("-") && !textToPrint.Contains("*") && !textToPrint.Contains("/") && !textToPrint.Contains("%") && !textToPrint.Contains("=="))
                {
                    Console.WriteLine(textToPrint.ToString());
                }
                else if (!isAString && (!isAnInt && !isADouble) && textToPrint.TrimEnd().TrimStart().StartsWith("true") || textToPrint.TrimEnd().TrimStart().StartsWith("false"))
                {
                    try
                    {
                        Console.WriteLine(Convert.ToBoolean(textToPrint.TrimEnd().TrimStart()));
                    }
                    catch
                    {
                        ExceptionSender ExSend = new ExceptionSender();
                        string[] errorText = {
                        $"ERROR; Trying to convert \"{textToPrint}\" to a boolean failed!\n"
                    };
                        ExSend.SendException("0x000BC3", errorText);
                    }
                }
                else if (textToPrint.StartsWith("random.range("))
                {
                    string text = textToPrint;
                    text = text.Replace("random.range(", "");
                    text = text[..^1];
                    int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                    int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                    Random rnd = new Random(); Console.WriteLine(rnd.Next(number1, number2));
                }
                else if (textToPrint.Contains("+") && textToPrint.Count(f => (f == '+')) == 1 && !isAString /*&& ((isAnInt || isAnInt2) && (isADouble || isADouble2))*/)
                {
                    Math_Add math_Add = new Math_Add(); var result = math_Add.Interperate(textToPrint, -1);
                    Console.WriteLine(result); Console.Write(">");
                    string textFromUser = Console.ReadLine();
                }
                else if (textToPrint.Contains("-") && textToPrint.Count(f => (f == '-')) == 1 && !isAString && (isAnInt && isADouble))
                {
                    Math_Subtract math_Subtract = new Math_Subtract(); var result = math_Subtract.Interperate(textToPrint, -1);
                    Console.WriteLine(result); Console.Write(">");
                    string textFromUser = Console.ReadLine();
                }
                else if (textToPrint.Contains("*") && textToPrint.Count(f => (f == '*')) == 1 && !isAString && (isAnInt && isADouble))
                {
                    Math_Multiply math_Multiply = new Math_Multiply(); var result = math_Multiply.Interperate(textToPrint, -1);
                    Console.WriteLine(result); Console.Write(">");
                    string textFromUser = Console.ReadLine();
                }
                else if (textToPrint.Contains("/") && textToPrint.Count(f => (f == '/')) == 1 && !isAString && (isAnInt && isADouble))
                {
                    Math_Divide math_Divide = new Math_Divide(); var result = math_Divide.Interperate(textToPrint, -1);
                    Console.WriteLine(result); Console.Write(">");
                    string textFromUser = Console.ReadLine();
                }
                else if (textToPrint.Contains("%") && textToPrint.Count(f => (f == '%')) == 1 && !isAString && (isAnInt && isADouble))
                {
                    Math_Module math_Module = new Math_Module(); var result = math_Module.Interperate(textToPrint, -1);
                    Console.WriteLine(result); Console.Write(">");
                    string textFromUser = Console.ReadLine();
                }
                else if (textToPrint.Contains("==") && textToPrint.Count(f => (f == '=')) == 2)
                {
                    Math_Equals math_Equals = new Math_Equals(); var result = math_Equals.Interperate(textToPrint, -1);
                    Console.WriteLine(result); Console.Write(">");
                    string textFromUser = Console.ReadLine();
                }
                else if (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\"") && isAString && !isAnInt)
                {
                    Console.WriteLine(textToPrint.Substring(0, textToPrint.Length - 1).Substring(1));
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                }
                else if (!isAString && !isAnInt)
                {
                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP"))
                    {
                        if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                        {
                            string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP");
                            foreach (string file in files)
                            {
                                if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == textToPrint)
                                {
                                    var contentInFile = File.ReadAllText(file);
                                    Console.WriteLine(contentInFile.ToString());
                                    Console.Write(">");
                                    string textFromUser = Console.ReadLine();
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
                    string[] errorText = { " Your syntax/parameters were incorrect!", $"  at line {returnLineNumber}", $"at line {code_part_unedited}\n" };
                    tErM.sendErrMessage(null, errorText, "error");
                }
            }
        }
    }
}
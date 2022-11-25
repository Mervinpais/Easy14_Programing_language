using System;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    public static class ConsoleInput_OLD
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static string Interperate(string code_part, string[] lines, string[] textArray = null, string fileloc = null, string varName = null)
        {
            string code_part_unedited = code_part;
            string textToPrint;

            if (code_part.IndexOf("=") > 0)
            {
                code_part = code_part.Substring(varName.Length + 3);
            }
            code_part = code_part.TrimStart();

            bool foundUsing = false;
            if (code_part.StartsWith($"input("))
            {
                string[] someLines = null;
                if (textArray == null && fileloc != null) someLines = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLines = textArray;
                else someLines = lines;

                foreach (string x in someLines)
                {
                    if (x == code_part)
                    {
                        break;
                    }
                    if (x.StartsWith("using") || x.StartsWith("from"))
                    {
                        if (x == "using Console;" || x == "from Console get input;")
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
                    return "";
                }
            }
            else if (code_part.StartsWith($"Console.input(")) { foundUsing = true; }

            code_part_unedited = code_part;
            if (code_part.StartsWith("Console."))
                code_part = code_part.Substring(14);
            else
                code_part = code_part.Substring(6);

            code_part = code_part.Substring(0, code_part.Length - 2);

            textToPrint = code_part;
            if (code_part == "")
            {
                Console.Write(">");
                string textFromUser = Console.ReadLine();
                return textFromUser;
            }
            bool isAString = false;

            int textToPrint_int;
            bool isAnInt = false;
            bool isAnInt2 = false;

            double textToPrint_double;
            bool isADouble = false;
            bool isADouble2 = false;

            isAString = (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\""));

            var num1_toCheck = textToPrint.Substring(0, (textToPrint.Length / 2 + 1) - 1);
            var num2_toCheck = textToPrint.Substring((textToPrint.Length / 2 - 1) + 2);
            num1_toCheck = num1_toCheck.TrimEnd().TrimStart();
            num2_toCheck = num2_toCheck.TrimEnd().TrimStart();

            isAnInt = int.TryParse(num1_toCheck, out textToPrint_int);
            isAnInt2 = int.TryParse(num2_toCheck, out textToPrint_int);

            isADouble = double.TryParse(num1_toCheck, out textToPrint_double);
            isADouble2 = double.TryParse(num2_toCheck, out textToPrint_double);

            if (foundUsing == true && code_part_unedited.StartsWith("input") || code_part_unedited.StartsWith("Console.input"))
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
                        string[] errorText = {
                        $"ERROR; Trying to convert \"{textToPrint}\" to a boolean failed!\n"
                        };
                        ExceptionSender.SendException("0x000BC3", errorText);
                    }
                }
                else if (textToPrint.StartsWith("random.range("))
                {
                    string text = textToPrint;
                    text = text.Replace("random.range(", "");
                    text = text[..^1];
                    int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                    int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                    Random rnd = new Random();
                    Console.WriteLine(rnd.Next(number1, number2));
                    string textFromUser = Console.ReadLine();
                    return textFromUser;
                }
                else if (textToPrint.Contains("+") && textToPrint.Count(f => (f == '+')) == 1 && !isAString /*&& ((isAnInt || isAnInt2) && (isADouble || isADouble2))*/)
                {
                    Console.WriteLine(Math_Add.Interperate(textToPrint, -1));
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                    return textFromUser;
                }
                else if (textToPrint.Contains("-") && textToPrint.Count(f => (f == '-')) == 1 && !isAString && (isAnInt && isADouble))
                {
                    Console.WriteLine(Math_Subtract.Interperate(textToPrint, -1));
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                    return textFromUser;
                }
                else if (textToPrint.Contains("*") && textToPrint.Count(f => (f == '*')) == 1 && !isAString && (isAnInt && isADouble))
                {
                    Console.WriteLine(Math_Multiply.Interperate(textToPrint, -1));
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                    return textFromUser;
                }
                else if (textToPrint.Contains("/") && textToPrint.Count(f => (f == '/')) == 1 && !isAString && (isAnInt && isADouble))
                {
                    Console.WriteLine(Math_Divide.Interperate(textToPrint, -1));
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                    return textFromUser;
                }
                else if (textToPrint.Contains("%") && textToPrint.Count(f => (f == '%')) == 1 && !isAString && (isAnInt && isADouble))
                {
                    Console.WriteLine(Math_Module.Interperate(textToPrint, -1));
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                    return textFromUser;
                }
                else if (textToPrint.Contains("==") && textToPrint.Count(f => (f == '=')) == 2)
                {
                    Console.WriteLine(isEqualTo.Interperate(textToPrint));
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                    return textFromUser;
                }
                else if (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\"") && isAString && !isAnInt)
                {
                    Console.WriteLine(textToPrint.Substring(0, textToPrint.Length - 1).Substring(1));
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                    return textFromUser;
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
                                    return textFromUser;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.Write(">");
                    string textFromUser = Console.ReadLine();
                    return textFromUser;
                    /*
                    ThrowErrorMessage tErM = new ThrowErrorMessage();
                    string unknownLine = "<unknownLineNumber>";
                    var returnLineNumber = lineNumber > -1 ? lineNumber.ToString() : unknownLine;
                    string[] errorText = { " Your syntax/parameters were incorrect!", $"  at line {returnLineNumber}", $"at line {code_part_unedited}\n" };
                    tErM.sendErrMessage(null, errorText, "error");
                    */
                }
            }
            return "";
        }
    }
}
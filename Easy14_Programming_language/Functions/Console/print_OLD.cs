using System;
using System.Data;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    public static class ConsolePrint_OLD
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static void Interperate(string code_part, string[] textArray = null, string fileloc = null, int lineNumber = -1)
        {
            string code_part_unedited = code_part;
            string textToPrint;

            code_part = code_part.TrimStart();

            bool foundUsing = false;
            if (code_part.StartsWith("print("))
            {
                string[] Lines = null;
                if (textArray == null && fileloc != null) Lines = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) Lines = textArray;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to print()");
                    Console.ResetColor();
                    return;
                }

                foreach (string x in Lines)
                {
                    if (x.TrimStart().TrimEnd() == "using Console;" || x.TrimStart().TrimEnd() == "from Console get print;")
                    {
                        foundUsing = true;
                        break;
                    }
                    else if (x.TrimStart().TrimEnd() == code_part) { break; }
                }

                if (!foundUsing)
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

            code_part = code_part.Substring(0, code_part.Length - 1);
            if (code_part.EndsWith(")")) code_part = code_part.Substring(0, code_part.Length - 1);

            textToPrint = code_part;
            bool isAString = false;

            int textToPrint_int;
            bool isAnInt = false;
            bool isAnInt2 = false;

            double textToPrint_double;
            bool isADouble = false;
            bool isADouble2 = false;

            isAString = ((textToPrint.StartsWith("\"") || textToPrint.StartsWith("cl\"")) && textToPrint.EndsWith("\"") || textToPrint.EndsWith("cl\""));

            if (textToPrint == "") { Console.WriteLine(""); return; }

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


            if (textToPrint == "Time.Now") Console.WriteLine(DateTime.Now);
            else if (!isAString && (isAnInt || isADouble) && !textToPrint.Contains("+") && !textToPrint.Contains("-") && !textToPrint.Contains("*") && !textToPrint.Contains("/") && !textToPrint.Contains("%") && !textToPrint.Contains("=="))
            { Console.WriteLine(textToPrint.ToString()); }
            else if (!isAString && (!isAnInt && !isADouble) && textToPrint.TrimEnd().TrimStart().StartsWith("true") || textToPrint.TrimEnd().TrimStart().StartsWith("false"))
            {
                try { Console.WriteLine(Convert.ToBoolean(textToPrint.TrimEnd().TrimStart())); }
                catch
                {
                    string[] errorText = { $"ERROR; Trying to convert \"{textToPrint}\" to a boolean failed!\n" };
                    ExceptionSender.SendException("0x000BC3", errorText);
                }
            }
            else if (textToPrint.StartsWith("random.range(") && textToPrint.EndsWith(")"))
            {
                //Random range is just a random function, it will automatically get converted to a string cuz why not :)
                bool isAString2 = textToPrint.Substring(0, textToPrint.Length).Substring(13).StartsWith("\"") && textToPrint.Substring(0, textToPrint.Length).Substring(13).EndsWith("\"");
                if (isAString2) { ExceptionSender.SendException("0x0000B3"); return; }
                string text = textToPrint;
                text = text.Substring(13);
                text = text.Substring(0, text.Length - 1);
                int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                Random rnd = new Random();
                Console.WriteLine(rnd.Next(number1, number2));
            }

            //=======================START OF MATH FUNCTIONS==========================\\
            else if (textToPrint.Contains("+") || textToPrint.Contains("-") || textToPrint.Contains("/") || textToPrint.Contains("*") || textToPrint.Contains("%"))
            {
                textToPrint = textToPrint.Substring(0, textToPrint.Length - 1);
                try
                {
                    double result = Convert.ToDouble(new DataTable().Compute(textToPrint, null));
                    Console.WriteLine(Convert.ToDouble(new DataTable().Compute(textToPrint, null)));
                }
                catch
                {
                    ThrowErrorMessage.sendErrMessage($"Uh oh, the value you wanted to calculate won't work! (check if the value is a string and change it to an integer)", null, "error");
                }
            }
            else if (textToPrint.Contains("+") && textToPrint.Count(f => (f == '+')) == 1 && !isAString)
            {
                Console.WriteLine(Math_Add.Interperate(textToPrint, -1));
            }
            else if (textToPrint.Contains("-") && textToPrint.Count(f => (f == '-')) == 1 && !isAString && (isAnInt && isADouble))
            {
                Console.WriteLine(Math_Subtract.Interperate(textToPrint, -1));
            }
            else if (textToPrint.Contains("*") && textToPrint.Count(f => (f == '*')) == 1 && !isAString && (isAnInt && isADouble))
            {
                Console.WriteLine(Math_Multiply.Interperate(textToPrint, -1));
            }
            else if (textToPrint.Contains("/") && textToPrint.Count(f => (f == '/')) == 1 && !isAString && (isAnInt && isADouble))
            {
                Console.WriteLine(Math_Divide.Interperate(textToPrint, -1));
            }
            else if (textToPrint.Contains("%") && textToPrint.Count(f => (f == '%')) == 1 && !isAString && (isAnInt && isADouble))
            {
                Console.WriteLine(Math_Module.Interperate(textToPrint, -1));
            }
            //=======================END OF MATH FUNCTIONS==========================\\
            else if (textToPrint.Contains("==") && textToPrint.Count(f => (f == '=')) == 2)
            {
                Console.WriteLine(isEqualTo.Interperate(textToPrint));
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
                string unknownLine = "<unknownLineNumber>";
                var returnLineNumber = lineNumber > -1 ? lineNumber.ToString() : unknownLine;
                string[] errorText = { " Your syntax/parameters were incorrect!", $"  at line {returnLineNumber}", $"at line {code_part_unedited}\n" };
                ThrowErrorMessage.sendErrMessage(null, errorText, "error");
            }
            //}
        }
    }
}
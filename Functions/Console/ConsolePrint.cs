using System;
using System.IO;
using System.Linq;

namespace Easy14_Coding_Language
{
    class ConsolePrint
    {
        private string endOfStatementCode_;
        public string endOfStatementCode
        {
            get { return endOfStatementCode_; }
            set { endOfStatementCode_ = value; }
        }

        public void interperate(string code_part, string[] textArray, string fileloc, int lineNumber = -1)
        {
            string code_part_unedited;
            string textToPrint;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();
            bool foundUsing = false;
            if (code_part.StartsWith($"print("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
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
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                }
            }
            else if (code_part.StartsWith($"Console.print(")) { }

            if (code_part_unedited.StartsWith($"Console.print("))
                code_part = code_part.Substring(14);
            else if (code_part_unedited.StartsWith($"print("))
                code_part = code_part.Substring(6);

            if (endOfStatementCode == ");")
                code_part = code_part.Substring(0, code_part.Length - 2);
            else
                code_part = code_part.Substring(0, code_part.Length - 1);

            //code_part = code_part.Substring(0, code_part.Length - 1);
            textToPrint = code_part;
            bool isAString = false;
            bool isAnInt = false;
            /*if (foundUsing == true)
            {*/
            if (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\""))
            {
                isAString = true;
            }
            int textToPrint_int;
            isAnInt = int.TryParse(textToPrint, out textToPrint_int);

            

            if (textToPrint == "Time.Now")
            {
                Console.WriteLine(DateTime.Now);
            }

            else if (textToPrint.StartsWith("random.range(") && textToPrint.EndsWith(")"))
            {
                //Random range is just a random function, it will automatically get converted to a string cuz why not :)
                bool isAString2 = textToPrint.Substring(0, textToPrint.Length).Substring(13).StartsWith("\"") && textToPrint.Substring(0, textToPrint.Length).Substring(13).EndsWith("\"");
                if (isAString2)
                {
                    ExceptionSender exceptionSender = new ExceptionSender();
                    exceptionSender.SendException(0x0000B3);
                    return;
                }
                string text = textToPrint;
                text = text.Replace("random.range(", "").Replace(")", "");
                int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                Random rnd = new Random();
                Console.WriteLine(rnd.Next(number1, number2));
            }
            
            //=======================START OF MATH FUNCTIONS==========================\\
            else if (textToPrint.Contains("+") && textToPrint.Count(f => (f == '+')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("+") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("+") + 2);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var sum = Convert.ToInt32(num1) + Convert.ToInt32(num2);
                Console.WriteLine(sum);
            }
            else if (textToPrint.Contains("-") && textToPrint.Count(f => (f == '-')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("-") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("-") + 1);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var difference = Convert.ToInt32(num1) - Convert.ToInt32(num2);
                Console.WriteLine(difference);
            }
            else if (textToPrint.Contains("*") && textToPrint.Count(f => (f == '*')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("*") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("*") + 1);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var result = Convert.ToInt32(num1) * Convert.ToInt32(num2);
                Console.WriteLine(result);
            }
            else if (textToPrint.Contains("/") && textToPrint.Count(f => (f == '/')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("/") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("/") + 1);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var result = Convert.ToInt32(num1) / Convert.ToInt32(num2);
                Console.WriteLine(result);
            }
            else if (textToPrint.Contains("%") && textToPrint.Count(f => (f == '%')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("%") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("%") + 1);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var result = Convert.ToInt32(num1) / Convert.ToInt32(num2);
                Console.WriteLine(result);
            }
            //=======================END OF MATH FUNCTIONS==========================\\
            else if (textToPrint.Contains("==") && textToPrint.Count(f => (f == '=')) == 2 && !isAString)
            {
                //basically an if statement in a print statement and the result is either true or false, nothing in between or there will be an error
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("==") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("==") + 2);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var result = Convert.ToInt32(num1) == Convert.ToInt32(num2);
                Console.WriteLine(result);
            }
            else if (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\""))
            //example; print("Test");, we just detect if it contains Double Quotes " in the parameter
            {
                Console.WriteLine(textToPrint.Replace('"'.ToString(), ""));
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
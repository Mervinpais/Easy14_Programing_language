using System;
using System.IO;
using System.Linq;

namespace Easy14_Coding_Language
{
    class ConsoleExec
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
            if (code_part.StartsWith($"exec("))
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
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'exec' without its reference  (Use Console.exec(\"*Text To Execute*\") to fix this error :)");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                }
            }
            else if (code_part.StartsWith($"Console.exec(")) { }

            if (code_part_unedited.StartsWith($"Console.exec("))
                code_part = code_part.Substring(13);
            else if (code_part_unedited.StartsWith($"exec("))
                code_part = code_part.Substring(5);

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

            Program prog = new Program();

            if (textToPrint == "Time.Now")
            {
                //Console.WriteLine(DateTime.Now);
                string[] execArray = {DateTime.Now.ToString()};
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\""))
            {
                string[] execArray = { textToPrint.TrimEnd().TrimStart().Substring(1).Substring(0, textToPrint.Length - 2).ToString() };
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.StartsWith("random.range("))
            {
                string text = textToPrint;
                text = text.Replace("random.range(", "").Replace(")", "");
                int number1 = Convert.ToInt32(text.Substring(0, text.IndexOf(",")).Replace(",", ""));
                int number2 = Convert.ToInt32(text.Substring(text.IndexOf(",")).Replace(",", ""));
                Random rnd = new Random();
                //Console.WriteLine(rnd.Next(number1, number2));
                string[] execArray = {rnd.Next(number1, number2).ToString()};
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("+") && textToPrint.Count(f => (f == '+')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("+") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("+") + 2);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var sum = Convert.ToInt32(num1) + Convert.ToInt32(num2);
                //Console.WriteLine(sum);
                string[] execArray = { sum.ToString() };
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("-") && textToPrint.Count(f => (f == '-')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("-") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("-") + 1);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var difference = Convert.ToInt32(num1) - Convert.ToInt32(num2);
                //Console.WriteLine(difference);
                string[] execArray = { difference.ToString() };
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("*") && textToPrint.Count(f => (f == '*')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("*") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("*") + 1);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var result = Convert.ToInt32(num1) * Convert.ToInt32(num2);
                //Console.WriteLine(result);
                string[] execArray = { result.ToString() };
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("/") && textToPrint.Count(f => (f == '/')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("/") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("/") + 1);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var result = Convert.ToInt32(num1) / Convert.ToInt32(num2);
                //Console.WriteLine(result);
                string[] execArray = { result.ToString() };
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("%") && textToPrint.Count(f => (f == '%')) == 1 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("%") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("%") + 1);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var result = Convert.ToInt32(num1) / Convert.ToInt32(num2);
                //Console.WriteLine(result);
                string[] execArray = { result.ToString() };
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("==") && textToPrint.Count(f => (f == '=')) == 2 && !isAString)
            {
                var num1 = textToPrint.Substring(0, textToPrint.IndexOf("==") - 1);
                var num2 = textToPrint.Substring(textToPrint.IndexOf("==") + 2);
                num1 = num1.TrimEnd().TrimStart();
                num2 = num2.TrimEnd().TrimStart();
                var result = Convert.ToInt32(num1) == Convert.ToInt32(num2);
                //Console.WriteLine(result);
                string[] execArray = { result.ToString() };
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith(endOfStatementCode == ")" ? "\"" : "\";"))
            {
                //Console.WriteLine(textToPrint.Replace('"'.ToString(), ""));
                string[] execArray = { textToPrint.Replace('"'.ToString(), "").ToString() };
                prog.compileCode_fromOtherFiles(null, execArray);
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
                                //Console.WriteLine(contentInFile.ToString());
                                string[] execArray = { contentInFile };
                                prog.compileCode_fromOtherFiles(null, execArray);
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
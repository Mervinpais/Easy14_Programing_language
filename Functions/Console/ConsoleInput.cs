using System;
using System.IO;
using System.Linq;

namespace Easy14_Coding_Language
{
    class ConsoleInput
    {
        private string endOfStatementCode_;
        public string endOfStatementCode
        {
            get { return endOfStatementCode_; }
            set { endOfStatementCode_ = value; }
        }
        public void interperate(string code_part, string[] textArray, string fileloc, string varName)
        {
            string code_part_unedited;
            string textToPrint;
            
            bool foundUsing = false;
            if (code_part.StartsWith($"input("))
            {
                string[] someLines = null;
                if (textArray == null && fileloc != null) someLines = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLines = textArray;

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
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                }
            }
            else if (code_part.StartsWith($"Console.input(")) { }

            code_part_unedited = code_part;
            code_part = code_part.Substring(code_part.IndexOf("="));
            code_part = code_part_unedited.TrimStart();
            code_part = code_part.Substring(10);

            if (endOfStatementCode == ");")
                code_part = code_part.Substring(0, code_part.Length - 3);
            else
                code_part = code_part.Substring(0, code_part.Length - 2);

            textToPrint = code_part;
            if (foundUsing == false)
            {
                if (textToPrint == "Time.Now")
                {
                    Console.WriteLine(DateTime.Now);
                }
                else if (textToPrint.StartsWith("random.range("))
                {
                    string text = textToPrint;
                    text = text.Replace("random.range(", "");
                    text = text.Replace(")", "");
                    int number1 = Convert.ToInt32(
                        text.Substring(0, text.IndexOf(",")).Replace(",", "")
                    );
                    int number2 = Convert.ToInt32(
                        text.Substring(text.IndexOf(",")).Replace(",", "")
                    );
                    Random rnd = new Random();
                    Console.WriteLine(rnd.Next(number1, number2));
                }
                else if (textToPrint.Contains("+") && textToPrint.Count(f => (f == '+')) == 1)
                {
                    var num1 = textToPrint.Substring(3, textToPrint.IndexOf("+") - 3);
                    var num2 = textToPrint.Substring(textToPrint.IndexOf("+") + 1);
                    num1 = num1.TrimEnd().TrimStart();
                    num2 = num2.TrimEnd().TrimStart();
                    var sum = Convert.ToInt32(num1) + Convert.ToInt32(num2);
                    Console.WriteLine(sum);
                }
                else if (textToPrint.Contains("-") && textToPrint.Count(f => (f == '-')) == 1)
                {
                    var num1 = textToPrint.Substring(3, textToPrint.IndexOf("-") - 3);
                    var num2 = textToPrint.Substring(textToPrint.IndexOf("-") + 1);
                    num1 = num1.TrimEnd().TrimStart();
                    num2 = num2.TrimEnd().TrimStart();
                    var difference = Convert.ToInt32(num1) - Convert.ToInt32(num2);
                    Console.WriteLine(difference);
                }
                else if (textToPrint.Contains("*") && textToPrint.Count(f => (f == '*')) == 1)
                {
                    var num1 = textToPrint.Substring(3, textToPrint.IndexOf("*") - 3);
                    var num2 = textToPrint.Substring(textToPrint.IndexOf("*") + 1);
                    num1 = num1.TrimEnd().TrimStart();
                    num2 = num2.TrimEnd().TrimStart();
                    var result = Convert.ToInt32(num1) * Convert.ToInt32(num2);
                    Console.WriteLine(result);
                }
                else if (textToPrint.Contains("/") && textToPrint.Count(f => (f == '/')) == 1)
                {
                    var num1 = textToPrint.Substring(3, textToPrint.IndexOf("/") - 3);
                    var num2 = textToPrint.Substring(textToPrint.IndexOf("/") + 1);
                    num1 = num1.TrimEnd().TrimStart();
                    num2 = num2.TrimEnd().TrimStart();
                    var result = Convert.ToInt32(num1) / Convert.ToInt32(num2);
                    Console.WriteLine(result);
                }
                else if (textToPrint.Contains("%") && textToPrint.Count(f => (f == '%')) == 1)
                {
                    var num1 = textToPrint.Substring(3, textToPrint.IndexOf("%") - 3);
                    var num2 = textToPrint.Substring(textToPrint.IndexOf("%") + 1);
                    num1 = num1.TrimEnd().TrimStart();
                    num2 = num2.TrimEnd().TrimStart();
                    var result = Convert.ToInt32(num1) / Convert.ToInt32(num2);
                    Console.WriteLine(result);
                }
                else if (textToPrint.Contains("==") && textToPrint.Count(f => (f == '=')) == 2)
                {
                    var num1 = textToPrint.Substring(3, textToPrint.IndexOf("==") - 4);
                    var num2 = textToPrint.Substring(textToPrint.IndexOf("==") + 2);
                    num1 = num1.TrimEnd().TrimStart();
                    num2 = num2.TrimEnd().TrimStart();
                    var result = Convert.ToInt32(num1) == Convert.ToInt32(num2);
                    Console.WriteLine(result);
                }
                else if (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\""))
                {
                    Console.WriteLine(textToPrint.Substring(1).Substring(0, textToPrint.Length - 3));
                }
                else if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP").Length != 0)
                {
                    string[] files = Directory.GetFiles(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                            + @$"\EASY14_Variables_TEMP"
                    );
                    foreach (string file in files)
                    {
                        if (file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == textToPrint.Replace(".txt", ""))
                        {
                            var contentInFile = File.ReadAllText(file);
                            Console.WriteLine(contentInFile.ToString());
                            break;
                        }
                    }
                }
                Console.Write(">");
                string textFromUser = Console.ReadLine();
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP\{varName}.txt", textFromUser);
            }
        }
    }
}
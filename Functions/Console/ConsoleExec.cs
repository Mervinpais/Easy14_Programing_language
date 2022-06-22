using System;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    class ConsoleExec
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(strWorkPath + "..\\..\\..\\..\\Application Code", "options.ini"));
        
        public void interperate(string code_part, string[] textArray, string fileloc, int lineNumber = -1)
        {
            string endOfStatementCode = ")";
            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons"))
                    endOfStatementCode.Equals(line.EndsWith("true") ? endOfStatementCode = ");" : endOfStatementCode = ")");
                break;
            }

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
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to exec()");
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
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'exec' without its reference  (Use Console.exec(\"*Text To Execute*\") to fix this error :)");
                    Console.ResetColor();
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
            string[] execArray = { textToPrint.ToString() };

            if (textToPrint == "Time.CurrentTime")
            {
                //Console.WriteLine(DateTime.Now);
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.StartsWith("\"") && textToPrint.EndsWith("\""))
            {
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.StartsWith("random.range("))
            {
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("+") && textToPrint.Count(f => (f == '+')) == 1 && !isAString)
            {
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("-") && textToPrint.Count(f => (f == '-')) == 1 && !isAString)
            {
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("*") && textToPrint.Count(f => (f == '*')) == 1 && !isAString)
            {
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("/") && textToPrint.Count(f => (f == '/')) == 1 && !isAString)
            {
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("%") && textToPrint.Count(f => (f == '%')) == 1 && !isAString)
            {
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.Contains("==") && textToPrint.Count(f => (f == '=')) == 2 && !isAString)
            {
                prog.compileCode_fromOtherFiles(null, execArray);
            }
            else if (textToPrint.StartsWith('"'.ToString()) && textToPrint.EndsWith(endOfStatementCode == ")" ? "\"" : "\";"))
            {
                //Console.WriteLine(textToPrint.Replace('"'.ToString(), ""));
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
                                string[] fContent = { contentInFile };
                                prog.compileCode_fromOtherFiles(null, fContent);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                try {

                }
                catch {
                    ThrowErrorMessage tErM = new ThrowErrorMessage();
                    string unknownLine = "<unknownLineNumber>";
                    var returnLineNumber = lineNumber > -1 ? lineNumber.ToString() : unknownLine;
                    string[] errorText = {" An error occurred while executing commands from the exec command", $"  at line {returnLineNumber}", $"at line {code_part_unedited}\n"};
                    tErM.sendErrMessage(null, errorText, "error");
                }
            }
            //}
        }
    }
}
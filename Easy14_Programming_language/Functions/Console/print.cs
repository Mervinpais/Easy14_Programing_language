using System;
using System.IO;
using System.Linq;
using Z.Expressions;
using Z.Expressions.Compiler.Shared;

namespace Easy14_Programming_Language
{
    public static class ConsolePrint
    {
        static readonly string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static readonly string strWorkPath = Path.GetDirectoryName(strExeFilePath);

        public static void Interperate(string code_part, string[] textArray = null, string fileloc = null, int lineNumber = -1)
        {
            string[] lines = Array.Empty<string>();
            if (textArray != null && fileloc == null)
            {
                lines = textArray;
            }
            else if (textArray == null && fileloc != null)
            {
                lines = File.ReadAllLines(fileloc);
            }
            else if (textArray == null && fileloc == null)
            {
                CSharpErrorReporter.ConsoleLineReporter.CSharpError("The Values textArray and fileloc are null");
                return;
            }

            bool foundUsing = false;
            string statement_line = code_part.TrimStart();
            if (statement_line.StartsWith("print"))
            {
                foreach (string line in lines)
                {
                    string line_trimed = line.TrimStart().TrimEnd();
                    if (line_trimed == "using Console;" || line_trimed == "from Console get print;")
                    {
                        foundUsing = true;
                        break;
                    }
                    else if (line.TrimStart().TrimEnd() == statement_line)
                    {
                        break;
                    }
                }

                if (!foundUsing)
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error($"ERROR; 'Console' was not referenced to use 'print'");
                    return;
                }
            }

            if (!statement_line.StartsWith("Console.print") && statement_line.StartsWith("print"))
            {
                statement_line = statement_line.Substring("print".Length);
            }
            else if (statement_line.StartsWith("Console.print") && !statement_line.StartsWith("print"))
            {
                statement_line = statement_line.Substring("Console.print".Length);
            }
            else
            {
                CSharpErrorReporter.ConsoleLineReporter.Message("Print was called without a \"print\" or \"Console.print\" at the start, checking again...");
                statement_line = statement_line.TrimStart();
                if (!statement_line.StartsWith("Console.print") && statement_line.StartsWith("print"))
                {
                    statement_line = statement_line.Substring("print".Length);
                }
                else if (statement_line.StartsWith("Console.print") && !statement_line.StartsWith("print"))
                {
                    statement_line = statement_line.Substring("Console.print".Length);
                }
                else
                {

                }
            }

            if (!statement_line.StartsWith("("))
            {
                CSharpErrorReporter.ConsoleLineReporter.Error($"Failed to find the start of statement at line \' {code_part} \'");
                return;
            }
            else
            {
                statement_line = statement_line.Substring(1, statement_line.Length - 1);
            }

            if (!statement_line.EndsWith(");"))
            {
                CSharpErrorReporter.ConsoleLineReporter.Error($"Failed to end statement at line \' {code_part} \'");
                return;
            }
            else
            {
                statement_line = statement_line.Substring(0, statement_line.Length - 2);
            }

            try
            {
                if (statement_line.Contains('+') || statement_line.Contains("-") || statement_line.Contains("*") || statement_line.Contains("/") || statement_line.Contains("%"))
                {
                    if (!(statement_line.StartsWith("\"") && statement_line.EndsWith("\"")))
                    {
                        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\EASY14_Variables_TEMP"))
                        {
                            char[] variablesInMathExpression = statement_line.Where(Char.IsLetter).ToArray();
                            foreach (char e in variablesInMathExpression)
                            {
                                string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\EASY14_Variables_TEMP\\" + e + ".txt";
                                if (File.Exists(fileName))
                                {
                                    statement_line = statement_line.Replace(e, Convert.ToChar(File.ReadAllText(fileName)));
                                }
                            }
                        }
                        try
                        {
                            Console.WriteLine(Eval.Execute<int>(statement_line));
                        }
                        catch
                        {
                            Console.WriteLine(statement_line);
                        }
                        return;
                    }
                }
            }
            catch (EvalException eval_ex)
            {
                if (eval_ex.Message.StartsWith("Integral constant is too large"))
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error("The Number in the expresion is too large");
                }
                else
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error(Convert.ToString(eval_ex.Message));
                }
            }

            if (statement_line.StartsWith("cl"))
            {
                if (statement_line.StartsWith("cl\"") && statement_line.EndsWith("\""))
                {
                    Console.Write(statement_line.Substring(3, statement_line.Length - 4)); return;
                }
                else if (statement_line.StartsWith("cl\"") && !statement_line.EndsWith("\""))
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error($"The String '{statement_line}' is not closed properly"); return;
                }
                else if (!statement_line.StartsWith("cl\"") && statement_line.EndsWith("\""))
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error($"The String '{statement_line}' is not opened properly"); return;
                }
            }
            else
            {
                if (statement_line.StartsWith("\"") && statement_line.EndsWith("\""))
                {
                    Console.WriteLine(statement_line.Substring(1, statement_line.Length - 2)); return;
                }
                else if (statement_line.StartsWith("\"") && !statement_line.EndsWith("\""))
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error($"The String '{statement_line}' is not closed properly"); return;
                }
                else if (!statement_line.StartsWith("\"") && statement_line.EndsWith("\""))
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error($"The String '{statement_line}' is not opened properly"); return;
                }
            }

            string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\\EASY14_Variables_TEMP";
            if (Directory.Exists(dir))
            {
                if (Directory.GetFiles(dir).Length != 0)
                {
                    string[] files = Directory.GetFiles(dir);
                    foreach (string file in files)
                    {
                        string varFileName = file.Substring(file.LastIndexOf("\\")).Replace(".txt", "").Substring(1);
                        if (varFileName == statement_line)
                        {
                            Console.WriteLine(File.ReadAllText(file));
                            break;
                        }
                    }
                }
            }

            //Anything else...
            Program prog = new Program();
            Console.WriteLine(prog.CompileCode_fromOtherFiles(textArray: new string[] { statement_line }));
            return;
        }
    }
}
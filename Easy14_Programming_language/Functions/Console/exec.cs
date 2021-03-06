using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class ConsoleExec
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(strWorkPath + "..\\..\\..\\..\\Application Code", "options.ini"));

        public static void Interperate(string code_part, string[] textArray, string fileloc, int lineNumber = -1)
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
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to exec()");
                    Console.ResetColor();
                    return;
                }
                foreach (string x in someLINEs)
                {
                    if (x.TrimStart().TrimEnd() == "using Console;" || x.TrimStart().TrimEnd() == "from Console get exec;")
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

            code_part = code_part.Substring(0, code_part.Length - 1);
            if (code_part.EndsWith(")"))
            {
                code_part = code_part.Substring(0, code_part.Length - 1);
            }

            code_part = code_part.Substring(1);
            code_part = code_part.Substring(0, code_part.Length - 1);

            textToPrint = code_part;

            Program prog = new Program();
            string[] execArray = { textToPrint.ToString() };

            try
            {
                if (textToPrint.StartsWith("C#)"))
                {
                    textToPrint = textToPrint.Substring(4);
                    execArray = new string[] { textToPrint.ToString() };
                    CSharpScript.RunAsync(string.Join(Environment.NewLine, execArray), ScriptOptions.Default.WithReferences(Assembly.GetExecutingAssembly()));
                }
                else
                {
                    prog.CompileCode_fromOtherFiles(null, execArray);
                }
            }
            catch (Exception e)
            {
                string unknownLine = "<unknownLineNumber>";
                var returnLineNumber = lineNumber > -1 ? lineNumber.ToString() : unknownLine;
                string[] errorText = { " An error occurred while executing commands from the exec command", $"  at line {returnLineNumber}", $"at line {code_part_unedited}\nException;\n{e.Message}\n" };
                ThrowErrorMessage.sendErrMessage(null, errorText, "error");
            }
            //}
        }
    }
}
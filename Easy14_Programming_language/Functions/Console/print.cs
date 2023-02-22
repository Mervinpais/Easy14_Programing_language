using System;
using System.Collections.Generic;
using System.Data;

namespace Easy14_Programming_Language
{
    public static class ConsolePrint
    {
        public static void Interperate(string line)
        {
            bool missingSemiColon = false;
            bool missingParenthesis = false;
            bool QuotesMismatch = false;

            line = line.Substring("Print".Length);

            if (line.EndsWith(";"))
            {
                line = line.Substring(0, line.Length - 1);
            }
            else
            {
                missingSemiColon = true;
            }

            if (line.StartsWith("(") && line.EndsWith(")"))
            {
                line = line.Substring(1, line.Length - 2);
            }
            else
            {
                missingParenthesis = true;
                line = line.Replace("(", "");
                line = line.Replace(")", "");
            }

            int errors = (missingSemiColon ? 1 : 0) + (missingParenthesis ? 1 : 0) + (QuotesMismatch ? 1 : 0);
            if (line.StartsWith("\"") && line.EndsWith("\""))
            {
                line = line.Substring(1, line.Length - 2);
                if (errors == 0)
                {
                    Console.WriteLine(line);
                    return;
                }
            }
            else if (VariableCode.variableList.ContainsKey(line))
            {
                Console.WriteLine(VariableCode.variableList[line]);
            }
            else if ((!line.StartsWith("\"") || !line.EndsWith("\"")) && !(!line.StartsWith("\"") && !line.EndsWith("\"")))
            {
                if ((line.IndexOf("\"") % 2 != 0))
                {
                    QuotesMismatch = true;
                }
            }
            else
            {
                try
                {
                    Console.WriteLine(new DataTable().Compute(line, ""));
                }
                catch
                {

                }
            }

            if (0 < errors)
            {
                List<string> errorMessage = new List<string>
                {
                    "Errors within print statement;"
                };

                if (missingSemiColon)
                {
                    errorMessage.Add("Missing a Semicolon");
                }
                if (missingParenthesis)
                {
                    errorMessage.Add("Missing 1 (or more) Parenthesies");
                }
                if (QuotesMismatch)
                {
                    errorMessage.Add("Mismatching quotes (or Missing quotes)");
                }
                ErrorReportor.ConsoleLineReporter.Error(string.Join(Environment.NewLine, errorMessage));
            }

            //Anything else...
            /*
            Program prog = new Program();
            Console.WriteLine(prog.CompileCode_fromOtherFiles(textArray: new string[] { statement_line }));
            return; */
        }
    }
}
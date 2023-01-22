using System;
using System.Collections.Generic;
using System.IO;

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
            else if (line.StartsWith("\"") || line.EndsWith("\"") || (line.IndexOf("\"") % 2 != 0))
            {
                QuotesMismatch = true;
            }
            else
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string variable_dir = dir + "\\EASY14_Variables_TEMP";
                if (Directory.Exists(variable_dir))
                {
                    var files = Directory.GetFiles(variable_dir);
                    if (files.Length > 0)
                    {
                        var variable = variable_dir + "\\" + line;
                        if (File.Exists(variable))
                        {
                            Console.WriteLine(File.ReadAllText(variable));
                        }
                        else
                        {
                            CSharpErrorReporter.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                        }
                    }
                    else
                    {
                        CSharpErrorReporter.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
                    }
                }
                else
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error("Failed to get variable \'" + line + "\', make sure variable exists.");
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
                CSharpErrorReporter.ConsoleLineReporter.Error(string.Join(Environment.NewLine, errorMessage));
            }

            //Anything else...
            /*
            Program prog = new Program();
            Console.WriteLine(prog.CompileCode_fromOtherFiles(textArray: new string[] { statement_line }));
            return; */
        }
    }
}
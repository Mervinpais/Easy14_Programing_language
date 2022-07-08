using System;
using System.Collections.Generic;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class while_Loop_fixed
    {
        /// <summary>
        /// This function takes in a string, an array of strings, an array of strings, a string, a
        /// boolean, and a string.
        /// </summary>
        /// <param name="code_part">The code part that is being Interperated.</param>
        /// <param name="lines">The lines of the code</param>
        /// <param name="textArray">The array of strings that contains the code.</param>
        /// <param name="fileloc">The location of the file</param>
        /// <param name="isInAMethod">If the code is in a method, this is true.</param>
        /// <param name="methodName">The name of the method that the code is in.</param>
        public static void Interperate(string code_part, string[] lines, string[] textArray, string fileloc, bool isInAMethod = false, string methodName = "}")
        {
            //make a program that gets the if statement
            string if_line = code_part;
            int lineCount_lines = 0;
            List<string> lines_list = new List<string>(lines);
            List<string> UnderLines_list = new List<string>();
            //From line 1 to where the if statment is, we remove it
            foreach (string line in lines)
            {
                if (line == code_part)
                {
                    lines_list.RemoveRange(0, lineCount_lines);
                    break;
                }
                lineCount_lines++;
            }
            //remove everything under the }
            lineCount_lines = 0;
            bool inIfStatment = false;
            foreach (string line in lines)
            {
                if (!inIfStatment)
                {
                    if (line == code_part)
                    {
                        inIfStatment = true;
                    }
                }
                if (line == "}")
                {
                    if (inIfStatment)
                    {
                        bool errorRecived = false;
                        try { string tempLine = lines[lineCount_lines + 1]; } catch { errorRecived = true; }
                        if (errorRecived)
                        {
                            break;
                        }
                        UnderLines_list = lines_list.GetRange(lineCount_lines - 2, lines_list.Count - lineCount_lines + 2);
                        lines_list.RemoveRange(lineCount_lines - 2, lines_list.Count - lineCount_lines + 2);
                        break;
                    }
                }
                lineCount_lines++;
            }

            //Console.WriteLine(string.Join(Environment.NewLine, lines_list.ToArray()));
            List<string> if_content = new List<string>(lines_list);
            if_content.RemoveRange(0, 1);
            if_content.RemoveRange(lines_list.Count - 2, 1);

            Program prog = new Program();

            if_line = if_line.Substring(5); //while
            if (if_line.EndsWith("{"))
                if_line = if_line.Substring(0, if_line.Length - 1);
            if_line = if_line.TrimStart().TrimEnd();

            string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";

            if (if_line.StartsWith("(") && if_line.EndsWith(")"))
            {
                if_line = if_line.Substring(1);
                if_line = if_line.Substring(0, if_line.Length - 1);

                string obj1 = null;
                bool obj1_isVar = false;
                string obj2 = null;
                bool obj2_isVar = false;

                string condition_operator = "?";
                if (if_line.Contains("=="))
                {
                    obj1 = if_line.Split("==")[0];
                    obj2 = if_line.Split("==")[1];
                    condition_operator = "==";
                }
                else if (if_line.Contains("!="))
                {
                    obj1 = if_line.Split("!=")[0];
                    obj2 = if_line.Split("!=")[1];
                    condition_operator = "!=";
                }

                obj1 = obj1.TrimStart().TrimEnd();
                obj2 = obj2.TrimStart().TrimEnd();

                if (File.Exists(dir + "\\" + obj1 + ".txt"))
                {
                    obj1_isVar = true;
                }
                if (File.Exists(dir + "\\" + obj2 + ".txt"))
                {
                    obj2_isVar = true;
                }

                if (obj1.StartsWith("\"") && obj1.EndsWith("\"")) obj1 = obj1.Substring(0, obj1.Length - 1).Substring(1);
                if (obj2.StartsWith("\"") && obj2.EndsWith("\"")) obj2 = obj2.Substring(0, obj2.Length - 1).Substring(1);
                obj1 = obj1.TrimStart().TrimEnd();
                obj2 = obj2.TrimStart().TrimEnd();

                bool conditionIsTrue = false;

                if (obj1_isVar == true && obj2_isVar == false)
                {
                    if (condition_operator == "==")
                    {
                        if (File.ReadAllText(dir + "\\" + obj1 + ".txt") == obj2) conditionIsTrue = true;
                    }
                    else if (condition_operator == "!=")
                    {
                        if (File.ReadAllText(dir + "\\" + obj1 + ".txt") != obj2) conditionIsTrue = true;
                    }
                }
                if (!obj1_isVar && obj2_isVar)
                {
                    if (condition_operator == "==")
                    {
                        if (obj1 == File.ReadAllText(dir + "\\" + obj2 + ".txt")) conditionIsTrue = true;
                    }
                    else if (condition_operator == "!=")
                    {
                        if (obj1 != File.ReadAllText(dir + "\\" + obj2 + ".txt")) conditionIsTrue = true;
                    }
                }
                if (!obj1_isVar && !obj2_isVar)
                {
                    if (condition_operator == "==")
                    {
                        if (File.ReadAllText(dir + "\\" + obj1 + ".txt") == File.ReadAllText(dir + "\\" + obj2 + ".txt")) conditionIsTrue = true;
                    }
                    else if (condition_operator == "!=")
                    {
                        if (File.ReadAllText(dir + "\\" + obj1 + ".txt") != File.ReadAllText(dir + "\\" + obj2 + ".txt")) conditionIsTrue = true;
                    }

                }
                if (obj1_isVar && obj2_isVar)
                {
                    if (condition_operator == "==")
                    {
                        if (obj1 == obj2) conditionIsTrue = true;
                    }
                    else if (condition_operator == "!=")
                    {
                        if (obj1 != obj2) conditionIsTrue = true;
                    }
                }

                while (conditionIsTrue)
                {
                    prog.CompileCode_fromOtherFiles(textArray: if_content.ToArray());
                }
                prog.CompileCode_fromOtherFiles(textArray: UnderLines_list.ToArray());
            }
        }
    }
}
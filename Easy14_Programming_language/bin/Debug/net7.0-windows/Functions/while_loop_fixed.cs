using System;
using System.Collections.Generic;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class While_Loop
    {
        public static void Interperate(string code_part, string[] lines, string[] textArray)
        {
            //make a program that gets the if statement
            string if_line = code_part;

            List<string> lines_list = new List<string>(lines);
            List<string> UnderLines_list = new List<string>();
            //From line 1 to where the if statment is, we remove it
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line == code_part)
                {
                    lines_list.RemoveRange(0, i);
                    break;
                }
            }
            //remove everything under the }
            bool inIfStatment = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
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
                        try { string tempLine = lines[i + 1]; } catch { errorRecived = true; }
                        if (errorRecived)
                        {
                            break;
                        }
                        UnderLines_list = lines_list.GetRange(i - 2, lines_list.Count - i + 2);
                        lines_list.RemoveRange(i - 2, lines_list.Count - i + 2);
                        break;
                    }
                }
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
                string obj2 = null;
                string condition_operator = "<?>";

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

                if (condition_operator == "<?>")
                {
                    ErrorReportor.ConsoleLineReporter.Error("Error; unknown operator used at while statement");
                    return;
                }

                obj1 = obj1.TrimStart().TrimEnd();
                obj2 = obj2.TrimStart().TrimEnd();

                bool obj1_isVar = false;
                if (File.Exists(dir + "\\" + obj1 + ".txt"))
                {
                    obj1_isVar = true;
                }

                bool obj2_isVar = false;
                if (File.Exists(dir + "\\" + obj2 + ".txt"))
                {
                    obj2_isVar = true;
                }

                if (obj1.StartsWith("\"") && obj1.EndsWith("\"")) obj1 = obj1.Substring(0, obj1.Length - 1).Substring(1);
                if (obj2.StartsWith("\"") && obj2.EndsWith("\"")) obj2 = obj2.Substring(0, obj2.Length - 1).Substring(1);
                obj1 = obj1.TrimStart().TrimEnd();
                obj2 = obj2.TrimStart().TrimEnd();

                bool conditionIsTrue = false;

                if (condition_operator == "<?>")
                {
                    ErrorReportor.ConsoleLineReporter.Error("Error; unknown operator used at while statement");
                    return;
                }
                if (obj1_isVar == true && obj2_isVar == false)
                {
                    switch (condition_operator)
                    {
                        case "==":
                            if (File.ReadAllText(dir + "\\" + obj1 + ".txt") == obj2) conditionIsTrue = true;
                            break;
                        case "!=":
                            if (File.ReadAllText(dir + "\\" + obj1 + ".txt") != obj2) conditionIsTrue = true;
                            break;
                    }
                }
                if (!obj1_isVar && obj2_isVar)
                {
                    switch (condition_operator)
                    {
                        case "==":
                            if (obj1 == File.ReadAllText(dir + "\\" + obj2 + ".txt")) conditionIsTrue = true;
                            break;
                        case "!=":
                            if (obj1 != File.ReadAllText(dir + "\\" + obj2 + ".txt")) conditionIsTrue = true;
                            break;
                    }
                }
                if (!obj1_isVar && !obj2_isVar)
                {
                    switch (condition_operator)
                    {
                        case "==":
                            if (File.ReadAllText(dir + "\\" + obj1 + ".txt") == File.ReadAllText(dir + "\\" + obj2 + ".txt")) conditionIsTrue = true;
                            break;
                        case "!=":
                            if (File.ReadAllText(dir + "\\" + obj1 + ".txt") != File.ReadAllText(dir + "\\" + obj2 + ".txt")) conditionIsTrue = true;
                            break;
                    }

                }
                if (obj1_isVar && obj2_isVar)
                {
                    switch (condition_operator)
                    {
                        case "==":
                            if (obj1 == obj2) conditionIsTrue = true;
                            break;
                        case "!=":
                            if (obj1 != obj2) conditionIsTrue = true;
                            break;
                    }
                }

                while (conditionIsTrue)
                {
                    prog.ExternalComplieCode(textArray: if_content.ToArray());
                }
                prog.ExternalComplieCode(textArray: UnderLines_list.ToArray());
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;

namespace Easy14_Coding_Language
{
    class If_Loop
    {
        Program prog = new Program();
        //Above needed as functions like 'compileCode' in Program.cs cant be accessed here and instead of copying it to other functions, just make an object of it 
        //and use it's 'compileCode_forOtherFiles' function to get 'compileCode' (because 'compileCode' is static, we need another function that is not static to access
        //the static function, kinda smart in my opinion), its kind of a bad way of doing it but it the easy way and has no error with it :|


        /// <summary>
        /// This function takes in a string, an array of strings, an array of strings, a string, a
        /// boolean, and a string.
        /// </summary>
        /// <param name="code_part">The code part that is being interperated.</param>
        /// <param name="lines">The lines of the code</param>
        /// <param name="textArray">The array of strings that contains the code.</param>
        /// <param name="fileloc">The location of the file</param>
        /// <param name="isInAMethod">If the code is in a method, this is true.</param>
        /// <param name="methodName">The name of the method that the code is in.</param>
        public void interperate(string code_part, string[] lines, string[] textArray, string fileloc,  bool isInAMethod = false, string methodName = "}")
        {
            string code_part_unedited = code_part;

            string[] if_lines = lines;
            int end_line_IDX = 0;
            int line_counterr = 1;

            //Take all lines and convert them to a list
            List<string> if_lines_list = new List<string>(if_lines);
            List<string> usings_lines_list = new List<string>(if_lines_list);

            //Below we get the usings and then use them for other stuff
            foreach (string line_withUsings in if_lines)
            {
                if (!line_withUsings.StartsWith("using"))
                {
                    usings_lines_list.RemoveRange(line_counterr, usings_lines_list.Count - line_counterr);
                    break;
                }
                line_counterr++;
            }

            line_counterr = 1;
            //Below is a method to find the code under the if statement
            foreach (string line_ifStatement in if_lines)
            {
                if (line_ifStatement == code_part)
                {
                    if_lines_list.RemoveRange(0, line_counterr - 1);
                    break;
                }
                line_counterr++;
            }

            line_counterr = 0;

            List<string> understuff = new List<string>(if_lines_list);
            foreach (string line__ in if_lines_list)
            {
                line_counterr++;
                if (line__ == "}")
                {
                    end_line_IDX = line_counterr;
                    if (if_lines_list.Count != end_line_IDX)
                    {
                        try
                        {
                            if_lines_list.RemoveRange(end_line_IDX, if_lines_list.Count - end_line_IDX);
                            understuff.RemoveRange(0, end_line_IDX);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    break;
                }
            }
            string[] arr = if_lines_list.ToArray();
            string if_Line = if_lines_list[0];
            if_Line = if_Line.Substring(2);
            if_Line = if_Line.Substring(1, if_Line.Length - 2);

            string obj1 = null; bool obj1_variable = false;
            string obj2 = null; bool obj2_variable = false;

            /* Checking if the if statement is true or false. */
            if (if_Line.TrimEnd().TrimStart() != "(true)" && if_Line.TrimEnd().TrimStart() != "(false)")
            {
                if (if_Line.Contains("=="))
                    obj1 = if_Line.Substring(0, if_Line.IndexOf("==") - 0);
                if (if_Line.Contains("!="))
                    obj1 = if_Line.Substring(0, if_Line.IndexOf("!=") - 0);

                if (if_Line.Contains("=="))
                    obj2 = if_Line.Substring((if_Line.IndexOf("==") + 3));
                if (if_Line.Contains("!="))
                    obj2 = if_Line.Substring(if_Line.IndexOf("!=") + 2);

                obj1 = obj1.TrimStart().TrimEnd();
                obj2 = obj2.TrimStart().TrimEnd();

                if (obj1.StartsWith("\"") && obj1.EndsWith("\"")) obj1_variable = false;
                else obj1_variable = true;
                if (obj2.StartsWith("\"") && obj2.EndsWith("\"")) obj2_variable = false;
                else obj2_variable = true;
            }

            if (if_Line.TrimEnd().TrimStart() == "(true)")
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP";

                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;
                foreach (string x in someLINEs)
                {
                    lin_count++;
                    if (!x.StartsWith("using") && x != "" && x != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                List<string> usings_code = someLINEs.GetRange(0, lin_count);
                usings_code.AddRange(e_code);
                e_code = usings_code;

                if (true)
                {
                    prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    System.Threading.Thread.Sleep(100);
                }
            }
            if (if_Line.TrimEnd().TrimStart() == "(false)")
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP";

                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;
                foreach (string x in someLINEs)
                {
                    lin_count++;
                    if (!x.StartsWith("using") && x != "" && x != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                List<string> usings_code = someLINEs.GetRange(0, lin_count);
                usings_code.AddRange(e_code);
                e_code = usings_code;

                if (false)
                {
                    //Yes C#, i wnat this code to be unreachable ok? :)
                    prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    System.Threading.Thread.Sleep(100);
                }
            }
            else if (if_Line.Contains("=="))
            {
                /* Checking if the variables are variables or not. */
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";
                if (!Directory.Exists(dir) || Directory.GetFiles(dir).Length <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR; No variables were ever made so the program can't find any variables :(");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                }
                if (!obj1.Contains("\""))
                {
                    obj1_variable = true;
                }
                if (!obj2.Contains("\""))
                {
                    obj2_variable = true;
                }
                /*
                foreach (string ffile in Directory.GetFiles(dir))
                {
                    if (ffile.Substring(ffile.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == obj1)
                    { obj1_variable = true; }
                }
                foreach (string ffile in Directory.GetFiles(dir))
                {
                    if (ffile.Substring(ffile.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == obj2)
                    { obj2_variable = true; }
                }*/

                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;
                foreach (string x in someLINEs)
                {
                    lin_count++;
                    if (!x.StartsWith("using") && x != "" && x != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                string obj1_fileContent = null;
                string obj2_fileContent = null;
                if (obj1_variable)
                    obj1_fileContent = File.ReadAllText(dir + @$"\{obj1}.txt");
                if (obj2_variable)
                    obj2_fileContent = File.ReadAllText(dir + @$"\{obj2}.txt");

                if (obj1_variable && !obj2_variable)
                {
                    if (obj1_fileContent == obj2.Replace("\"", ""))
                    {
                        List<string> e_code = if_lines_list.GetRange(1, if_lines_list.Count - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (!obj1_variable && obj2_variable)
                {
                    if (obj1.Replace("\"", "") == obj2_fileContent)
                    {
                        List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable && obj2_variable)
                {
                    if (obj1_fileContent == obj2_fileContent)
                    {
                        List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (!obj1_variable && !obj2_variable)
                {
                    if (obj1.Replace("\"", "") == obj2.Replace("\"", ""))
                    {
                        List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(fileloc, e_code.ToArray());
                    }
                }

                understuff.RemoveRange(0, end_line_IDX - 2);
                List<string> code_toContinueExceuting = new List<string>();
                code_toContinueExceuting.AddRange(usings_lines_list);
                code_toContinueExceuting.AddRange(understuff);

                //int lineIDX_underpart = lineIDX + (code_toContinueExceuting.ToArray().Length - lineIDX);
                
                if (isInAMethod != true)
                {
                    prog.compileCode_fromOtherFiles(null, code_toContinueExceuting.ToArray());
                }
            }
            else if (if_Line.Contains("!="))
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";
                if (!obj1.Contains("\""))
                {
                    obj1_variable = true;
                }
                if (!obj2.Contains("\""))
                {
                    obj2_variable = true;
                }

                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;
                foreach (string x in someLINEs)
                {
                    lin_count++;
                    if (!x.StartsWith("using") && x != "" && x != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                string obj1_fileContent = null;
                string obj2_fileContent = null;

                if (obj1_variable)
                    obj1_fileContent = File.ReadAllText(dir + @$"\{obj1}.txt");
                if (obj2_variable)
                    obj2_fileContent = File.ReadAllText(dir + @$"\{obj2}.txt");

                if (obj1_variable == true && obj2_variable == false)
                {
                    if (obj1_fileContent != obj2.Replace("\"", ""))
                    {
                        List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == true)
                {
                    if (obj1.Replace("\"", "") != obj2_fileContent)
                    {
                        List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable == true && obj2_variable == true)
                {
                    if (obj1_fileContent != obj2_fileContent)
                    {
                        List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == false)
                {
                    if (obj1.Replace("\"", "") != obj2.Replace("\"", ""))
                    {
                        List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(fileloc, e_code.ToArray());
                    }
                }

                int part_to_continue_at = 1;
                foreach (string e in understuff)
                {
                    if (e == methodName + "();")
                    {
                        break;
                    }
                    part_to_continue_at++;
                }

                understuff.RemoveRange(0, end_line_IDX - 1);
                List<string> code_toContinueExceuting = new List<string>();
                code_toContinueExceuting.AddRange(usings_lines_list);
                code_toContinueExceuting.AddRange(understuff);

                part_to_continue_at = 1;
                /*
                foreach (string e in code_toContinueExceuting)
                {
                    if (e == methodName + "();")
                    {
                        code_toContinueExceuting.RemoveAt(part_to_continue_at- 1);
                        break;
                    }
                    part_to_continue_at++;
                }*/

                //int lineIDX_underpart = lineIDX + (code_toContinueExceuting.ToArray().Length - lineIDX);
                if (isInAMethod != true)
                {
                    prog.compileCode_fromOtherFiles(null, code_toContinueExceuting.ToArray());
                }
            }
        }
    }
}
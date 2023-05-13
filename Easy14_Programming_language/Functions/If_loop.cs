using System;
using System.Collections.Generic;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class If_Loop_OLD
    {
        public static void Interperate(string code_part, string[] lines, string[] textArray, string fileloc, bool isInAMethod = false, string methodName = "}")
        {
            Program prog = new Program();
            string code_part_unedited = code_part;
            int end_line_IDX = 0;

            List<string> if_lines_list = new List<string>(lines);
            List<string> usings_lines_list = new List<string>(lines);

            //Below we get the usings and then use them for other stuff
            int lines_lineCounter = 0;
            foreach (string line_withUsings in lines)
            {
                if (!line_withUsings.StartsWith("using"))
                {
                    usings_lines_list.RemoveRange(lines_lineCounter, usings_lines_list.Count - lines_lineCounter);
                    break;
                }
                lines_lineCounter++;
            }

            lines_lineCounter = 0;

            foreach (string line_ifStatement in lines)
            {
                if (line_ifStatement == code_part)
                {
                    if_lines_list.RemoveRange(0, lines_lineCounter - 1);
                    break;
                }
                lines_lineCounter++;
            }

            lines_lineCounter = 0;

            List<string> understuff = new List<string>(if_lines_list);
            foreach (string line__ in if_lines_list)
            {
                lines_lineCounter++;
                if (line__ == "}")
                {
                    end_line_IDX = lines_lineCounter - 2;
                    if (if_lines_list.Count != end_line_IDX)
                    {
                        try
                        {
                            if_lines_list.RemoveRange(end_line_IDX + 2, if_lines_list.Count - end_line_IDX - 2);
                            understuff.RemoveRange(0, end_line_IDX + 2);
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
            string if_Line = if_lines_list[1];
            if_Line = if_Line.Substring(2);
            if_Line = if_Line.Substring(1, if_Line.Length - 1);

            string obj1 = null;
            bool obj1_variable = false;
            string obj2 = null;
            bool obj2_variable = false;

            if_Line = if_Line.TrimEnd().TrimStart();
            if (if_Line != "(true)" && if_Line != "(false)")
            {
                obj1 = if_Line.Contains("==") ? obj1 = if_Line.Substring(0, if_Line.IndexOf("==") - 0) : obj1 = if_Line.Substring(0, if_Line.IndexOf("!=") - 0);
                obj2 = if_Line.Contains("==") ? obj2 = if_Line.Substring((if_Line.IndexOf("==") + 3)) : obj2 = if_Line.Substring(if_Line.IndexOf("!=") + 2);

                obj1 = obj1.TrimStart().TrimEnd();
                obj2 = obj2.TrimStart().TrimEnd();

                obj1_variable = (obj1.StartsWith("\"") && obj1.EndsWith("\""));
                obj2_variable = (obj2.StartsWith("\"") && obj2.EndsWith("\""));
            }

            if (if_Line == "(true)")
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP";

                List<string> someLINEs = null;
                if (textArray == null && fileloc != null)
                {
                    someLINEs = new List<string>(File.ReadAllLines(fileloc));
                }
                else if (textArray != null && fileloc == null)
                {
                    someLINEs = new List<string>(textArray);
                }

                int lin_count = 1;

                foreach (string usingStatements in someLINEs)
                {
                    lin_count++;
                    if (!usingStatements.StartsWith("using") && usingStatements != "" && usingStatements != null)
                    { break; }
                }

                lin_count = lin_count - 2;

                List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX - 2);
                List<string> usings_code = someLINEs.GetRange(0, lin_count);
                usings_code.AddRange(Code_in_if_statement_List);
                Code_in_if_statement_List = usings_code;

                if (true)
                {
                    prog.CompileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    System.Threading.Thread.Sleep(100);
                }
            }

            if (if_Line == "(false)")
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP";

                List<string> someLINEs = null;
                if (textArray == null && fileloc != null)
                {
                    someLINEs = new List<string>(File.ReadAllLines(fileloc));
                }
                else if (textArray != null && fileloc == null)
                {
                    someLINEs = new List<string>(textArray);
                }

                int lin_count = 1;

                foreach (string usingStatements in someLINEs)
                {
                    lin_count++;
                    if (!usingStatements.StartsWith("using") && usingStatements != "" && usingStatements != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX - 2);
                List<string> usings_code = someLINEs.GetRange(0, lin_count);
                usings_code.AddRange(Code_in_if_statement_List);
                Code_in_if_statement_List = usings_code;

                if (false)
                {
                    //Yes C#, i wnat this code to be unreachable ok? :)
#pragma warning disable CS0162 // Unreachable code detected
                    prog.CompileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
#pragma warning restore CS0162 // Unreachable code detected
                    System.Threading.Thread.Sleep(100);
                }
            }

            else if (if_Line.Contains("=="))
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";
                if (!Directory.Exists(dir) || Directory.GetFiles(dir).Length <= 0)
                {
                    ExceptionSender.SendException("0xFC0002");
                    return;
                }

                string var1_fileLoc = GetVariable.findVar(obj1);
                string var2_fileLoc = GetVariable.findVar(obj2);

                if (var1_fileLoc != "0xF00001") obj1_variable = true;
                if (var2_fileLoc != "0xF00001") obj2_variable = true;

                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;

                foreach (string usingStatements in someLINEs)
                {
                    lin_count++;
                    if (!usingStatements.StartsWith("using") && usingStatements != "" && usingStatements != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                string obj1_fileContent = null;
                string obj2_fileContent = null;
                if (obj1_variable)
                {
                    try
                    {
                        obj1_fileContent = File.ReadAllText(dir + @$"\\{obj1.Replace("\"", "")}");
                    }
                    catch
                    {
                        obj1_variable = false;
                    }
                }
                if (obj2_variable)
                {
                    try
                    {
                        obj2_fileContent = File.ReadAllText(dir + @$"\\{obj2.Replace("\"", "")}");
                    }
                    catch
                    {
                        obj2_variable = false;
                    }
                }


                if (obj1_variable && !obj2_variable)
                {
                    if (obj1_fileContent == obj2.Replace("\"", ""))
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(2, if_lines_list.Count - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }

                else if (!obj1_variable && obj2_variable)
                {
                    if (obj1.Replace("\"", "") == obj2_fileContent)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(2, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }
                else if (obj1_variable && obj2_variable)
                {
                    if (obj1_fileContent == obj2_fileContent)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(2, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }

                else if (!obj1_variable && !obj2_variable)
                {
                    if (obj1.Replace("\"", "") == obj2.Replace("\"", ""))
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(2, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(fileloc, Code_in_if_statement_List.ToArray());
                    }
                }

                List<string> code_toContinueExceuting = new List<string>();
                code_toContinueExceuting.AddRange(usings_lines_list);
                code_toContinueExceuting.AddRange(understuff);

                if (isInAMethod != true)
                {
                    prog.CompileCode_fromOtherFiles(null, code_toContinueExceuting.ToArray());
                }
            }

            else if (if_Line.Contains("!="))
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";
                string var1_fileLoc = GetVariable.findVar(obj1);
                string var2_fileLoc = GetVariable.findVar(obj2);

                if (var1_fileLoc != "0xF00001") obj1_variable = true;
                if (var2_fileLoc != "0xF00001") obj2_variable = true;

                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;
                foreach (string usingStatements in someLINEs)
                {
                    lin_count++;
                    if (!usingStatements.StartsWith("using") && usingStatements != "" && usingStatements != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                string obj1_fileContent = null;
                string obj2_fileContent = null;


                if (obj1_variable)
                {
                    try
                    {
                        obj1_fileContent = File.ReadAllText(dir + @$"\\{obj1.Replace("\"", "")}");
                    }
                    catch
                    {
                        obj1_variable = false;
                    }
                }
                if (obj2_variable)
                {
                    try
                    {
                        obj2_fileContent = File.ReadAllText(dir + @$"\\{obj2.Replace("\"", "")}");
                    }
                    catch
                    {
                        obj2_variable = false;
                    }
                }


                if (obj1_variable == true && obj2_variable == false)
                {
                    if (obj1_fileContent != obj2.Replace("\"", ""))
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(2, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == true)
                {
                    if (obj1.Replace("\"", "") != obj2_fileContent)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(2, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == true && obj2_variable == true)
                {
                    if (obj1_fileContent != obj2_fileContent)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(2, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == false)
                {
                    obj1 = obj1.Substring(0, obj1.Length).Substring(1);
                    obj2 = obj2.Substring(0, obj2.Length).Substring(1);
                    if (obj1 != obj2)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(2, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(fileloc, Code_in_if_statement_List.ToArray());
                    }
                }

                int part_to_continue_at = 1;
                foreach (string e in understuff)
                {
                    if (e == methodName + "();") break;
                    part_to_continue_at++;
                }

                List<string> code_toContinueExceuting = new List<string>();
                code_toContinueExceuting.AddRange(usings_lines_list);
                code_toContinueExceuting.AddRange(understuff);

                part_to_continue_at = 1;

                if (isInAMethod != true)
                {
                    prog.CompileCode_fromOtherFiles(null, code_toContinueExceuting.ToArray());
                }
            }
        }
    }
}
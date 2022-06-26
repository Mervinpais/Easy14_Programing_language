using System;
using System.Collections.Generic;
using System.IO;

namespace Easy14_Programming_Language
{
    class WhileLoop
    {
        Program prog = new Program();
        public void Interperate(string code_part, string[] textArray, string[] lines, string fileloc)
        {
            string code_part_unedited;
            //string textToPrint;
            code_part_unedited = code_part;

            string[] _lines_ = lines;
            int line_counterr = 1;
            int end_line_IDX = 0;
            List<string> while_lines_list = new List<string>(_lines_);
            foreach (string line__ in _lines_)
            {
                if (line__ == code_part)
                {
                    while_lines_list.RemoveRange(0, line_counterr - 1);
                    break;
                }
                line_counterr++;
            }
            line_counterr = 0;

            //Take all lines and convert them to a list
            List<string> if_lines_list = new List<string>(_lines_);
            List<string> usings_lines_list = new List<string>(if_lines_list);

            //Below we get the usings and then use them for other stuff
            foreach (string line_withUsings in _lines_)
            {
                if (!line_withUsings.StartsWith("using"))
                {
                    usings_lines_list.RemoveRange(line_counterr, usings_lines_list.Count - line_counterr);
                    break;
                }
                line_counterr++;
            }

            line_counterr = 1;

            List<string> understuff = new List<string>(while_lines_list);
            foreach (string line__ in while_lines_list)
            {
                if (line__ == "}")
                {
                    end_line_IDX = line_counterr - 2;
                    if (while_lines_list.Count != end_line_IDX)
                    {
                        try
                        {
                            while_lines_list.RemoveRange(end_line_IDX + 1, while_lines_list.Count - end_line_IDX - 1);
                            understuff.RemoveRange(0, end_line_IDX);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    break;
                }
                line_counterr++;
            }
            string[] arr = while_lines_list.ToArray();
            string while_Line = while_lines_list[0];
            while_Line = while_Line.Substring(5);
            while_Line = while_Line.Substring(1, while_Line.Length - 2);
            string obj1 = null;
            string obj2 = null;
            bool obj1_variable = false;
            bool obj2_variable = false;

            if (while_Line.TrimEnd().TrimStart() == "(false)")
            {
                if (while_Line.Contains("=="))
                    obj1 = while_Line.Substring(0, while_Line.IndexOf("==") - 0);
                if (while_Line.Contains("!="))
                    obj1 = while_Line.Substring(0, while_Line.IndexOf("!=") - 0);

                if (while_Line.Contains("=="))
                    obj2 = while_Line.Substring((while_Line.IndexOf("==") + 3));
                if (while_Line.Contains("!="))
                    obj2 = while_Line.Substring(while_Line.IndexOf("!=") + 2);

                obj1 = obj1.TrimStart().TrimEnd();
                obj2 = obj2.TrimStart().TrimEnd();


                if (obj1.StartsWith("\"") && obj1.EndsWith("\"")) 
                    obj1_variable = false;
                else 
                    obj1_variable = true;
                
                if (obj2.StartsWith("\"") && obj2.EndsWith("\"")) 
                    obj2_variable = false;
                else
                    obj2_variable = true;
            }

            if (while_Line.TrimEnd().TrimStart() == "(true)")
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";

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

                List<string> Code_in_while_statement_List = while_lines_list.GetRange(1, end_line_IDX - 2);
                List<string> usings_code = someLINEs.GetRange(0, lin_count);
                usings_code.AddRange(Code_in_while_statement_List);
                Code_in_while_statement_List = usings_code;

                while (true)
                {
                    prog.CompileCode_fromOtherFiles(null, Code_in_while_statement_List.ToArray());
                    System.Threading.Thread.Sleep(100); //Just to not cause a stackoverflow/lag the computer
                }
            }
            else if (while_Line.Contains("=="))
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";

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
                {
                    if (!File.Exists(dir + $"\\{obj1}.txt"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ERROR; Variable \"{obj1}\" does not exist!");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }
                    else
                    {
                        obj1_fileContent = File.ReadAllText(dir + $"\\{obj1}.txt");
                    }
                }

                if (obj2_variable)
                {
                    if (!File.Exists(dir + $"\\{obj2}.txt"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ERROR; Variable \"{obj2}\" does not exist!");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }
                    else
                    {
                        obj2_fileContent = File.ReadAllText(dir + $"\\{obj2}.txt");
                    }
                }

                if (obj1_variable == true && obj2_variable == false)
                {
                    obj2 = obj2.Substring(0, obj2.Length - 1).Substring(1);
                    while (obj1_fileContent == obj2)
                    {
                        List<string> Code_in_while_statement_List = while_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_while_statement_List);
                        Code_in_while_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_while_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == true)
                {
                    obj1 = obj1.Substring(0, obj1.Length - 1).Substring(1);
                    while (obj1 == obj2_fileContent)
                    {
                        List<string> Code_in_while_statement_List = while_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_while_statement_List);
                        Code_in_while_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_while_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == true && obj2_variable == true)
                {
                    while (obj1_fileContent == obj2_fileContent)
                    {
                        List<string> Code_in_while_statement_List = while_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_while_statement_List);
                        Code_in_while_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_while_statement_List.ToArray());
                        obj1_fileContent = File.ReadAllText(dir + $"\\{obj1}.txt");
                        obj2_fileContent = File.ReadAllText(dir + $"\\{obj2}.txt");
                    }
                }
                else if (obj1_variable == false && obj2_variable == false)
                {
                    obj1 = obj1.Substring(0, obj1.Length - 1).Substring(1);
                    obj2 = obj2.Substring(0, obj2.Length - 1).Substring(1);
                    while (obj1 == obj2)
                    {
                        List<string> Code_in_while_statement_List = while_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_while_statement_List);
                        Code_in_while_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_while_statement_List.ToArray());
                    }
                }
                //understuff.RemoveRange(0, end_line_IDX);
                List<string> code_toContinueExceuting = new List<string>();
                code_toContinueExceuting.AddRange(usings_lines_list);
                code_toContinueExceuting.AddRange(understuff);

                prog.CompileCode_fromOtherFiles(null, code_toContinueExceuting.ToArray());
            }
            else if (while_Line.Contains("!="))
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";

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
                {
                    if (!File.Exists(dir + $"\\{obj1}.txt"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ERROR; Variable \"{obj1}\" does not exist!");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }
                    else
                    {
                        obj1_fileContent = File.ReadAllText(dir + $"\\{obj1}.txt");
                    }
                }

                if (obj2_variable)
                {
                    if (!File.Exists(dir + $"\\{obj2}.txt"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ERROR; Variable \"{obj2}\" does not exist!");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }
                    else
                    {
                        obj2_fileContent = File.ReadAllText(dir + $"\\{obj2}.txt");
                    }
                }

                if (obj1_variable == true && obj2_variable == false)
                {
                    while (obj1_fileContent != obj2.Replace("\"", ""))
                    {
                        List<string> Code_in_while_statement_List = while_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_while_statement_List);
                        Code_in_while_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_while_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == true)
                {
                    while (obj1.Replace("\"", "") != obj2_fileContent)
                    {
                        List<string> Code_in_while_statement_List = while_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_while_statement_List);
                        Code_in_while_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_while_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == true && obj2_variable == true)
                {
                    while (obj1_fileContent != obj2_fileContent)
                    {
                        List<string> Code_in_while_statement_List = while_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_while_statement_List);
                        Code_in_while_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_while_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == false)
                {
                    while (obj1.Replace("\"", "") != obj2.Replace("\"", ""))
                    {
                        List<string> Code_in_while_statement_List = while_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_while_statement_List);
                        Code_in_while_statement_List = usings_code;
                        prog.CompileCode_fromOtherFiles(null, Code_in_while_statement_List.ToArray());
                    }
                }
                //understuff.RemoveRange(0, end_line_IDX);
                List<string> code_toContinueExceuting = new List<string>();
                code_toContinueExceuting.AddRange(usings_lines_list);
                code_toContinueExceuting.AddRange(understuff);

                prog.CompileCode_fromOtherFiles(null, code_toContinueExceuting.ToArray());
            }
        }
    }
}
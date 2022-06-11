using System;
using System.Collections.Generic;
using System.IO;

namespace Easy14_Coding_Language
{
    class WhileLoop
    {
        Program prog = new Program();
        //Above needed as functions like 'compileCode' in Program.cs cant be accessed here and instead of copying it to other functions, just make an object of it 
        //and use it's 'compileCode_forOtherFiles' function to get 'compileCode' (because 'compileCode' is static, we need another function that is not static to access
        //the static function, kinda smart in my opinion), its kind of a bad way of doing it but it the easy way and has no error with it :|
        public void interperate(string code_part, string[] textArray, string[] lines, string fileloc)
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
            List<string> understuff = new List<string>(while_lines_list);
            foreach (string line__ in while_lines_list)
            {
                line_counterr++;
                if (line__ == "}")
                {
                    end_line_IDX = line_counterr;
                    if (while_lines_list.Count != end_line_IDX)
                    {
                        try
                        {
                            while_lines_list.RemoveRange(end_line_IDX, while_lines_list.Count - end_line_IDX);
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
            string[] arr = while_lines_list.ToArray();
            string if_Line = while_lines_list[0];
            if_Line = if_Line.Substring(5);
            if_Line = if_Line.Substring(1, if_Line.Length - 2);
            string obj1 = null;
            string obj2 = null;
            bool obj1_variable = false;
            bool obj2_variable = false;

            if (if_Line.TrimEnd().TrimStart() != "(true)")
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

                List<string> e_code = while_lines_list.GetRange(1, end_line_IDX - 2);
                List<string> usings_code = someLINEs.GetRange(0, lin_count);
                usings_code.AddRange(e_code);
                e_code = usings_code;

                while (true)
                {
                    prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    System.Threading.Thread.Sleep(100); //Just to not cause a stackoverflow/lag the computer
                }
            }
            else if (if_Line.Contains("=="))
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
                    while (obj1_fileContent == obj2.Replace("\"", ""))
                    {
                        List<string> e_code = while_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == true)
                {
                    while (obj1.Replace("\"", "") == obj2_fileContent)
                    {
                        List<string> e_code = while_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable == true && obj2_variable == true)
                {
                    while (obj1_fileContent == obj2_fileContent)
                    {
                        List<string> e_code = while_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == false)
                {
                    while (obj1.Replace("\"", "") == obj2.Replace("\"", ""))
                    {
                        List<string> e_code = while_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                //understuff.RemoveRange(0, end_line_IDX);
                prog.compileCode_fromOtherFiles(null, understuff.ToArray());
            }
            else if (if_Line.Contains("!="))
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
                        List<string> e_code = while_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == true)
                {
                    while (obj1.Replace("\"", "") != obj2_fileContent)
                    {
                        List<string> e_code = while_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable == true && obj2_variable == true)
                {
                    while (obj1_fileContent != obj2_fileContent)
                    {
                        List<string> e_code = while_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == false)
                {
                    while (obj1.Replace("\"", "") != obj2.Replace("\"", ""))
                    {
                        List<string> e_code = while_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                understuff.RemoveRange(0, end_line_IDX);
                prog.compileCode_fromOtherFiles(null, understuff.ToArray());
            }
        }
    }
}
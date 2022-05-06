using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;
using System.Linq;
using System.Management;

namespace Easy14_Coding_Language
{
    class If_loop
    {
        Program prog = new Program(); 
        //Above needed as functions like 'compileCode' in Program.cs cant be accessed here and instead of copying it to other functions, just make an object of it 
        //and use it's 'compileCode_forOtherFiles' function to get 'compileCode' (because 'compileCode' is static, we need another function that is not static to access
        //the static function, kinda smart in my opinion), its kind of a bad way of doing it but it the easy way and has no error with it :|
        public void interperate(string code_part, string[] lines, string[] textArray, string fileloc)
        {
            string code_part_unedited;
            //string textToPrint;
            code_part_unedited = code_part;

            string[] if_lines = lines;
            int end_line_IDX = 0;
            int line_counterr = 1;
            List<string> if_lines_list = new List<string>(if_lines);

            foreach (string line__ in if_lines)
            {
                if (line__ == code_part)
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
            string obj1 = null;
            bool obj1_variable = false;
            if (if_Line.Contains("=="))
                obj1 = if_Line.Substring(0, if_Line.IndexOf("==") - 0);
            if (if_Line.Contains("!="))
                obj1 = if_Line.Substring(0, if_Line.IndexOf("!=") - 0);
            string obj2 = null;
            bool obj2_variable = false;
            if (if_Line.Contains("=="))
                obj2 = if_Line.Substring(if_Line.IndexOf("==") + 2).TrimEnd();
            if (if_Line.Contains("!="))
                obj2 = if_Line.Substring(if_Line.IndexOf("!=") + 2).TrimEnd();
            if (if_Line.Contains("=="))
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP";
                foreach (string ffile in Directory.GetFiles(dir))
                {
                    if (ffile.Substring(ffile.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == obj1.Replace(".txt", ""))
                    {
                        obj1_variable = true;
                    }
                }
                foreach (string ffile in Directory.GetFiles(dir))
                {
                    if (ffile.Substring(ffile.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "") == obj2.Replace(".txt", ""))
                    {
                        obj2_variable = true;
                    }
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

                if (obj1_variable == true && obj2_variable == false)
                {
                    if (File.ReadAllText(dir + @$"\{obj1}.txt") == obj2)
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
                    if (obj1 == File.ReadAllText(dir + @$"\{obj2}.txt"))
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
                    if (File.ReadAllText(dir + @$"\{obj1}.txt") == File.ReadAllText(dir + @$"\{obj2}.txt"))
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
                    if (obj1 == obj2)
                    {
                        List<string> e_code = if_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(e_code);
                        e_code = usings_code;
                        prog.compileCode_fromOtherFiles(null, e_code.ToArray());
                    }
                }
                understuff.RemoveRange(0, end_line_IDX - 1);
                prog.compileCode_fromOtherFiles(null, understuff.ToArray());
            }
        }
    }
}
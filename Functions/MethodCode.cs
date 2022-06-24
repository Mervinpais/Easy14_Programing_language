using System;
using System.Collections.Generic;
using System.IO;

namespace Easy14_Programming_Language
{
    class MethodCode
    {
        Program prog = new Program();
        public void interperate(string code_part, string[] textArray, string[] lines, string fileloc, bool Making_A_Method)
        {
            string code_part_unedited;
            //string textToPrint;
            code_part_unedited = code_part;

            if (Making_A_Method == true)
            {
                string[] _lines_ = lines;
                int line_counterr = 1;
                int end_line_IDX = 0;

                List<string> method_lines_list = new List<string>(_lines_);

                foreach (string line__ in _lines_)
                {
                    if (line__ == code_part)
                    {
                        method_lines_list.RemoveRange(0, line_counterr - 1);
                        break;
                    }
                    line_counterr++;
                }
                line_counterr = 0;
                List<string> understuff = new List<string>(method_lines_list);
                foreach (string line__ in method_lines_list)
                {
                    line_counterr++;
                    if (line__ == "}")
                    {
                        end_line_IDX = line_counterr;
                        if (method_lines_list.Count != end_line_IDX)
                        {
                            try
                            {
                                method_lines_list.RemoveRange(end_line_IDX, method_lines_list.Count - end_line_IDX);
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

                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;
                foreach (string x in someLINEs)
                {
                    if (!x.StartsWith("using"))
                    {
                        break;
                    }
                    lin_count++;
                }

                string[] arr = method_lines_list.ToArray();
                
                List<string> code_in_method_code = method_lines_list.GetRange(1, end_line_IDX - 1);
                List<string> usings_code = someLINEs.GetRange(0, lin_count);
                // This \/ is just something i added and it just fixes everything
                List<string> usings_code_undercode = someLINEs.GetRange(0, lin_count);
                usings_code.AddRange(code_in_method_code);
                usings_code_undercode.AddRange(understuff);
                code_in_method_code = usings_code;
                
                string methodName = code_part_unedited;
                methodName = methodName.Replace("func", "").TrimStart();
                methodName = methodName.Substring(0, methodName.IndexOf("("));

                code_in_method_code.RemoveAt(0);
                code_in_method_code.RemoveAt(code_in_method_code.Count - 1);
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}\\INSTRUCTIONS.txt", string.Join(Environment.NewLine, code_in_method_code.ToArray()));

                prog.compileCode_fromOtherFiles(null, understuff.ToArray(), 0, true, methodName);
            }
            else
            {
                string methodName = code_part_unedited;
                methodName = methodName.Replace("func", "").TrimStart();
                methodName = methodName.Substring(0, methodName.IndexOf("("));
                
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}") == false)
                {
                    Console.WriteLine("ERROR; Can't Find method, make sure you made the method in your code");
                    return;
                }
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}");
                
                try {
                    prog.compileCode_fromOtherFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}\\INSTRUCTIONS.txt", null, 0, true, methodName);
                }
                catch
                {
                    Console.WriteLine("");
                    return;
                }
                List<string> understuff = new List<string>(lines);
                understuff.RemoveRange(understuff.IndexOf(code_part) , 1);
                // The Reason why the Above /\ needs to have a "+ 1" at the end is because it will get stuck in a loop, example (CHECK LINE 126);
            }
        }
    }
}
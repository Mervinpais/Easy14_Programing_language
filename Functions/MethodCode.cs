using System;
using System.Collections.Generic;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class MethodCode
    {
        /*
         * CHANGLE LOG 24/6/2022;
         * Code will/has  be/been refactored for better readablilty and easier to understand
         */
        public static void Interperate(string code_part, string[] textArray, string[] lines, string fileloc, bool Making_A_Method)
        {
            Program prog = new Program();
            string code_part_unedited = code_part;

            if (Making_A_Method == true)
            {
                string[] _lines_ = lines;
                int last_line_Index = 0;

                List<string> method_lines_list = new List<string>(_lines_);

                int lines_lineCounter = 1;
                foreach (string line__ in _lines_)
                {
                    if (line__ == code_part)
                    {
                        method_lines_list.RemoveRange(0, lines_lineCounter - 1);
                        break;
                    }
                    lines_lineCounter++;
                }

                int underStuff_lineCounterr = 0;
#pragma warning disable IDE0090 // Use 'new(...)'
                List<string> understuff = new List<string>(method_lines_list);
#pragma warning restore IDE0090 // Use 'new(...)'

                foreach (string line__ in understuff)
                {
                    underStuff_lineCounterr++;
                    if (line__ == "}")
                    {
                        last_line_Index = underStuff_lineCounterr;
                        if (method_lines_list.Count != last_line_Index)
                        {
                            try
                            {
                                method_lines_list.RemoveRange(last_line_Index, method_lines_list.Count - last_line_Index);
                                understuff.RemoveRange(0, last_line_Index);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        break;
                    }
                }

                List<string> usingsCode_list = null;
                if (textArray == null && fileloc != null) usingsCode_list = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) usingsCode_list = new List<string>(textArray);
                int usingsCodeList_lineCount = 1;
                foreach (string x in usingsCode_list)
                {
                    if (!x.StartsWith("using") || !x.StartsWith("from"))
                    {
                        break;
                    }
                    usingsCodeList_lineCount++;
                }

                List<string> code_in_method_code = method_lines_list.GetRange(1, last_line_Index - 1);
                List<string> usings_code = usingsCode_list.GetRange(0, usingsCodeList_lineCount);
                // This \/ is just something i added and it just fixes everything
                List<string> usings_code_undercode = usingsCode_list.GetRange(0, usingsCodeList_lineCount);
                usings_code.AddRange(code_in_method_code);
                usings_code_undercode.AddRange(understuff);
                code_in_method_code = usings_code;

                string methodName = code_part_unedited;
                methodName = methodName.Replace("func", "");
                methodName = methodName.TrimStart();
                methodName = methodName[..methodName.IndexOf("(")];

                code_in_method_code.RemoveAt(0);
                code_in_method_code.RemoveAt(code_in_method_code.Count - 1);
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}\\INSTRUCTIONS.txt", string.Join(Environment.NewLine, code_in_method_code.ToArray()));

                prog.CompileCode_fromOtherFiles(null, understuff.ToArray(), 0, true, methodName);
            }
            else
            {
                string methodName = code_part_unedited;
                methodName = methodName.Replace("func", "");
                methodName = methodName.TrimStart();
                methodName = methodName[..methodName.IndexOf("(")];

                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}"))
                {
                    Console.WriteLine("ERROR; Can't Find method, make sure you made the method in your code");
                    return;
                }
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}");

                try
                {
                    prog.CompileCode_fromOtherFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{methodName}\\INSTRUCTIONS.txt", null, 0, true, methodName);
                }
                catch
                {
                    Console.WriteLine("");
                    return;
                }
                List<string> understuff = new List<string>(lines);
                understuff.RemoveRange(understuff.IndexOf(code_part), 1);
                // The Reason why the Above /\ needs to have a "+ 1" at the end is because it will get stuck in a loop, example (CHECK LINE 126);
            }
        }
    }
}
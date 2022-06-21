using System;
using System.Collections.Generic;
using System.IO;

namespace Easy14_Programming_Language
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
        public void interperate(string code_part, string[] lines, string[] textArray, string fileloc, bool isInAMethod = false, string methodName = "}")
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

            /* Removing the lines after the closing bracket of the if statement. */
            List<string> understuff = new List<string>(if_lines_list);
            foreach (string line__ in if_lines_list)
            {
                line_counterr++;
                if (line__ == "}")
                {
                    end_line_IDX = line_counterr - 2;
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

            /* Taking the first line of the if statement and removing the first two characters and the
            last two characters. */
            string[] arr = if_lines_list.ToArray();
            string if_Line = if_lines_list[0];
            if_Line = if_Line.Substring(2);
            if_Line = if_Line.Substring(1, if_Line.Length - 2);

            string obj1 = null; bool obj1_variable = false;
            string obj2 = null; bool obj2_variable = false;

            /* Checking if the if statement is true or false. */
            if (if_Line.TrimEnd().TrimStart() != "(true)" && if_Line.TrimEnd().TrimStart() != "(false)")
            {
                /* Checking if the if_Line contains "==" or "!=" and if it does, it is assigning the value of the string before the "==" or "!=" to the variable obj1. */
                if (if_Line.Contains("=="))
                    obj1 = if_Line.Substring(0, if_Line.IndexOf("==") - 0);
                if (if_Line.Contains("!="))
                    obj1 = if_Line.Substring(0, if_Line.IndexOf("!=") - 0);

                /* Checking if the line contains "==" or "!=" and if it does, it will assign the value of obj2 to the
                value of the line after the "==" or "!=" */

                if (if_Line.Contains("=="))
                    obj2 = if_Line.Substring((if_Line.IndexOf("==") + 3));
                if (if_Line.Contains("!="))
                    obj2 = if_Line.Substring(if_Line.IndexOf("!=") + 2);

                obj1 = obj1.TrimStart().TrimEnd();
                obj2 = obj2.TrimStart().TrimEnd();

                /* Checking if the string is a variable or a string. */
                if (obj1.StartsWith("\"") && obj1.EndsWith("\"")) obj1_variable = false;
                else obj1_variable = true;
                if (obj2.StartsWith("\"") && obj2.EndsWith("\"")) obj2_variable = false;
                else obj2_variable = true;
            }

            /* Compiling the code in the if statement if the codition equals the boolean true. */
            if (if_Line.TrimEnd().TrimStart() == "(true)")
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP";

                /* Finding the first line of code in a file. */
                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;
                /* Counting the number of lines in a file that start with "using" */
                foreach (string x in someLINEs)
                {
                    lin_count++;
                    if (!x.StartsWith("using") && x != "" && x != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                /* Compiling the code in the if statement. */
                List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX - 2);
                List<string> usings_code = someLINEs.GetRange(0, lin_count);
                usings_code.AddRange(Code_in_if_statement_List);
                Code_in_if_statement_List = usings_code;

                if (true)
                {
                    prog.compileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    System.Threading.Thread.Sleep(100);
                }
            }
            /* Checking if the line is (false) and if it is, it will compile the code in the if statement. */
            if (if_Line.TrimEnd().TrimStart() == "(false)")
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP";

                /* Reading the file and storing it in a list. */
                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;
                /* Counting the number of lines in the file that start with "using" */
                foreach (string x in someLINEs)
                {
                    lin_count++;
                    if (!x.StartsWith("using") && x != "" && x != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                /* Taking the lines from the if_lines_list and adding them to the someLINEs list. */
                List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX - 2);
                List<string> usings_code = someLINEs.GetRange(0, lin_count);
                usings_code.AddRange(Code_in_if_statement_List);
                Code_in_if_statement_List = usings_code;

                /* A C# code that is not reachable. */
                if (false)
                {
                    //Yes C#, i wnat this code to be unreachable ok? :)
                    prog.compileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
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
                    Console.ResetColor();
                    return;
                }
                /* Checking if the string contains a double quote. */
                GetVariable getVar = new GetVariable();
                string var1_fileLoc = getVar.findVar(obj1);
                string var2_fileLoc = getVar.findVar(obj2);

                if (var1_fileLoc != "0xF00001") obj1_variable = true;
                if (var2_fileLoc != "0xF00001") obj2_variable = true;

                //Old Way \/
                /*
                if (!obj1.Contains("\""))
                {
                    obj1_variable = true;
                }
                if (!obj2.Contains("\""))
                {
                    obj2_variable = true;
                }*/

                //OLD OLD Way \/
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

                /* Reading the file and storing it in a list. */
                List<string> someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = new List<string>(File.ReadAllLines(fileloc));
                else if (textArray != null && fileloc == null) someLINEs = new List<string>(textArray);
                int lin_count = 1;

                /* Counting the number of lines in a file that start with "using" */
                foreach (string x in someLINEs)
                {
                    lin_count++;
                    if (!x.StartsWith("using") && x != "" && x != null)
                    {
                        break;
                    }
                }

                lin_count = lin_count - 2;

                /* Reading the file content of the two objects. */
                string obj1_fileContent = null;
                string obj2_fileContent = null;
                if (obj1_variable)
                    obj1_fileContent = File.ReadAllText(dir + @$"\\{obj1}.txt");
                if (obj2_variable)
                    obj2_fileContent = File.ReadAllText(dir + @$"\\{obj2}.txt");

                /* Checking if the variable is true and the other variable is false. If it is, it will
                check if the file content is equal to the other variable. If it is, it will add the
                lines to the list. */

                if (obj1_variable && !obj2_variable)
                {
                    if (obj1_fileContent == obj2.Replace("\"", ""))
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, if_lines_list.Count - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.compileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }

                /* Checking if the object is a variable or a string. If it is a variable, it will check
                if the variable is equal to the string. If it is a string, it will check if the
                string is equal to the variable. */

                else if (!obj1_variable && obj2_variable)
                {
                    if (obj1.Replace("\"", "") == obj2_fileContent)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX - 2);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.compileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }

                /// <summary>
                /// It takes a list of strings, and if the first string is a file path, it will compile
                /// the file, and if the second string is a file path, it will compile the file, and if
                /// the two files are the same, it will compile the rest of the strings in the list
                /// </summary>
                /// <param name="obj1_variable">is a string that contains the name of the variable that
                /// is being compared.</param>

                else if (obj1_variable && obj2_variable)
                {
                    if (obj1_fileContent == obj2_fileContent)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.compileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }

                /* Checking if the two objects are equal. */

                else if (!obj1_variable && !obj2_variable)
                {
                    if (obj1.Replace("\"", "") == obj2.Replace("\"", ""))
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.compileCode_fromOtherFiles(fileloc, Code_in_if_statement_List.ToArray());
                    }
                }

                /* Removing the lines of code that are not needed to continue executing the code. */
                //understuff.RemoveRange(0, end_line_IDX - 2);
                List<string> code_toContinueExceuting = new List<string>();
                code_toContinueExceuting.AddRange(usings_lines_list);
                code_toContinueExceuting.AddRange(understuff);

                //int lineIDX_underpart = lineIDX + (code_toContinueExceuting.ToArray().Length - lineIDX);

                if (isInAMethod != true)
                {
                    prog.compileCode_fromOtherFiles(null, code_toContinueExceuting.ToArray());
                }
            }
           


           

            // Seperation so i dont mix up the code





            else if (if_Line.Contains("!="))
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP";
                GetVariable getVar = new GetVariable();
                string var1_fileLoc = getVar.findVar(obj1);
                string var2_fileLoc = getVar.findVar(obj2);

                if (var1_fileLoc != "0xF00001") obj1_variable = true;
                if (var2_fileLoc != "0xF00001") obj2_variable = true;

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

                /* Reading the file content of the two objects. */

                if (obj1_variable)
                {
                    obj1_fileContent = File.ReadAllText(dir + @$"\\{obj1}.txt");
                }
                if (obj2_variable)
                {
                    obj2_fileContent = File.ReadAllText(dir + @$"\\{obj2}.txt");
                }

                /* Checking if the two objects are equal or not. */

                if (obj1_variable == true && obj2_variable == false)
                {
                    if (obj1_fileContent != obj2.Replace("\"", ""))
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.compileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == true)
                {
                    if (obj1.Replace("\"", "") != obj2_fileContent)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.compileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == true && obj2_variable == true)
                {
                    if (obj1_fileContent != obj2_fileContent)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.compileCode_fromOtherFiles(null, Code_in_if_statement_List.ToArray());
                    }
                }
                else if (obj1_variable == false && obj2_variable == false)
                {
                    obj1 = obj1.Substring(0, obj1.Length).Substring(1);
                    obj2 = obj2.Substring(0, obj2.Length).Substring(1);
                    if (obj1 != obj2)
                    {
                        List<string> Code_in_if_statement_List = if_lines_list.GetRange(1, end_line_IDX);
                        List<string> usings_code = someLINEs.GetRange(0, lin_count);
                        usings_code.AddRange(Code_in_if_statement_List);
                        Code_in_if_statement_List = usings_code;
                        prog.compileCode_fromOtherFiles(fileloc, Code_in_if_statement_List.ToArray());
                    }
                }

                /* Finding the line number of the method that is being called. */
                int part_to_continue_at = 1;
                foreach (string e in understuff)
                {
                    if (e == methodName + "();") break;
                    part_to_continue_at++;
                }

                /* Removing the lines of code that are not needed to continue executing the code. */
                //understuff.RemoveRange(0, end_line_IDX - 1);
                List<string> code_toContinueExceuting = new List<string>();
                code_toContinueExceuting.AddRange(usings_lines_list);
                code_toContinueExceuting.AddRange(understuff);

                part_to_continue_at = 1;

                /* Compiling the code that is in the code_toContinueExceuting list. */
                if (isInAMethod != true)
                {
                    prog.compileCode_fromOtherFiles(null, code_toContinueExceuting.ToArray());
                }
            }
        }
    }
}
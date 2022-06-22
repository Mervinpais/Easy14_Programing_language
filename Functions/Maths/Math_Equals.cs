using System;
using System.IO;

namespace Easy14_Programming_Language
{
    class Math_Equals
    {
        /// <summary>
        /// It takes in a string, an int, and a string. It returns a bool.
        /// </summary>
        /// <param name="code_part">The code to be interperated.</param>
        /// <param name="lineNumber">The line number of the code that is being interperated.</param>
        /// <param name="fileName">The name of the file that the code is being interperated
        /// from.</param>
        public bool? interperate(string code_part, int lineNumber, string fileName = null)
        {
            string code_part_unedited;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();

            string expression = code_part;
            if (expression is null && !expression.Contains("==")) {
                Console.WriteLine("ERROR; Can't Compare, please check your code and fix the error");
                return null;
            }
            expression = expression.Replace(" ", "");
            expression = expression.Replace("==", " ");
            //Console.WriteLine(expression);
            string[] intergers = expression.Split(' ');
            bool result = false;
            try {
                Nullable<int> integer1 = null;
                Nullable<int> integer2 = null;
                string integer1_str = null;
                string integer2_str = null;

                if (Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP").Length != 0)
                {
                    string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP");
                    foreach (string file in files)
                    {
                        string _file = file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "");
                        if (_file == intergers[0])
                        {
                            integer1_str = File.ReadAllText(file);
                            //Console.WriteLine(contentInFile.ToString());
                            break;
                        }
                    }
                    foreach (string file in files)
                    {
                        string _file = file.Substring(file.LastIndexOf(@"\")).Replace(@"\", "").Replace(".txt", "");
                        if (_file == intergers[1])
                        {
                            integer2_str = File.ReadAllText(file);
                            //Console.WriteLine(contentInFile.ToString());
                            break;
                        }
                    }
                }

                GetVariable getVar = new GetVariable();
                integer1_str = getVar.findVar(intergers[0]);
                integer2_str = getVar.findVar(intergers[1]);

                if (integer1_str == "0xF000001")
                    integer1 = Convert.ToInt32(intergers[0]);
                if (integer2_str == "0xF000001")
                    integer2 = Convert.ToInt32(intergers[1]);

                //Seperatated Sections

                if (integer1_str != "0xF000001" && integer2_str == "0xF000001")
                    result = Convert.ToInt32(integer1_str) == Convert.ToInt32(integer2);
                else if (integer1_str == "0xF000001" && integer2_str != "0xF000001")
                    result = Convert.ToInt32(integer1) == Convert.ToInt32(integer2_str);
                else if (integer1_str == "0xF000001" && integer2_str == "0xF000001")
                    result = Convert.ToInt32(integer1) == Convert.ToInt32(integer2);
                else if (integer1_str != null && integer2_str != null)
                    result = Convert.ToInt32(integer1_str) == Convert.ToInt32(integer2_str);
            }
            catch
            {
                Console.WriteLine("ERROR; Can't compare that these 2 integers are equal, please check your code and fix the error");
                //Console.WriteLine(sum);
                return null;
            }
            
            /* Saving the result of the math equation to a file. */
            if (fileName is not null) {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{fileName}.txt", result.ToString());
            }
            else if (fileName is null) {
                return result;
            }
            return result;
        }
    }
}
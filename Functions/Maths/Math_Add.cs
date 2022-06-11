using System;
using System.IO;

namespace Easy14_Coding_Language
{
    class Math_Add
    {
        /// <summary>
        /// A function that takes in a string and an int and returns an int.
        /// </summary>
        /// <param name="code_part">The code to be interperated</param>
        /// <param name="lineNumber">The line number of the code_part</param>
        /// <param name="fileName">The name of the file that the code is in.</param>
        public int interperate(string code_part, int lineNumber, string fileName = null)
        {
            string code_part_unedited;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();

            string expression = code_part;

            /* Checking if the expression is null and if it doesn't contain a +, if it is null or
            doesn't contain a + it will return an error. */
            if (expression is null && !expression.Contains("+")) {
                Console.WriteLine("ERROR; Can't Add, please check your code and fix the error");
                return 0;
            }
            expression = expression.Replace(" ", "");
            expression = expression.Replace("=", "");
            expression = expression.Replace(";", "");
            //Console.WriteLine(expression);
            string[] intergers = expression.Split('+');
            int result = 0; //incase the below sum can't be added
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

                if (integer1_str == null)
                    integer1 = Convert.ToInt32(intergers[0]);
                if (integer2_str == null)
                    integer2 = Convert.ToInt32(intergers[1]);
                
                //Seperatated Sections

                /* Checking if the 2 integers are variables or not, if they are variables it will get
                the value of the variable and add them together, if they are not variables it will
                add them together. */
                if (integer1_str == null && integer2_str == null)
                    result = Convert.ToInt32(integer1) + Convert.ToInt32(integer2);
                else if (integer1_str != null && integer2_str != null)
                    result = Convert.ToInt32(integer1_str) + Convert.ToInt32(integer2_str);
                else if (integer1_str == null && integer2_str != null)
                    result = Convert.ToInt32(integer1) + Convert.ToInt32(integer2_str);
                
                /// <summary>
                /// It checks if the first integer is not null and the second integer is null.
                /// </summary>
                /// <param name="integer1_str">The first integer as a string.</param>
                else if (integer1_str != null && integer2_str == null)
                    result = Convert.ToInt32(integer1_str) + Convert.ToInt32(integer2);
            }
            catch
            {
                Console.WriteLine("ERROR; Can't Add these 2 integers, please check your code and fix the error");
                //Console.WriteLine(sum);
                return 0;
            }
            
            /* Checking if the fileName is not null, if it is not null it will write the result to a
            file, if it is null it will return the result. */
            if (fileName is not null) {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP\\{fileName}.txt", result.ToString());
            }
            else if (fileName is null) {
                //Console.WriteLine(result);
            }
            return result;
        }
    }
}
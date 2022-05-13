using System;
using System.IO;

namespace Easy14_Coding_Language
{
    class Math_Multiply
    {
        public int interperate(string code_part, int lineNumber, string fileName = null)
        {
            string code_part_unedited;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();

            string expression = code_part;
            if (expression is null && !expression.Contains("*")) {
                Console.WriteLine("ERROR; Can't Multiply, please check your code and fix the error");
                return 0;
            }
            expression = expression.Replace(" ", "");
            expression = expression.Replace("=", "");
            //Console.WriteLine(expression);
            string[] intergers = expression.Split('*');
            int result = 0; //incase the below sum can't be added
            try {
                int integer1 = Convert.ToInt32(intergers[0]);
                int integer2 = Convert.ToInt32(intergers[1]);
                result = integer1 * integer2;
            }
            catch
            {
                Console.WriteLine("ERROR; Can't Multiply these 2 integers, please check your code and fix the error");
                //Console.WriteLine(sum);
                return 0;
            }
            //Console.WriteLine(sum);
            if (fileName is not null) {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\EASY14_Variables_TEMP\{fileName}.txt", result.ToString());
            }
            else if (fileName is null) {
                //Console.WriteLine(result);
                return result;
            }
            return result;
        }
    }
}
using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class Math_Tan
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static double Interperate(string code_part, int lineNumber, string fileName = null)
        {
            string code_part_unedited;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();

            string expression = code_part.ToLower();
            if (expression is null && !expression.Contains("tan"))
            {
                Console.WriteLine("ERROR; Can't do Tan, please check your code and fix the error");
                return 0.0;
            }
            expression = expression.Replace(" ", "");
            expression = expression.Replace("=", "");
            //Console.WriteLine(expression);
            int theNumber = Convert.ToInt32(expression.Replace("tan(", "").Replace(")", ""));
            //Console.WriteLine(theNumber);
            /* Taking the number that the user inputted and then it is taking the tangent of that
            number. */
            var result = Math.Tan(theNumber);
            return result;
        }
    }
}
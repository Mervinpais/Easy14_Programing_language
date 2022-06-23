using System;
using System.IO;

namespace Easy14_Programming_Language
{
    class Math_SquareRoot
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(strWorkPath + "..\\..\\..\\..\\Application Code", "options.ini"));

        public double interperate(string code_part, int lineNumber, string fileName = null)
        {
            string code_part_unedited;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();

            string expression = code_part.ToLower();
            if (expression is null && !expression.Contains("sqrt")) {
                Console.WriteLine("ERROR; Can't do a Square root on the number, please check your code and fix the error");
                return 0.0;
            }
            expression = expression.Replace(" ", "");
            expression = expression.Replace("=", "");
            //Console.WriteLine(expression);
            int theNumber = Convert.ToInt32(expression.Replace("sqrt(", "").Replace(")", ""));
            //Console.WriteLine(theNumber);
            
            /* Taking the number that the user inputted and then it is taking the square root of it. */
            var result = Math.Sqrt(theNumber);
            return result;
        }
    }
}
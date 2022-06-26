using System;
using System.IO;

namespace Easy14_Programming_Language
{
    class Math_Square
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public double Interperate(string code_part, int lineNumber, string fileName = null)
        {
            string code_part_unedited;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();

            string expression = code_part.ToLower();

            /* Checking if the expression is null and if it doesn't contain the word sq, if it doesn't
            then it will return an error. */
            if (expression is null && !expression.Contains("sq")) {
                Console.WriteLine("ERROR; Can't do Squaring on the number, please check your code and fix the error");
                return 0.0;
            }
            expression = expression.Replace(" ", "");
            expression = expression.Replace("=", "");
            //Console.WriteLine(expression);
            int theNumber = Convert.ToInt32(expression.Replace("sq(", "").Replace(")", ""));
            //Console.WriteLine(theNumber);
            
            /* Squaring the number. */
            var result = theNumber * theNumber;
            return result;
        }
    }
}
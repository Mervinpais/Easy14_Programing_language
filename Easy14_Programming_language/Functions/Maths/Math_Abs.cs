using System;
using System.Diagnostics;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class Math_Abs
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static double Interperate(object code_part, bool ForVariable = false)
        {
            code_part = code_part.ToString().TrimStart();

            string expression = code_part.ToString().ToLower();
            /*
            if (expression is null || !expression.Contains("abs"))
            { Console.WriteLine("ERROR; Can't do Abs, please check your code and fix the error"); return 0.0; }
            */
            expression = expression.Trim();
            if (string.IsNullOrEmpty(expression))
            {
                return 0;
            }

            if (expression.Contains("=")) expression = expression.Replace("=", "");

            if (!expression.StartsWith("abs")) { }
            else
            {
                expression = expression.Replace("abs(", "").Replace(")", "");
            }
            Debug.WriteLine(expression);
            int theNumber = Convert.ToInt32(expression);

            /* Taking the absolute value of the number. */
            var result = Math.Abs(theNumber);
            return result;
        }
    }
}
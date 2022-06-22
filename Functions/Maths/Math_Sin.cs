using System;

namespace Easy14_Programming_Language
{
    class Math_Sin
    {
        /// <summary>
        /// This function takes a string, an integer, and an optional string, and returns a double.
        /// </summary>
        /// <param name="code_part">The code to be interperated</param>
        /// <param name="lineNumber">The line number of the code that is being interperated.</param>
        /// <param name="fileName">The name of the file that the code is in.</param>
        public double interperate(string code_part, int lineNumber, string fileName = null)
        {
            string code_part_unedited;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();

            string expression = code_part.ToLower();
            if (expression is null && !expression.Contains("sin")) {
                Console.WriteLine("ERROR; Can't do Sin, please check your code and fix the error");
                return 0.0;
            }
            expression = expression.Replace(" ", "");
            expression = expression.Replace("=", "");
            //Console.WriteLine(expression);
            int theNumber = Convert.ToInt32(expression.Replace("sin(", "").Replace(")", ""));
            
            /* Taking the number that the user inputted, and then it is taking the sin of that number. */
            var result = Math.Sin(theNumber);
            return result;
        }
    }
}
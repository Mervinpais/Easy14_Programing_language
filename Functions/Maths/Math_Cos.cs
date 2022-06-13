using System;
namespace Easy14_Coding_Language
{
    class Math_Cos
    {
        /// <summary>
        /// This function takes in a string of code, a line number, and a file name, and returns a
        /// double
        /// </summary>
        /// <param name="code_part">The code to be interperated.</param>
        /// <param name="lineNumber">The line number of the code that is being interperated.</param>
        /// <param name="fileName">The name of the file that the code is in.</param>
        public double interperate(string code_part, int lineNumber, string fileName = null)
        {
            string code_part_unedited;

            code_part_unedited = code_part;
            code_part = code_part_unedited.TrimStart();

            string expression = code_part.ToLower();
            if (expression is null && !expression.Contains("cos")) {
                Console.WriteLine("ERROR; Can't do Cos, please check your code and fix the error");
                return 0.0;
            }
            expression = expression.Replace(" ", "");
            expression = expression.Replace("=", "");
            //Console.WriteLine(expression);
            int theNumber = Convert.ToInt32(expression.Replace("cos(", "").Replace(")", ""));
            
            /* Taking the cosine of the number and returning it. */
            var result = Math.Cos(theNumber);
            return result;
        }
    }
}
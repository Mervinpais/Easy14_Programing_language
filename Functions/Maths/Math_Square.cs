using System;
namespace Easy14_Coding_Language
{
    class Math_Square
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
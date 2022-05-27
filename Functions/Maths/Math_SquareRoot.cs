using System;
namespace Easy14_Coding_Language
{
    class Math_SquareRoot
    {
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
            var result = Math.Sqrt(theNumber);
            return result;
        }
    }
}
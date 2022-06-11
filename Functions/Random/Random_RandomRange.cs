using System;
using System.IO;
using System.Linq;

namespace Easy14_Coding_Language
{
    class Random_RandomRange
    {
        private string endOfStatementCode_;
        public string endOfStatementCode
        {
            get { return endOfStatementCode_; }
            set { endOfStatementCode_ = value; }
        }

        /// <summary>
        /// It takes a string, an array of strings, and a string, and returns a string.
        /// </summary>
        /// <param name="code_part">The code that is being interperated</param>
        /// <param name="textArray">The array of strings that contains the code.</param>
        /// <param name="fileloc">The location of the file</param>
        public string interperate(string code_part, string[] textArray, string fileloc)
        {
            string code_part_unedited = code_part;
            bool foundUsing = false;
            if (code_part.StartsWith("RandomRange("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                
                /* Checking if the code has the using Random; statement. */
                foreach (string x in someLINEs)
                {
                    if (x.TrimStart().TrimEnd() == "using Random;")
                    {
                        foundUsing = true;
                        break;
                    }
                    if (x.TrimStart().TrimEnd() == code_part)
                    {
                        break;
                    }
                }
                
                /* Checking if the code has the using Random; statement. */
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Random' wasnt referenced to use 'RandomRange' without its reference  (Use Random.RandomRange() to fix this error :)");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return "<ERROR_USED_FUNCTION_WHICH_NEEDED_A_USING_NAMESPACE_BEFORE>";
                }
            }
            else if (code_part.StartsWith($"Random.RandomRange(")) { }

            /* Removing the `RandomRange(` from the code. */
            if (code_part_unedited.StartsWith($"Random.RandomRange("))
                code_part = code_part.Substring(19);
            else if (code_part_unedited.StartsWith($"RandomRange("))
                code_part = code_part.Substring(12);

            /* Removing the `);` or `)` from the code. */
            if (endOfStatementCode == ");")
                code_part = code_part.Substring(0, code_part.Length - 2);
            else
                code_part = code_part.Substring(0, code_part.Length - 1);
            
            string[] codePart_Array = code_part.Split(',');
            int codePartInt = Convert.ToInt32(codePart_Array[0]);
            int codePartInt2 = Convert.ToInt32(codePart_Array[1]);
            /* Creating a random number between the two numbers. */
            Random rnd = new Random();
            int randomNumber = rnd.Next(codePartInt, codePartInt2);
            string rndNumber_str = randomNumber.ToString();
            return rndNumber_str;
        }
    }
}
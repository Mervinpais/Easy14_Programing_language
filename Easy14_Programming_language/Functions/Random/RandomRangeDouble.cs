using System;
using System.IO;
using System.Linq;

namespace Easy14_Programming_Language
{
    public static class Random_RandomRangeDouble
    {
        public static string Interperate(string code_part, string[] textArray, string fileloc)
        {
            string code_part_unedited = code_part;
            bool foundUsing = false;
            if (code_part.StartsWith("RandomRangeDouble("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the RandomRange function.");
                    Console.ResetColor();
                    return null;
                }

                /* Checking if the code has the using Random; statement. */
                var using_ =
                    from x in someLINEs
                    where x == "using Random;"
                    select x;
                foreach (string x in someLINEs)
                {
                    if (x.TrimStart().TrimEnd() == "using Random;")
                    {
                        foundUsing = true;
                        break;
                    }
                    if (x.TrimStart().TrimEnd() == code_part) break;
                }

                /* Checking if the code has the using Random; statement. */
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Random' wasnt referenced to use 'RandomRange' without its reference  (Use Random.RandomRange() to fix this error :)");
                    Console.ResetColor();
                    return "<ERROR_USED_FUNCTION_WHICH_NEEDED_A_USING_NAMESPACE_BEFORE>";
                }
            }
            else if (code_part.StartsWith($"Random.RandomRangeDouble(")) { }

            /* Removing the `RandomRange(` from the code. */
            if (code_part_unedited.StartsWith($"Random.RandomRangeDouble("))
                code_part = code_part.Substring(19);
            else if (code_part_unedited.StartsWith($"RandomRangeDouble("))
                code_part = code_part.Substring(12);

            /* Removing the `);` or `)` from the code. */
            if (code_part.EndsWith(";"))
                code_part = code_part.Substring(0, code_part.Length - 1);
            if (code_part.EndsWith(")"))
                code_part = code_part.Substring(0, code_part.Length - 1);
            /* Creating a random number between the 0.0 and 1.0 */
            Random rnd = new Random();
            double randomNumber = rnd.NextDouble();
            string rndNumber_str = randomNumber.ToString();
            return rndNumber_str;
        }
    }
}
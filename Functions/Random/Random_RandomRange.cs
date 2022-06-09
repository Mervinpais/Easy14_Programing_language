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

        public string interperate(string code_part, string[] textArray, string fileloc)
        {
            string code_part_unedited = code_part;
            bool foundUsing = false;
            if (code_part.StartsWith("RandomRange("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
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
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Random' wasnt referenced to use 'RandomRange' without its reference  (Use Random.RandomRange() to fix this error :)");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return "<ERROR_USED_FUNCTION_WHICH_NEEDED_A_USING_NAMESPACE_BEFORE>";
                }
            }
            else if (code_part.StartsWith($"Random.RandomRange(")) { }

            if (code_part_unedited.StartsWith($"Random.RandomRange("))
                code_part = code_part.Substring(19);
            else if (code_part_unedited.StartsWith($"RandomRange("))
                code_part = code_part.Substring(12);

            if (endOfStatementCode == ");")
                code_part = code_part.Substring(0, code_part.Length - 2);
            else
                code_part = code_part.Substring(0, code_part.Length - 1);
            
            string[] codePart_Array = code_part.Split(',');
            int codePartInt = Convert.ToInt32(codePart_Array[0]);
            int codePartInt2 = Convert.ToInt32(codePart_Array[1]);
            Random rnd = new Random();
            int randomNumber = rnd.Next(codePartInt, codePartInt2);
            string rndNumber_str = randomNumber.ToString();
            return rndNumber_str;
        }
    }
}
using System;
using System.IO;

namespace Easy14_Coding_Language
{
    class ThrowErrorMessage
    {
        public void sendErrMessage(string text, string[] textArray, string typeOfError )
        {
            //Haha yes i copy full code from ConsolePrint.cs and then modified it, Problem?
            string code_part_unedited;
            if (textArray != null)
                text = string.Join(Environment.NewLine, textArray);
            code_part_unedited = text;
            text = code_part_unedited.TrimStart();
            typeOfError = typeOfError.ToLower();
            if (typeOfError == "error")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {text}");
            }
            if (typeOfError == "warning")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"WARNING: {text}");
            }
            if (typeOfError == "joke")
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                string[] joke_txt = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net5.0", "") + "\\Application Code\\jokesForThrowErrorMessage.txt");
                Random random = new Random();
                int line_to_choose = random.Next(1,joke_txt.Length);
                Console.WriteLine("\n JOKE:" + joke_txt[line_to_choose]);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            return;
        }
    }
}
using System;
using System.IO;

namespace Easy14_Programming_Language
{
    class ThrowErrorMessage
    {
        /// <summary>
        /// It sends an error message.
        /// </summary>
        /// <param name="text">The text to be displayed in the message box.</param>
        /// <param name="textArray">An array of strings that will be used to replace the placeholders in
        /// the text string.</param>
        /// <param name="typeOfError"></param>
        public void sendErrMessage(string text, string[] textArray, string typeOfError )
        {
            /*
            
                ████████╗██╗░░██╗██╗░██████╗  ██╗░██████╗  ██╗░░░██╗███╗░░██╗██╗░░░██╗░██████╗███████╗██████╗░
                ╚══██╔══╝██║░░██║██║██╔════╝  ██║██╔════╝  ██║░░░██║████╗░██║██║░░░██║██╔════╝██╔════╝██╔══██╗
                ░░░██║░░░███████║██║╚█████╗░  ██║╚█████╗░  ██║░░░██║██╔██╗██║██║░░░██║╚█████╗░█████╗░░██║░░██║
                ░░░██║░░░██╔══██║██║░╚═══██╗  ██║░╚═══██╗  ██║░░░██║██║╚████║██║░░░██║░╚═══██╗██╔══╝░░██║░░██║
                ░░░██║░░░██║░░██║██║██████╔╝  ██║██████╔╝  ╚██████╔╝██║░╚███║╚██████╔╝██████╔╝███████╗██████╔╝
                ░░░╚═╝░░░╚═╝░░╚═╝╚═╝╚═════╝░  ╚═╝╚═════╝░  ░╚═════╝░╚═╝░░╚══╝░╚═════╝░╚═════╝░╚══════╝╚═════╝░
            */
            string code_part_unedited;
            if (textArray != null)
                text = string.Join(Environment.NewLine, textArray);
            code_part_unedited = text;
            text = code_part_unedited.TrimStart();
            typeOfError = typeOfError.ToLower();
            
            /* Checking if the type of error is an error or a warning. If it is an error, it will print
            the error in red. If it is a warning, it will print the warning in yellow. */
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
            /* A joke. It will print a random joke from the file jokesForThrowErrorMessage.txt. */
            if (typeOfError == "joke")
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                string[] joke_txt = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Application Code\\jokesForThrowErrorMessage.txt");
                Random random = new Random();
                int line_to_choose = random.Next(1,joke_txt.Length);
                Console.WriteLine("\n JOKE:" + joke_txt[line_to_choose]);
            }
            /* Setting the color of the text to gray. */
            Console.ResetColor();
            return;
        }
    }
}
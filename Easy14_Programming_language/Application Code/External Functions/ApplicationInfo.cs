using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class AppInformation
    {
        public static void ShowInfo()
        {
            Console.ResetColor();
            Console.Write("=== ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("( DEPRICATED )");
            Console.ResetColor();
            Console.Write(" Information of Easy 14 ===");
            Console.WriteLine("\n");
            string[] logo = {
                        "█▀▀▀ █▀▀█ █▀▀ █  █ ▄█  █   █ ",
                        "█▀▀▀ █▄▄█ ▀▀█ █▄▄█  █  █▄▄▄█▄ ",
                        "█▄▄▄ ▀  ▀ ▀▀▀ ▄▄▄█ ▄█▄     █ ",
                        "",
                        "Copyright © Mervinpais14 (Mervinpaismakeswindows14)"
                        };
            Console.WriteLine(String.Join(Environment.NewLine, logo));

            Console.WriteLine("WARNING; This function will be removed in 0.0.33, Dont use this function in the meanwhile");
            return;
            string[] currentVersionFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net7.0", "") + "\\Application Code\\currentVersion.txt");

            string currentVersion = null;
            string dateOfVerInstall = null;
            string BuildTypeOfApp = null;

            bool nextLineEqualsCurrentVer = false;
            bool nextLineEqualsCurrentVerDateInstall = false;

            foreach (string line_inText in currentVersionFile)
            {
                if (nextLineEqualsCurrentVer == true)
                {
                    currentVersion = line_inText;
                    nextLineEqualsCurrentVer = false;
                }
                if (nextLineEqualsCurrentVerDateInstall == true)
                {
                    dateOfVerInstall = line_inText;
                    nextLineEqualsCurrentVerDateInstall = false;
                }

                if (line_inText == "[CURRENT VERSION]")
                {
                    nextLineEqualsCurrentVer = true;
                }
                if (line_inText == "[DATE OF VERSION INSTALLED]")
                {
                    nextLineEqualsCurrentVerDateInstall = true;
                }
            }

            Console.WriteLine($"\n Current Version; {currentVersion}, The Current Version was installed on {dateOfVerInstall} and is a {BuildTypeOfApp}");
            //if (BuildTypeOfApp.Contains("Dev") || BuildTypeOfApp.Contains("Unfinished"))
            //{
            //    Console.ForegroundColor = ConsoleColor.Yellow;
            //    Console.WriteLine("\nWARNING; Using Dev/Unfinished Builds are not safe as they could lead to memory leaks or mess with the filesystem (fun fact; Making variables in Easy14 needs to save to a folder in your system temperary");
            //    Console.ForegroundColor = ConsoleColor.White;
            //}
            Console.WriteLine("\nThanks for reading :)");
        }
    }
}
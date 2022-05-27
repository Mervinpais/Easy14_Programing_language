using System;
using System.IO;

namespace Easy14_Coding_Language
{
    class AppInformation
    {
        public void ShowInfo()
        {
            Console.WriteLine("=== Information of Easy 14 ===");
            string[] logo = {
                        "█▀▀▀ █▀▀█ █▀▀ █  █ ▄█   ▄█▀█ ",
                        "█▀▀▀ █▄▄█ ▀▀█ █▄▄█  █  █▄▄▄█▄ ",
                        "█▄▄▄ ▀  ▀ ▀▀▀ ▄▄▄█ ▄█▄     █ ",
                        "",
                        "Copyright © Mervinpaismakeswindows14"
                        };
            Console.WriteLine(String.Join(Environment.NewLine, logo));
            string[] currentVersionFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net5.0", "") + "\\Application Code\\currentVersion.txt");

            string currentVersion = null;
            string dateOfVerInstall = null;
            string BuildTypeOfApp = null;

            bool nextLineEqualsCurrentVer = false;
            bool nextLineEqualsCurrentVerDateInstall = false;
            bool nextLineEqualsBuildType = false;

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
                if (nextLineEqualsBuildType == true)
                {
                    BuildTypeOfApp = line_inText;
                    nextLineEqualsBuildType = false;
                }

                if (line_inText == "[CURRENT VERSION]")
                {
                    nextLineEqualsCurrentVer = true;
                }
                if (line_inText == "[DATE OF VERSION INSTALLED]")
                {
                    nextLineEqualsCurrentVerDateInstall = true;
                }
                if (line_inText == "[TYPE OF BUILD]")
                {
                    nextLineEqualsBuildType = true;
                }
            }

            Console.WriteLine($"\n Current Version; {currentVersion}, The Current Version was installed on {dateOfVerInstall} and is a {BuildTypeOfApp}");
            if (BuildTypeOfApp.Contains("Dev") || BuildTypeOfApp.Contains("Unfinished"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nWARNING; Using Dev/Unfinished Builds are not safe as they could lead to memory leaks or mess with the filesystem (fun fact; Making variables in Easy14 needs to save to a folder in your system temperary");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("\nThanks for reading :)");
        }
    }
}
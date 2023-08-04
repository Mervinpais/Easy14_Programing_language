using System;
using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class IntroductionCode_old
    {
        public static void IntroCode()
        {
            Console.WriteLine("=== Welcome to Easy 14! ===");
            Console.WriteLine("\n Hey there!, Easy 14 is a simple language for beginners so they can be confortable with other languages in the future if they wanted to change");
            /*Console.WriteLine("\n (NOTE: The language has a Java/C# + Python-ish style so if they wanted to learn say \"html\" (in quotes since it isn't really a 'programming language') or js (Javascript) or heck! even visual basic then you may want to learn that languages syntax "); */
            Console.WriteLine("\n This Language is Built ontop of C# and its easy to start with as a beginner in programming\n");
            string[] logo = {
                        """
                            ______                 ____ __
                           / ____/___ ________  _<  / // /
                          / __/ / __ `/ ___/ / / / / // /_
                         / /___/ /_/ (__  ) /_/ / /__  __/
                        /_____/\__,_/____/\__, /_/  /_/   
                                         /____/           
                            Copyright © Mervinpais14 (Mervinpaismakeswindows14)
                        """
                        };
            Console.WriteLine(String.Join(Environment.NewLine, logo));
            string[] currentVersionFile = { };
            try
            {
                string execAsmParentParentPath = Path.GetDirectoryName(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName);
                string version = Path.Combine(execAsmParentParentPath, @"Application Code\currentVersion.txt");
                currentVersionFile = File.ReadAllLines(execAsmParentParentPath);
            }
            catch
            {
                Console.WriteLine(string.Join("\n", new string[] {
                    """
                      __________
                     /          \
                    |            |
                    | X        X |     Cant Find Version Info, 
                    |     __     |
                     \   /  \   /
                      \________/
                       |______|
                    """
                }));
                return;
            }

            string currentVersion = null;
            string dateOfVerInstall = null;
            string BuildTypeOfApp = null;

            bool nextLineEqualsCurrentVer = false;
            bool nextLineEqualsCurrentVerDateInstall = false;
            bool nextLineEqualsBuildType = false;

            foreach (string line_inText in currentVersionFile)
            {
                if (nextLineEqualsCurrentVer == true)
                    currentVersion = line_inText; nextLineEqualsCurrentVer = false;

                if (nextLineEqualsCurrentVerDateInstall == true)
                    dateOfVerInstall = line_inText; nextLineEqualsCurrentVerDateInstall = false;

                if (nextLineEqualsBuildType == true)
                    BuildTypeOfApp = line_inText; nextLineEqualsBuildType = false;

                if (line_inText == "[CURRENT VERSION]")
                    nextLineEqualsCurrentVer = true;

                if (line_inText == "[DATE OF VERSION INSTALLED]")
                    nextLineEqualsCurrentVerDateInstall = true;

                /*if (line_inText == "[TYPE OF BUILD]")
                    nextLineEqualsBuildType = true;*/
            }

            try
            {
                Console.WriteLine($"\n Current Version; {currentVersion}, The Current Version was installed on {dateOfVerInstall} and is a {BuildTypeOfApp}");
                if (BuildTypeOfApp == null)
                {
                    throw new Exception();
                }
                if (BuildTypeOfApp.Contains("Dev") || BuildTypeOfApp.Contains("Unfinished"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nWARNING; Using Dev/Unfinished Builds are not safe as they could lead to memory leaks or mess with the filesystem (fun fact; Making variables in Easy14 needs to save to a folder in your system temperary");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch
            {
                Console.WriteLine(string.Join("\n", new string[] {
                    """
                      __________
                     /          \
                    |            |
                    | X        X |     e, 
                    |     __     |
                     \   /  \   /
                      \________/
                       |______|
                    """
                }));
                return;
            }
            Console.WriteLine("\nThanks for reading :)");
        }
    }
}
using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class UpdateChecker
    {
        public static void CheckLatestVersion()
        {
            string currentVersion = "v0 - Unknown";

            string[] currentVersionFile = File.ReadAllLines(Path.Combine("Application Code", "currentVersion.txt"));

            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        static void UpdateMessage(string latestUpdateNumber)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string[] UpdateWarningText = {
                "A New Update is available for you!",
                $"Version {latestUpdateNumber} has been released!",
                "You can choose to update now by going to https://github.com/Mervinpais/Easy14_Programing_language to get the update!",
                "\n"
            };
            Console.WriteLine(string.Join(Environment.NewLine, UpdateWarningText));
            Console.ResetColor();
        }
    }
}

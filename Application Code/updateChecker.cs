using System;
// Important Stuff/namespaces
using System.Net;
using System.Diagnostics;
using System.IO;

namespace Easy14_Coding_Language
{
    class updateChecker
    {
        public void checkLatestVersion()
        {
            string[] currentVerFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net5.0", "") + "\\Application Code\\currentVersion.txt");
            double currentVer = Convert.ToDouble(currentVerFile[1]);
            WebClient wc = new WebClient();
            string wot = wc.DownloadString("https://pastebin.com/raw/nETTM1ih");
            string[] upd = wot.Split(',');

            if (Convert.ToDouble(upd[0]) > currentVer)
            {
                //if true, means we got an update
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                string[] updateText = {
                    "A New Update is available for you!",
                    $"Version {upd[0].ToString()} has been released!",
                    "You can choose to update now by going to https://github.com/Mervinpais/Easy14_Programing_language to get the update!",
                    "Or just stay on the same version, and we will tell you when this version is out of support :)\n"
                };
                Console.WriteLine(string.Join(Environment.NewLine, updateText));
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}

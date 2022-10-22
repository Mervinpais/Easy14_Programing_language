using System;
// Important Stuff/namespaces
using System.IO;
using System.Net.Http;

namespace Easy14_Programming_Language
{
    public static class updateChecker
    {
        public static void checkLatestVersion(bool UpdatesWarningsDisabled = false)
        {
            //Initialize Variables
            /* It's initializing the variable `currentVer` to 0.0, so we can use it later. */
            double currentVer = 0.0;

            /* It's just a variable that is used to store the data that we get from the update server,
            and if we can't get the data, it will be set to that. */
            string wot = "<FAILED_TO_GET_UPDATEINFO>";

            try
            {
                /* It's getting the current version of the language, and saving it to a variable. */
                string[] currentVerFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0-windows", "").Replace("\\bin\\Release\\net6.0-windows", "") + "\\Application Code\\currentVersion.txt");
                currentVer = Convert.ToDouble(currentVerFile[1]);

                /* It's saving the current version to a cache file, so if the currentVersion.txt file
                goes missing, it can still get the version from the cache file. */
                try
                {
                    string currentVerFile_cached_FILE = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\..\\Local\\Temp\\EASY14_TEMP\\cachedVersion.txt";

                    if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\..\\Local\\Temp\\EASY14_TEMP"))
                    {
                        Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\..\\Local\\Temp\\EASY14_TEMP", true);
                    }

                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\..\\Local\\Temp\\EASY14_TEMP");
                    File.WriteAllLines(currentVerFile_cached_FILE, currentVerFile);
                }
                catch (Exception e)
                {
                    /* It's sending an error message to the user, telling them that we can't find out
                    what version this language is, and telling them to install the latest version
                    for their safety. */
                    CSharpErrorReporter.ConsoleLineReporter.Error("AN ERROR OCCURED WHEN SAVING TO CACHE (TO SAVE CURRENT VERSION) THIS WILL PREVENT THE LANGUAGE FROM REMEBERING IT'S VERSION IN THE CASE IF THE currentVersion.txt FILE GOES MISSING, ERROR MESSAGE BELOW\n");

                    CSharpErrorReporter.ConsoleLineReporter.Error(e.Message);
                }
            }
            catch
            {
                /* It's checking if the user has disabled the update warnings, and if they have, it
                will not show the warning. */
                if (!UpdatesWarningsDisabled)
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error("Uh oh! We can't find the version file(s)!, using cached version Files...");
                }

                try
                {
                    /* It's getting the current version of the language, and saving it to a variable. */
                    string[] currentVerFile_cached = File.ReadAllLines("C:\\Users\\mervi\\AppData\\Local\\Temp\\EASY14_TEMP\\cachedVersion.txt");
                    currentVer = Convert.ToDouble(currentVerFile_cached[1]);
                }
                catch
                {
                    if (!UpdatesWarningsDisabled)
                        /* It's sending an error message to the user, telling them that we can't find
                        out what version this language is, and telling them to install the latest
                        version for their safety. */
                        CSharpErrorReporter.ConsoleLineReporter.Error("An error occured! we can't find out what version this language is");
                }
            }

            /* It's checking if there is an update available, and if there is, it will tell you. */
            try
            {
                HttpClient hc = new HttpClient();
                if (checkForInternet.IsConnectedToInternet() == true)
                {
                    wot = hc.GetStringAsync("https://pastebin.com/raw/nETTM1ih").Result;
                }
                else
                {
                    CSharpErrorReporter.ConsoleLineReporter.Warning("NO INTENRET", "No Internet Connection Detected to get updates, this is not an error, this is a warning\n");
                    return;
                }
                string[] upd = wot.Split(',');
                upd = upd[0].Split("\r\n");
                if (Convert.ToDouble(upd[1]) > currentVer)
                {
                    //if true, means we got an update
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    string[] updateText = {
                        "A New Update is available for you!",
                        $"Version {upd[1]} has been released!",
                        "You can choose to update now by going to https://github.com/Mervinpais/Easy14_Programing_language to get the update!",
                        "Or just stay on the same version, note that newer versions will/may have bug fixes\n"
                    };
                    Console.WriteLine(string.Join(Environment.NewLine, updateText));
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ResetColor();
                }
            }
            catch
            {
                if (!UpdatesWarningsDisabled)
                {
                    CSharpErrorReporter.ConsoleLineReporter.Error("VERSION ERROR", "Uh oh! we can't check if you have the latest version of this language, please make sure you have the latest version yourself");
                }
            }
        }
    }
}

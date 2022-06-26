using System;
// Important Stuff/namespaces
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace Easy14_Programming_Language
{
    class updateChecker
    {
        /// <summary>
        /// It checks for the latest version of the program.
        /// </summary>
        /// <param name="UpdatesWarningsDisabled">If you want to disable the warning message that
        /// appears when the user is running an outdated version of your app, set this to true.</param>
        public void checkLatestVersion(bool UpdatesWarningsDisabled = false)
        {
            //Initialize Variables
            /* It's initializing the variable `currentVer` to 0.0, so we can use it later. */
            double currentVer = 0.0;
            ThrowErrorMessage tErM = new ThrowErrorMessage();

            /* It's just a variable that is used to store the data that we get from the update server,
            and if we can't get the data, it will be set to that. */
            string wot = "<FAILED_TO_GET_UPDATEINFO>"; //incase we cant get the data this will be the error data

            try
            {
                /* It's getting the current version of the language, and saving it to a variable. */
                string[] currentVerFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "").Replace("\\bin\\Release\\net6.0", "") + "\\Application Code\\currentVersion.txt");
                currentVer = Convert.ToDouble(currentVerFile[1]);

                /* It's saving the current version to a cache file, so if the currentVersion.txt file
                goes missing, it can still get the version from the cache file. */
                try
                {
                    string currentVerFile_cached_FILE = "C:\\Users\\mervi\\AppData\\Local\\Temp\\EASY14_TEMP\\cachedVersion.txt";

                    if (Directory.Exists("C:\\Users\\mervi\\AppData\\Local\\Temp\\EASY14_TEMP"))
                    {
                        Directory.Delete("C:\\Users\\mervi\\AppData\\Local\\Temp\\EASY14_TEMP", true);
                    }
                    
                    Directory.CreateDirectory("C:\\Users\\mervi\\AppData\\Local\\Temp\\EASY14_TEMP");
                    File.WriteAllLines(currentVerFile_cached_FILE, currentVerFile);
                }
                catch (Exception e)
                {
                    /* It's sending an error message to the user, telling them that we can't find out
                    what version this language is, and telling them to install the latest version
                    for their safety. */
                    tErM.sendErrMessage("AN ERROR OCCURED WHEN SAVING TO CACHE (TO SAVE CURRENT VERSION) THIS WILL PREVENT THE LANGUAGE FROM REMEBERING IT'S VERSION IN THE CASE IF THE currentVersion.txt FILE GOES MISSING, ERROR MESSAGE BELOW\n", null, "error");

                    tErM.sendErrMessage(e.Message, null, "error");
                }
            }
            catch
            {
                /* It's checking if the user has disabled the update warnings, and if they have, it
                will not show the warning. */
                if (UpdatesWarningsDisabled)
                    tErM.sendErrMessage("Uh oh! We can't find the version file(s)!, using cached version Files...", null, "warning");

                try
                {
                    /* It's getting the current version of the language, and saving it to a variable. */
                    string[] currentVerFile_cached = File.ReadAllLines("C:\\Users\\mervi\\AppData\\Local\\Temp\\EASY14_TEMP\\cachedVersion.txt");
                    currentVer = Convert.ToDouble(currentVerFile_cached[1]);
                }
                catch
                {
                    if (UpdatesWarningsDisabled)
                        /* It's sending an error message to the user, telling them that we can't find
                        out what version this language is, and telling them to install the latest
                        version for their safety. */
                        tErM.sendErrMessage("An error occured! we can't find out what version this language is", null, "error");
                }
            }

            /* It's checking if there is an update available, and if there is, it will tell you. */
            try
            {
                HttpClient hc = new HttpClient();
                wot = hc.GetStringAsync("https://pastebin.com/raw/nETTM1ih").Result;
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
                    Console.ResetColor();
                }
            }
            catch
            {
                if (UpdatesWarningsDisabled)
                    tErM.sendErrMessage("Uh oh! we can't check if you have the latest version of this language, please make sure you have the latest version yourself", null, "error");
            }
        }
    }
}

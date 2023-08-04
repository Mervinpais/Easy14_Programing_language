using System;
using System.Diagnostics;
// Important Stuff/namespaces
using System.IO;
using System.Net.Http;

namespace Easy14_Programming_Language
{
    public static class updateChecker
    {
        public static void checkLatestVersion()
        {
            //Initialize Variables
            /* It's initializing the variable `currentVer` to 0.0, so we can use it later. */
            double currentVer = 0.0;

            /* It's just a variable that is used to store the data that we get from the update server,
            and if we can't get the data, it will be set to that. */
            string DataFromServer = "<FAILED_TO_GET_UPDATEINFO>";

            string[] currentVerFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net7.0-windows", "").Replace("\\bin\\Release\\net7.0-windows", "") + "\\Application Code\\currentVersion.txt");
            try
            {

                //currentVer = Convert.ToDouble(dt);

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
                    ErrorReportor.ConsoleLineReporter.Error("AN ERROR OCCURED WHEN SAVING TO CACHE (TO SAVE CURRENT VERSION) THIS WILL PREVENT THE LANGUAGE FROM REMEBERING IT'S VERSION IN THE CASE IF THE currentVersion.txt FILE GOES MISSING, ERROR MESSAGE BELOW\n");

                    ErrorReportor.ConsoleLineReporter.Error(e.Message);
                }
            }
            catch (Exception)
            {
                /* It's checking if the user has disabled the update warnings, and if they have, it
                will not show the warning. */
                if (!Convert.ToBoolean(Configuration.GetBoolOption("UpdatesWarningsDisabled")))
                {
                    ErrorReportor.ConsoleLineReporter.Error("Uh oh! We can't find the version file(s)!, using cached version Files...");
                }

                try
                {
                    /* It's getting the current version of the language, and saving it to a variable. */
                    string[] currentVerFile_cached = File.ReadAllLines("C:\\Users\\mervi\\AppData\\Local\\Temp\\EASY14_TEMP\\cachedVersion.txt");
                    currentVer = Convert.ToDouble(currentVerFile_cached[1]);
                }
                catch
                {
                    if (!Convert.ToBoolean(Configuration.GetBoolOption("UpdatesWarningsDisabled")))
                        /* It's sending an error message to the user, telling them that we can't find
                        out what version this language is, and telling them to install the latest
                        version for their safety. */
                        ErrorReportor.ConsoleLineReporter.Error("An error occured! we can't find out what version this language is");
                }
            }

            void SendUpdateWarning(string latestVersionStr)
            {
                //if true, means we got an update
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                string[] updateText = {
                        "A New Update is available for you!",
                        $"Version {latestVersionStr} has been released!",
                        "You can choose to update now by going to https://github.com/Mervinpais/Easy14_Programing_language to get the update!",
                        "Or just stay on the same version, note that newer versions will/may have bug fixes\n"
                    };
                Console.WriteLine(string.Join(Environment.NewLine, updateText));
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ResetColor();
            }

            /* It's checking if there is an update available, and if there is, it will tell you. */
            try
            {
                HttpClient clientToGetVersion = new HttpClient();
                if (checkForInternet.IsConnectedToInternet() == true)
                {
                    DataFromServer = clientToGetVersion.GetStringAsync("https://pastebin.com/raw/nETTM1ih").Result;
                }
                else
                {
                    ErrorReportor.ConsoleLineReporter.Warning("NO INTENRET", "No Internet Connection Detected for updates \n");
                    return;
                }
                string[] UpdateInfo = DataFromServer.Split("\r\n");
                string[] localComponents = currentVerFile[1].Split('.');
                string[] serverComponents = UpdateInfo[1].Split('.');

                if (int.Parse(localComponents[0]) > int.Parse(serverComponents[0]))
                {
                    Debug.WriteLine("Major Check; Local version is newer than server version");
                }
                else if (int.Parse(localComponents[0]) == int.Parse(serverComponents[0]))
                {
                    if (int.Parse(localComponents[1]) > int.Parse(serverComponents[1]))
                    {
                        Debug.WriteLine("Minor Check; Local version is newer than server version");
                    }
                    else if (int.Parse(localComponents[1]) == int.Parse(serverComponents[1]))
                    {
                        if (int.Parse(localComponents[2]) > int.Parse(serverComponents[2]))
                        {
                            Debug.WriteLine("Patch Check; Local version is newer than server version");
                        }
                        else if (int.Parse(localComponents[2]) == int.Parse(serverComponents[2]))
                        {
                            Debug.WriteLine("Patch Check; Local version is the same as server version");
                        }
                        else
                        {
                            Debug.WriteLine("Patch Check; Local version is older than server version");
                            SendUpdateWarning(UpdateInfo[1]);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Minor Check; Local version is older than server version");
                    }
                }
                else
                {
                    Console.WriteLine("Major Check; Local version is older than server version");
                }
            }
            catch
            {
                if (!Convert.ToBoolean(Configuration.GetBoolOption("UpdatesWarningsDisabled")))
                {
                    ErrorReportor.ConsoleLineReporter.Error("VERSION ERROR", "Uh oh! we can't check if you have the latest version of this language, please make sure you have the latest version yourself");
                }
            }
        }
    }
}

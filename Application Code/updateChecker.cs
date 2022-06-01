using System;
// Important Stuff/namespaces
using System.Net;
using System.Diagnostics;
using System.IO;

namespace Easy14_Coding_Language
{
    class updateChecker
    {
        public void checkLatestVersion(bool UpdatesWarningsDisabled = false)
        {
            //Initialize Variables
            double currentVer = 0.0;
                  ThrowErrorMessage tErM = new ThrowErrorMessage();

            string wot = "<FAILED_TO_GET_UPDATEINFO>"; //incase we cant get the data this will be the error data

            try {
              string[] currentVerFile = File.ReadAllLines(Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net5.0", "") + "\\Application Code\\currentVersion.txt");
              currentVer = Convert.ToDouble(currentVerFile[1]);
            }
            catch
            {
                      if (UpdatesWarningsDisabled)
                          tErM.sendErrMessage("Uh oh! We can't find the version file(s)!, using cached version Files...", null, "warning");
                      try {
                          string[] currentVerFile_cached = File.ReadAllLines("C:\\Users\\mervi\\AppData\\Local\\Temp\\EASY14_TEMP\\cachedVersion.txt");
                          currentVer = Convert.ToDouble(currentVerFile_cached[1]);
                      }
                      catch {
                          if (UpdatesWarningsDisabled)
                              tErM.sendErrMessage("AN ERROR OCCURED! WE CAN'T FIND OUT WHAT VERSION THIS LANGUAGE IS, REINSTALL APP TO FIX THIS ISSUE! (OR YOU CAN DISABLE THIS WARNING IN OPTIONS.INI)", null, "error");
                      }
            }

            try {
                WebClient wc = new WebClient();
                wot = wc.DownloadString("https://pastebin.com/raw/nETTM1ih");
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
            catch {
                if (UpdatesWarningsDisabled) 
                    tErM.sendErrMessage("Uh oh! we can't check if you have the latest version of this language, please make sure you have the latest version yourself", null, "error");
            }
        }
    }
}

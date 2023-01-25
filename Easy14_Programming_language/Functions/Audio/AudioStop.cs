using System;
using System.IO;
using System.Media;

namespace Easy14_Programming_Language
{
    public static class AudioStop
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static void Interperate(string code_part, string[] textArray = null, string fileloc = null)
        {
            if (code_part.StartsWith("AudioStop("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else
                {
                    ErrorReportor.ConsoleLineReporter.Error("Error: No file or text array was provided to the Console.beep function.");
                }

                bool foundUsing = false;
                foreach (string usingStatements in someLINEs)
                {
                    if (usingStatements.TrimStart().TrimEnd() == "using Audio;")
                    {
                        foundUsing = true;
                        break;
                    }
                    if (usingStatements.TrimStart().TrimEnd() == code_part)
                    {
                        break;
                    }
                }
                if (foundUsing == false)
                {
                    ErrorReportor.ConsoleLineReporter.Error($"The Using 'Audio' wasnt referenced to use 'stop' without its reference");
                }
            }
            else if (code_part.StartsWith($"Audio.AudioStop(")) { }

            if (code_part.EndsWith(");"))
            {
                code_part = code_part.Substring(0, code_part.Length - 2);
            }
            else
            {
                ErrorReportor.ConsoleLineReporter.Error("Failed to End statement");
            }

            try
            {
                SoundPlayer soundPlayer = new SoundPlayer();
                soundPlayer.Stop();
            }
            catch (Exception)
            {
                ErrorReportor.ConsoleLineReporter.Error("Failed to stop audio");
            }

            return;
        }
    }
}
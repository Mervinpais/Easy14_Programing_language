using System;
using System.IO;
using System.Media;

namespace Easy14_Programming_Language
{
    public static class AudioPlay
    {
        static string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        static string strWorkPath = Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static void Interperate(string code_part = null, string[] textArray = null, string fileloc = null)
        {
            if (code_part == null && textArray == null && fileloc == null)
            {
                ThrowErrorMessage.sendErrMessage("No parameters specified!", null, "error");
                return;
            }
            string endOfStatementCode = ")";
            foreach (string line in configFile)
            {
                if (line.StartsWith("needSemicolons"))
                    endOfStatementCode.Equals(line.EndsWith("true") ? endOfStatementCode = ");" : endOfStatementCode = ")");
                break;
            }

            string code_part_unedited = code_part;
            bool foundUsing = false;
            if (code_part.StartsWith("play("))
            {
                string[] someLINEs = null;
                if (textArray == null && fileloc != null) someLINEs = File.ReadAllLines(fileloc);
                else if (textArray != null && fileloc == null) someLINEs = textArray;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or text array was provided to the Console.beep function.");
                    Console.ResetColor();
                }

                foreach (string x in someLINEs)
                {
                    if (x.TrimStart().TrimEnd() == "using Console;")
                    {
                        foundUsing = true;
                        break;
                    }
                    if (x.TrimStart().TrimEnd() == code_part)
                    {
                        break;
                    }
                }
                if (foundUsing == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR; The Using 'Console' wasnt referenced to use 'beep' without its reference  (Use Console.Beep() to fix this error :)");
                    Console.ResetColor();
                }
            }
            else if (code_part.StartsWith($"Audio.AudoPlay(")) { }

            if (code_part_unedited.StartsWith($"Audio.AudioPlay("))
                code_part = code_part.Substring(16);
            else if (code_part_unedited.StartsWith($"AudioPlay("))
                code_part = code_part.Substring(10);

            if (code_part.EndsWith(";"))
            {
                code_part = code_part.Substring(0, code_part.Length - 1);
            }
            if (code_part.EndsWith(")"))
            {
                code_part = code_part.Substring(0, code_part.Length - 1);
            }

            if (!code_part.Contains(","))
            {
                SoundPlayer soundPlayer = new SoundPlayer(code_part);
                soundPlayer.Load();
                soundPlayer.Play();
            }
            else
            {

            }
            return;
        }
    }
}
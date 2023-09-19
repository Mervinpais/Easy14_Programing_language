using System;
using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class Configuration
    {
        private static string[] configFile;

        static Configuration()
        {
            string exeLocation = Assembly.GetExecutingAssembly().Location;
            string workingDirectory = Path.GetDirectoryName(exeLocation);
            string optionsPath = Path.Combine(workingDirectory, "Application Code", "options.ini");
            configFile = File.ReadAllLines(optionsPath);
        }

        public static bool GetBoolOptionValue(string optionName)
        {
            foreach (string line in configFile)
            {
                if (line == $"{optionName}:true") { return true; }
                else if (line == $"{optionName}:false") { return true; }
            }
            return false;
        }

        public static string GetStringOptionValue(string optionName)
        {
            foreach (string line in configFile)
            {
                if (line == $"{optionName}:{line.Substring($"{optionName}:".Length).Trim()}")
                {
                    return line.Substring($"{optionName}:".Length).Trim();
                }
            }
            return "";
        }

        public static int GetIntOptionValue(string optionName)
        {
            foreach (string line in configFile)
            {
                if (line == $"{optionName}:{line.Substring($"{optionName}:".Length).Trim()}")
                {
                    bool isInt = int.TryParse(line.Substring($"{optionName}:".Length).Trim(), out _);
                    if (isInt) return Convert.ToInt32(line.Replace($"{optionName}:", "").Trim());
                    else throw new UnableToConvertException();
                }
            }
            return -1;
        }
    }
}

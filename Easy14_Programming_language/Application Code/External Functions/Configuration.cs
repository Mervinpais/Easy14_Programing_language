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
                if (line.StartsWith($"{optionName}:true", StringComparison.OrdinalIgnoreCase)) { return true; }
                else if (line.StartsWith($"{optionName}:false", StringComparison.OrdinalIgnoreCase)) { return false; }
            }
            return false;
        }

        public static string GetStringOptionValue(string optionName)
        {
            foreach (string line in configFile)
            {
                if (line.StartsWith($"{optionName}:", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Substring(optionName.Length + 1).Trim();
                }
            }
            return "";
        }

        public static int GetIntOptionValue(string optionName)
        {
            foreach (string line in configFile)
            {
                if (line.StartsWith($"{optionName}:", StringComparison.OrdinalIgnoreCase))
                {
                    string value = line.Substring(optionName.Length + 1).Trim();
                    if (int.TryParse(value, out int intValue))
                    {
                        return intValue;
                    }
                    else
                    {
                        //throw new UnableToConvertException();
                        return -1;
                    }
                }
            }
            return -1;
        }
    }
}

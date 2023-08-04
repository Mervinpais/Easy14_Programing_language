using System;
using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class Configuration
    {
        private static readonly string exeFilePath = Assembly.GetExecutingAssembly().Location;
        private static readonly string workPath = Path.GetDirectoryName(exeFilePath);
        private static string[] configFile;

        static Configuration()
        {
            string optionsPath = "Application Code\\options.ini";
            configFile = File.ReadAllLines(optionsPath);
        }

        public static object GetBoolOption(string optionName, bool getIntValue = false)
        {
            object returnVal = null;
            foreach (string line in configFile)
            {
                if (line.StartsWith(optionName))
                {
                    if (line.EndsWith("true")) returnVal = true;
                    else if (line.EndsWith("false")) returnVal = false;
                    else returnVal = line.Replace($"{optionName} =", "").Trim();
                }
            }
            if (getIntValue) returnVal = Convert.ToInt32(returnVal);
            return returnVal;
        }

        public static object GetIntOption(string optionName, bool getBoolValue = false)
        {
            foreach (string line in configFile)
            {
                if (line.StartsWith(optionName))
                {
                    bool isInt = int.TryParse(line.Replace($"{optionName} =", "").Trim(), out _);
                    if (isInt)
                    {
                        return line.Replace($"{optionName} =", "").Trim();
                    }
                    else
                    {
                        throw new UnableToConvertException();
                    }
                }
            }
            return -1;
        }
    }
}

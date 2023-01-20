using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class Easy14_configuration
    {
        private static readonly string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        private static readonly string strWorkPath = Path.GetDirectoryName(strExeFilePath);
        private static string[] configFile;

        static Easy14_configuration()
        {
            string optionsPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName, "Application Code", "options.ini");
            configFile = File.ReadAllLines(optionsPath);
        }

        public static object GetBoolOption(string optionName)
        {
            foreach (string item in configFile)
            {
                if (item.StartsWith(optionName))
                {
                    if (item.EndsWith("true"))
                    {
                        return true;
                    }
                    else if (item.EndsWith("false"))
                    {
                        return false;
                    }
                    else
                    {
                        return item.Replace($"{optionName} =", "").Trim();
                    }
                }
            }
            return false;
        }

        public static object GetIntOption(string optionName)
        {
            foreach (string item in configFile)
            {
                if (item.StartsWith(optionName))
                {
                    if (int.TryParse(item.Replace($"{optionName} =", "").Trim(), out _))
                    {
                        return item.Replace($"{optionName} =", "").Trim();
                    }
                    else
                    {
                        throw new UnableToConvertException(); //Not yet finished
                    }
                }
            }
            return "";
        }
    }
}

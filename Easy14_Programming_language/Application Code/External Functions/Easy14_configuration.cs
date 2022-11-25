using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class Easy14_configuration
    {
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static object getBoolOption(string optionName)
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
                        return item.Replace($"{optionName} =", "");
                    }
                }
            }
            return false;
        }
        public static object getIntOption(string optionName)
        {
            foreach (string item in configFile)
            {
                if (item.StartsWith(optionName))
                {
                    if (int.TryParse(item.Replace($"{optionName} =", ""), out _) == true)
                    {
                        return item.Replace($"{optionName} =", "");
                    }
                    else if (int.TryParse(item.Replace($"{optionName} =", ""), out _) == false)
                    {
                        throw new UnableToConvertException(); //Not yet finished
                    }
                    else
                    {
                        return item.Replace($"{optionName} =", "");
                    }
                }
            }
            return "";
        }
    }
}

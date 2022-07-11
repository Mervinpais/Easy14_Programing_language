using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class Easy14_configuration
    {
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);
        static string[] configFile = File.ReadAllLines(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(strWorkPath).FullName).FullName).FullName + "\\Application Code", "options.ini"));

        public static string getOption(string optionName)
        {
            foreach (string item in configFile)
            {
                if (item.StartsWith(optionName))
                {
                    if (item.EndsWith("true"))
                    {
                        return "true";
                    }
                    else if (item.EndsWith("false"))
                    {
                        return "false";
                    }
                    else
                    {
                        return item.Replace($"{optionName} =", "");
                    }
                }
            }
            return "false";
        }
    }
}

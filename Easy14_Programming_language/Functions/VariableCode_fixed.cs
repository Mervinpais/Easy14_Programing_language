using System;
using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class VariableCode
    {
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);

        public static void Interperate(string name, object value)
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP");
            }

            string[] value_split = value.ToString().Split(' ');
            if (Convert.ToString(value).StartsWith("\"") && Convert.ToString(value).EndsWith("\""))
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{name}", Convert.ToString(value));
            }
            else
            {
                if (value_split[0] == "Console")
                {
                    if (value_split[1] == "Input")
                    {
                        ConsoleInput.Interperate("");
                    }
                }
            }
        }
    }
}
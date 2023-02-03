using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Easy14_Programming_Language
{
    public static class VariableCode
    {
        readonly static string strExeFilePath = Assembly.GetExecutingAssembly().Location;
        readonly static string strWorkPath = Path.GetDirectoryName(strExeFilePath);
        public static Dictionary<string, object> variableList = new Dictionary<string, object>();

        public static void Interperate(string name, object value)
        {
            //if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP"))
            //{
            //    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\EASY14_Variables_TEMP");
            //}

            string[] value_split = value.ToString().Split(' ');
            value = value.ToString().Substring(0, value.ToString().Length - 1);
            if (Convert.ToString(value).StartsWith("\"") && Convert.ToString(value).EndsWith("\""))
            {
                try
                {
                    variableList.Add(name, value);
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("An item with the same key has already been added"))
                    {
                        ErrorReportor.Logger.Error(ex, "A Variable with the same Name already exists!");
                    }
                }
                //File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\\EASY14_Variables_TEMP\\{name}", Convert.ToString(value));
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
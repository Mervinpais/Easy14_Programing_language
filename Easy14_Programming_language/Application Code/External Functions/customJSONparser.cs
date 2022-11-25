using System.Collections.Generic;
using System.IO;

namespace Easy14_Programming_Language
{
    //shhh... let's not leak any work here








    public static class CustomJSONparser
    {
        public static void encode(string filePath, string[] data)
        {
            return;
#pragma warning disable CS0162 // Unreachable code detected
            List<string> data_list = new List<string>(data);
#pragma warning restore CS0162 // Unreachable code detected
            List<string> NEWdata_list = new List<string>(data);
            for (int i = 0; i < data_list.Count - 1; i++)
            {
                string str = data_list[i];
                NEWdata_list.Add("\"" + str.Split(",")[0] + "\" : \"" + str.Split(",")[1] + "\",");
            }
            string last_str = data_list[data_list.Count];
            NEWdata_list.Add("\"" + last_str.Split(",")[0] + "\" : \"" + last_str.Split(",")[1] + "\"");
            File.WriteAllLines(filePath, NEWdata_list.ToArray());
        }
    }
}

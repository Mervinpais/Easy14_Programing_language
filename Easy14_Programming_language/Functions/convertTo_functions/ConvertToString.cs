using System;

namespace Easy14_Programming_Language
{
    public static class ConvertToString
    {
        public static string Interperate(object val)
        {
            if (val.ToString().StartsWith("ToString(")) val = val.ToString().Substring(8);
            if (val.ToString().EndsWith(");")) val = val.ToString().Substring(1, val.ToString().Length - 3);
            if (val.ToString().EndsWith("\"") && val.ToString().StartsWith("\"")) val = val.ToString().Substring(1, val.ToString().Length - 2);
            try
            {
                return Convert.ToString(val);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
            return "";
        }
    }
}

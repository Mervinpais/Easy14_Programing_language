using System;

namespace Easy14_Programming_Language
{
    /* A class that converts a value to a string. */
    public static class ConvertToString
    {
        public static string Interperate(object val)
        {
            if (val.ToString().StartsWith("ToString("))
            {
                val = val.ToString().Substring(8);
            }
            if (val.ToString().EndsWith(");"))
            {
                val = val.ToString().Substring(1, val.ToString().Length - 3);
            }
            if (val.ToString().EndsWith("\"") && val.ToString().StartsWith("\""))
            {
                val = val.ToString().Substring(1, val.ToString().Length - 2);
            }

            try
            {
                return Convert.ToString(val);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
#pragma warning disable CS0162 // Unreachable code detected
            return "";
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}

using System;

namespace Easy14_Programming_Language
{
    /* A class that converts a value to a string. */
    public static class ConvertToString
    {
        public static string Interperate(object val)
        {
            Console.WriteLine("Start");
            if (string.Join("", val).StartsWith("ConvertToString("))
            {
                val = val.ToString().Substring(8);
            }
            if (string.Join("", val).EndsWith(");"))
            {
                val = val.ToString().Substring(1, val.ToString().Length - 3);
            }
            if (string.Join("", val).EndsWith("\"") && val.ToString().StartsWith("\""))
            {
                val = val.ToString().Substring(1, val.ToString().Length - 2);
            }
            Console.WriteLine("End");
            try
            {
                return Convert.ToString(val);
            }
            catch (Exception)
            {
                throw new UnableToConvertException("Failed to convert OBJECT to STRING");
                return "";
            }
            return "";
        }
    }
}

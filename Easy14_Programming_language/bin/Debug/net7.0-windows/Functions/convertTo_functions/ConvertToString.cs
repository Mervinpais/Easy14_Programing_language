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
                val = val.ToString()[8..];
            }
            if (string.Join("", val).EndsWith(");"))
            {
                val = val.ToString()[1..^2];
            }
            if (string.Join("", val).EndsWith("\"") && val.ToString().StartsWith("\""))
            {
                val = val.ToString()[1..^1];
            }
            Console.WriteLine("End");
            try
            {
                return Convert.ToString(val);
            }
            catch (Exception)
            {
                throw new UnableToConvertException("Failed to convert OBJECT to STRING");
            }
        }
    }
}

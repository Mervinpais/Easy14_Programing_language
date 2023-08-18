using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class Time_CurrentTime
    {
        public static string Interperate()
        {
            DateTime date = DateTime.Now;
            string date_str = Convert.ToString(date);
            return date_str;
        }
    }
}
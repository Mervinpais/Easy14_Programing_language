using System;
using System.IO;

namespace Easy14_Programming_Language
{
    public static class Time_IsLeapYear
    {
        public static bool Interperate(object date)
        {
            return DateTime.IsLeapYear(Convert.ToInt32(date));
        }
    }
}
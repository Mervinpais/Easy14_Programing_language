using System;
using System.Linq;

namespace Easy14_Programming_Language
{
    public static class Random_RandomRangeDouble
    {
        public static string Interperate()
        {
            Random rnd = new Random();
            double randomNumber = rnd.NextDouble();
            string rndNumber_str = randomNumber.ToString();
            return rndNumber_str;
        }
    }
}
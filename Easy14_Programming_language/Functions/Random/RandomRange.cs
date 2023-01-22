using System;

namespace Easy14_Programming_Language
{
    public static class Random_RandomRange
    {
        public static string Interperate(string line)
        {
            string[] line_split = line.Split(' ');
            Random rnd = new Random();
            int randomNumber = rnd.Next(Convert.ToInt32(line_split[0]), Convert.ToInt32(line_split[1]));
            string rndNumber_str = randomNumber.ToString();
            return rndNumber_str;
        }
    }
}
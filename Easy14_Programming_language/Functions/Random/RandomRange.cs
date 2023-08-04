using System;

namespace Easy14_Programming_Language
{
    public static class Random_RandomRange
    {
        public static int Interperate(int min, int max)
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(min, max);
            return randomNumber;
        }
    }
}
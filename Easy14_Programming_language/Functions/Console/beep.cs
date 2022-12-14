using System;

namespace Easy14_Programming_Language
{
    public static class ConsoleBeep
    {
        public static void Interperate(string line)
        {
            int frequency = Convert.ToInt32(line.Split(' ')[0]);
            int duration = Convert.ToInt32(line.Split(' ')[1]) * 1000; //NOTE; 1 second = 1000 ms and Console.Beep uses miliseconds

            if (frequency > 32767 || frequency < 37)
            {
                Console.Beep(2000, duration); //Default
                return;
            }

            Console.Beep(frequency, duration);
            return;
        }
    }
}
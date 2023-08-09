using System;
using System.Collections.Generic;
using System.IO;
using System.Media;

namespace Easy14_Programming_Language
{
    public static class AudioPlay
    {
        public static Dictionary<int, SoundPlayer> audioPlayersList = new Dictionary<int, SoundPlayer>();

        public static void Interperate(string audioFileToPlay)
        {
            try
            {
                SoundPlayer soundPlayer = new SoundPlayer(audioFileToPlay);
                soundPlayer.Load();
                audioPlayersList.Add(audioPlayersList.Count+1, soundPlayer);
                soundPlayer.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error; {ex.Message}");
            }
            return;
        }
    }
}
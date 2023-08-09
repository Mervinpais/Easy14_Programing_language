using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Media;

namespace Easy14_Programming_Language
{
    public static class AudioStop
    {
        public static void Interperate(int indexOfAudioPlayer)
        {
            try
            {
                AudioPlay.audioPlayersList[indexOfAudioPlayer].Stop();
            }
            catch (Exception)
            {
                ErrorReportor.ConsoleLineReporter.Error("Failed to stop audio");
            }

            return;
        }
    }
}
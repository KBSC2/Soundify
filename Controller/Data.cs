using Model;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace Controller
{
    public static class Data
    {
        public static IWavePlayer WaveOutDevice { get; set; }
        public static Song CurrentSong { get; set; }

        public static void Initialize()
        {
            WaveOutDevice = new WaveOut();
        }

        public static void PlaySong(Song song)
        {
            CurrentSong = song;
            WaveOutDevice.Init(song.AudioFile);

            //PowerShell.
        }
    }
}

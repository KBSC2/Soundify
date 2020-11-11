using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Song
    {
        public AudioFileReader AudioFile { get; set; }

        public double TotalTimeSong { 
            get { 
                return AudioFile.TotalTime.TotalSeconds;
            } 
        }
        public double CurrentTimeSong { 
            get {
                return AudioFile.CurrentTime.TotalSeconds;
            } 
        }

        public Song(AudioFileReader audioFile)
        {
            AudioFile = audioFile;
        }
    }
}

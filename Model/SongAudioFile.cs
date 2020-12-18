using NAudio.Wave;

namespace Model
{
    public class SongAudioFile
    {
        public AudioFileReader AudioFile { get; set; }

        public double CurrentTimeSong => AudioFile.CurrentTime.TotalSeconds;

        public SongAudioFile(string audioFile)
        {
            AudioFile = new AudioFileReader(audioFile);
        }
    }
}

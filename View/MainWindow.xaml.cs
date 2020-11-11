using NAudio.Wave;
using NAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;
using System.Timers;
using Controller;
using System.Runtime.CompilerServices;
using Model;

namespace Soundify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            Data.Initialize();
            Data.PlaySong(new Song(new AudioFileReader("dansenaandegracht.mp3")));
            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Data.WaveOutDevice.PlaybackState == PlaybackState.Paused || Data.WaveOutDevice.PlaybackState == PlaybackState.Stopped)
            {
                Data.WaveOutDevice.Play();
                Play.Content = "=";
            }
            else
            {
                Data.WaveOutDevice.Pause();
                Play.Content = ">";
            }
        }

        private void Duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            Data.CurrentSong.AudioFile.Skip((int)(slider.Value - Data.CurrentSong.CurrentTimeSong));

        }
    }
}

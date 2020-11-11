﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using Model;

namespace Tests
{
    [TestFixture]
    public class Controller_AudioPlayerShould
    {
        [SetUp]
        public void SetUp()
        {
            AudioPlayer.Initialize();
        }

        [Test]
        public void PlaySong_PlaybackState_Playing()
        {
            AudioPlayer.PlaySong(new SongAudioFile("dansenaandegracht.mp3"));
            Assert.IsTrue(AudioPlayer.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing);
        }

        [Test]
        public void AddSong_Queue_Contains()
        {
            SongAudioFile songAudioFile = new SongAudioFile("dansenaandegracht.mp3");
            AudioPlayer.AddSong(songAudioFile);
            Assert.IsTrue(AudioPlayer.SongQueue.Contains(songAudioFile));
        }
    }
}
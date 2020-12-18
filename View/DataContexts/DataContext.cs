﻿using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using View.Components;

namespace View.DataContexts
{
    public class DataContext : INotifyPropertyChanged
    {
        public static List<PermissionButton> PermissionButtons { get; set; } = new List<PermissionButton>();

        private static DataContext instance;
        public static DataContext Instance => instance ??= new DataContext();

        public event PropertyChangedEventHandler PropertyChanged;

        public Song CurrentSong => AudioPlayer.Instance.CurrentSong;
        public string CurrentSongArtistName => AudioPlayer.Instance.CurrentSongArtistName;
        public string PathToImage => CurrentSong == null ? "../Assets/null.png" : CurrentSong.PathToImage == null ? "../Assets/NoImage.png" : FileCache.Instance.GetFile(CurrentSong.PathToImage);
        public double Volume => AudioPlayer.Instance.WaveOutDevice.Volume;
        public double MaxVolume => AudioPlayer.Instance.MaxVolume;
        public double TotalTime => AudioPlayer.Instance.CurrentSongFile?.TotalTimeSong ?? 0;
        public double CurrentTime => AudioPlayer.Instance.CurrentSongFile?.CurrentTimeSong ?? 0;
        public string TotalTimeLabel => AudioPlayer.Instance.CurrentSongFile == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSongFile.TotalTimeSong).ToString("m':'ss");
        public string CurrentTimeLabel => AudioPlayer.Instance.CurrentSongFile == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSongFile.CurrentTimeSong).ToString("m':'ss");

        public User CurrentUser => UserController.CurrentUser;
        public string PlayImage => AudioPlayer.Instance.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing ? "/Assets/pause.png" : "/Assets/play.png";
                
        public int CurrentUserCoins => CurrentUser?.Coins ?? 0;

        public Role CurrentUserRole => CurrentUser == null ? null : RoleController.Create(DatabaseContext.Instance).GetItem(CurrentUser.RoleID);
        public bool IsAdmin => CurrentUser?.RoleID.Equals(3) ?? false;
        public bool IsArtist => CurrentUser != null && CurrentUser.RoleID.Equals(2);

        public string SongNameGiving => IsAdmin ? "All Songs" : "Own Songs";

        public string DisplayName => IsArtist ? ArtistController.Create(DatabaseContext.Instance).GetArtistFromUser(CurrentUser).ArtistName : CurrentUser?.Username;

        public bool IsLooping => AudioPlayer.Instance.Looping;
        public bool IsShuffling => AudioPlayer.Instance.Shuffling;

        private Timer timerSlider;
        public Timer Timer { get; set; }

        private DataContext()
        {
            Timer = new Timer {Interval = 1000};
            Timer.Elapsed += CoinsController.Instance.EarnCoins;
            Timer.Elapsed += OnTimedEvent;
            Timer.Start();

            timerSlider = new Timer { Interval = 10 };
            timerSlider.Elapsed += OnTimedEventSlider;
            timerSlider.Start();
        }

        public void OnTimedEvent(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public void OnTimedEventSlider(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentTime"));
        }
    }
}

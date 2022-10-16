using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;

namespace NightClub.ViewModels
{
	public partial class MusicPlayerViewModel : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        IAudioPlayer mediaPlayer;

        #endregion

        public MusicPlayerViewModel()
        {
            MediaPlayer = AudioManager.Current.CreatePlayer(GetStreamFromUrl("https://mp3l.jamendo.com/?trackid=1890762&format=mp31&from=ddFnjPIVLzonc%2FtM%2F8ITPg%3D%3D%7CSZZCqd5WdiWAgMe7FkKzxg%3D%3D"));
        }

        private static Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new System.Net.WebClient())
                imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }

        #region Commands

        [RelayCommand]
        void Play()
        {
            if (MediaPlayer.IsPlaying)
                MediaPlayer.Pause();
            else
                MediaPlayer.Play();

            return;
        }

        #endregion
    }
}
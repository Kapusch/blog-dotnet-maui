using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NightClub.Models;

namespace NightClub.ViewModels;

public partial class MusicPlayerViewModel : ObservableObject
{
    #region Properties

    /// <summary>
    /// A curated playlist of indie music coming from <see href="https://www.jamendo.com/start">Jamendo</see>
    /// </summary>
    static readonly MusicTrack[] playlist = new MusicTrack[]
    {
        new MusicTrack()
        {
            AudioURL = "https://prod-1.storage.jamendo.com/?trackid=1890762&format=mp31&from=b5bSbOTAT1kXawaT8EV9IA%3D%3D%7CGcDX%2BeejT3P%2F0CfPwtSyYA%3D%3D",
            AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/1890762/mp32/",
            Author = "Alfonso Lugo",
            Title = "Baila",
        },
        new MusicTrack()
        {
            AudioURL = "https://prod-1.storage.jamendo.com/?trackid=619144&format=mp31&from=%2BJv5PkdWd%2BvsByBkyrboJA%3D%3D%7Co%2FKvdc5gcd6iQLjnqacjYA%3D%3D",
            AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/619144/mp32/",
            Author = "Pablo Gómez",
            Title = "Devastation (remastered)",
        },
        new MusicTrack()
        {
            AudioURL = "https://prod-1.storage.jamendo.com/?trackid=1399476&format=mp31&from=LQFaB9%2FDVAE6QaK%2BsXtl%2FA%3D%3D%7CouuozaATpW3zoEvVwprgRw%3D%3D",
            AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/1399476/mp32/",
            Author = "Singularity",
            Title = "How many times",
        },
        new MusicTrack()
        {
            AudioURL = "https://prod-1.storage.jamendo.com/?trackid=946449&format=mp31&from=blTB635bS8UiDVL%2FzZC2Xw%3D%3D%7CQO1Fj6AWgTrjIu7LELLCLA%3D%3D",
            AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/946449/mp32/",
            Author = "Julien Gathy",
            Title = "Octave (HQ)",
        },
        new MusicTrack()
        {
            AudioURL = "https://prod-1.storage.jamendo.com/?trackid=1026396&format=mp31&from=nWYOo%2FxFcd1oJBINLSQAXg%3D%3D%7CI8xQbXqZfz2bfgmtqxmqyA%3D%3D",
            AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/1026396/mp32/",
            Author = "dj alike",
            Title = "dj alike (new trance edition)",
        }
    };

    /// <summary>
    /// The position in the playlist of the current track to be played
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentTrack))]
    int currentTrackPosition = 0;

    /// <summary>
    /// The current track to be played
    /// </summary>
    public MusicTrack CurrentTrack => playlist[CurrentTrackPosition];

    /// <summary>
    /// When True, loop the current track only once
    /// </summary>
    [ObservableProperty]
    bool mustRepeatCurrentTrackOnce;

    #endregion

    public MusicPlayerViewModel()
    {
    }

    public void AssessRepeatOrSkip()
    {
        if (MustRepeatCurrentTrackOnce)
        {
            // Either reset the current track by forcing notification to the View
            OnPropertyChanged("CurrentTrack");
            MustRepeatCurrentTrackOnce = false;
        }
        else
        {
            // Or move one step forward in the playlist
            GoToNextTrack();
        }
    }

    #region Commands

    /// <summary>
    /// Toggle option to repeat once the current track after it's over
    /// </summary>
    [RelayCommand]
    void ToggleRepeatOnce()
    {
        MustRepeatCurrentTrackOnce = !MustRepeatCurrentTrackOnce;
    }

    /// <summary>
    /// Move the current track position back
    /// </summary>
    /// <param name="elapsedTimeForCurrentTrack">The current elapsed time of the current track being played</param>
    [RelayCommand]
    void GoToPreviousTrack(double elapsedTimeForCurrentTrack)
    {
        if (elapsedTimeForCurrentTrack < 2)
        {
            // Move one step backward in the playlist
            if (CurrentTrackPosition - 1 >= 0) CurrentTrackPosition--;
            else CurrentTrackPosition = playlist.Length - 1;
        }
        else
        {
            // Or reset the current track by forcing notification to the View
            OnPropertyChanged("CurrentTrack");
        }
    }

    /// <summary>
    /// Move the current track position forward
    /// </summary>
    [RelayCommand]
    void GoToNextTrack()
    {
        // Move one step forward in the playlist
        if (CurrentTrackPosition + 1 < playlist.Length) CurrentTrackPosition++;
        else CurrentTrackPosition = 0;
    }

    [RelayCommand]
    async Task DownloadCurrentTrack(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 100000000; // ~100MB

            using var httpResponse =
                await client.GetAsync(
                    new Uri(CurrentTrack.AudioDownloadURL), cancellationToken);

            httpResponse.EnsureSuccessStatusCode();

            var downloadedImage = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            try
            {
                string fileName = $"{CurrentTrack.Title} - {CurrentTrack.Author}.mp3";

                var fileSaveResult = await FileSaver.SaveAsync(fileName, downloadedImage, cancellationToken);

                fileSaveResult.EnsureSuccess();

                await Toast.Make($"File saved at: {fileSaveResult.FilePath}").Show(cancellationToken);
            }
            catch (Exception ex)
            {
                await Toast.Make($"Cannot save file because: {ex.Message}").Show(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            await Toast.Make($"Cannot download file because: {ex.Message}").Show(cancellationToken);
        }
    }

    #endregion
}
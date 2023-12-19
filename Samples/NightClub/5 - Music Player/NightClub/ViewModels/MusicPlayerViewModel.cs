using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NightClub.Models;

namespace NightClub.ViewModels;

public partial class MusicPlayerViewModel : ObservableObject
{
    #region Properties

    [ObservableProperty]
    MusicTrack currentTrack;

    #endregion

    public MusicPlayerViewModel()
    {
        CurrentTrack = new MusicTrack()
        {
            AudioURL = "https://prod-1.storage.jamendo.com/?trackid=1890762&format=mp31&from=b5bSbOTAT1kXawaT8EV9IA%3D%3D%7CGcDX%2BeejT3P%2F0CfPwtSyYA%3D%3D",
            AudioDownloadURL = "https://prod-1.storage.jamendo.com/download/track/1890762/mp32/",
            Author = "Alfonso Lugo",
            Title = "Baila",
        };
    }

    #region Commands

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
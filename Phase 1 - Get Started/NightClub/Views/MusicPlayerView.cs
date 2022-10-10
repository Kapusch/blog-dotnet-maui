using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace NightClub.Views;
public class MusicPlayerView : ContentPage
{
	public MusicPlayerView()
    {
        Console.WriteLine("[NightClub] MusicPlayerView - Constructor");

        NavigationPage.SetHasNavigationBar(this, false);
        BackgroundColor = Colors.DimGray;

        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                Stars(60),
                Stars(40)),
            RowSpacing = 0,
            Children =
            {
                TopLayout.Row(0),
                BottomLayout.Row(1),
            }
        };
    }

    #region Controls

    #region Main Layouts

    Grid TopLayout => new Grid
    {
        BackgroundColor = Colors.Black
    };

    Grid BottomLayout => new Grid
    {
        BackgroundColor = Colors.DimGray,
        RowDefinitions = Rows.Define(
                Stars(1),
                Stars(1),
                Stars(1)),
        RowSpacing = 0,
        ColumnDefinitions = Columns.Define(
                Stars(10),
                Stars(10),
                Stars(20),
                Stars(20),
                Stars(20),
                Stars(10),
                Stars(10)),
        ColumnSpacing = 0,
        Children =
        {
            ElapsedTime.Row(0).Column(1),
            TimeTracker.Row(0).Column(2).ColumnSpan(3),
            RemainingTime.Row(0).Column(5),

            RepeatOnceButton.Row(1).Column(1),
            MediaControlLayout.Row(1).Column(2).ColumnSpan(3),
            DownloadButton.Row(1).Column(5),

            MuteButton.Row(2).Column(1),
            VolumeTracker.Row(2).Column(2).ColumnSpan(3),
        }
    };

    Grid MediaControlLayout => new Grid
    {
        ColumnDefinitions = Columns.Define(
                Stars(30),
                Stars(40),
                Stars(30)),
        ColumnSpacing = 0,
        Children =
        {
            SkipPreviousButton.Column(0),
            PlayButton.Column(1),
            SkipNextButton.Column(2)
        }
    };

    #endregion

    #region Time Tracker

    Label ElapsedTime => new Label
    {
        FontSize = 14,
        Text = "0:36",
        TextColor = Colors.White
    }.TextCenter();

    Slider TimeTracker => new Slider
    {
        Minimum = 0,
        MinimumTrackColor = Colors.LightSalmon,
        Maximum = 100,
        MaximumTrackColor = Colors.Black,
        Value = 20
    };

    Label RemainingTime => new Label
    {
        FontSize = 14,
        Text = "2:57",
        TextColor = Colors.White
    }.TextCenter();

    #endregion

    #region Media Control Panel

    ImageButton RepeatOnceButton => new ImageButton
    {
        CornerRadius = 5,
        HeightRequest = 25,
        WidthRequest = 25,
        Source = "repeat_once",
        BackgroundColor = Colors.Black
    };

    ImageButton SkipPreviousButton => new ImageButton
    {
        HeightRequest = 75,
        WidthRequest = 75,
        Source = "skip_previous"
    };

    ImageButton PlayButton => new ImageButton
    {
        CornerRadius = 50,
        HeightRequest = 100,
        WidthRequest = 100,
        Source = "play",
        BackgroundColor = Colors.Black
    };

    ImageButton SkipNextButton => new ImageButton
    {
        HeightRequest = 75,
        WidthRequest = 75,
        Source = "skip_next"
    };

    ImageButton DownloadButton => new ImageButton
    {
        CornerRadius = 5,
        HeightRequest = 25,
        WidthRequest = 25,
        Source = "download",
        BackgroundColor = Colors.Black
    };

    #endregion

    #region Volume Tracker

    ImageButton MuteButton => new ImageButton
    {
        HeightRequest = 25,
        WidthRequest = 25,
        Source = "volume_medium"
    };

    Slider VolumeTracker => new Slider
    {
        Minimum = 0,
        MinimumTrackColor = Colors.Black,
        Maximum = 100,
        MaximumTrackColor = Colors.Gray,
        Value = 60
    };

    #endregion

    #endregion

    #region Events

    /// <summary>
    /// Event raised when MusicPlayerView appears on screen
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        Console.WriteLine("[NightClub] MusicPlayerView - OnAppearing");
    }

    /// <summary>
    /// Event raised when MusicPlayerView disappears from screen
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Console.WriteLine("[NightClub] MusicPlayerView - OnDisappearing");
    }

    #endregion
}

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
            RemainingTime.Row(0).Column(5)
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

    /*
            <Slider
                x:Name="timeTracker"
                Grid.Row="0"
                Grid.Column="1"
                DragCompleted="TimeTracker_DragCompleted"
                DragStarted="TimeTracker_DragStarted"
                Maximum="{Binding Duration, Converter={StaticResource TimeSpanConverter}, Source={x:Reference mediaPlayer}}"
                MaximumTrackColor="{StaticResource Black}"
                MinimumTrackColor="{StaticResource LightSalmonPink}" />*/

    Label RemainingTime => new Label
    {
        FontSize = 14,
        Text = "2:57",
        TextColor = Colors.White
    }.TextCenter();

    #endregion

    #region Media Control Panel



    #endregion

    #region Volume Tracker



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

using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace NightClub.Views;
public class MusicPlayerView : ContentPage
{
	public MusicPlayerView()
    {
        Console.WriteLine("[NightClub] MusicPlayerView - Constructor");

        NavigationPage.SetHasNavigationBar(this, false);

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

    Grid TopLayout => new Grid
    {
        BackgroundColor = Colors.Black
    };

    Grid BottomLayout => new Grid
    {
        BackgroundColor = Colors.DimGray
    };

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

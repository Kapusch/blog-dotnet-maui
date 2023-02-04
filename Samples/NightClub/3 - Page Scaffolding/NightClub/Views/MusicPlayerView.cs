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
            new BoxView { Color = Color.FromArgb("#ffffff") }.Row(0).Column(0),
            new BoxView { Color = Color.FromArgb("#d0d0d0") }.Row(0).Column(1),
            new BoxView { Color = Color.FromArgb("#a2a3a3") }.Row(0).Column(2),
            new BoxView { Color = Color.FromArgb("#777879") }.Row(0).Column(3),
            new BoxView { Color = Color.FromArgb("#4e5051") }.Row(0).Column(4),
            new BoxView { Color = Color.FromArgb("#292b2c") }.Row(0).Column(5),
            new BoxView { Color = Color.FromArgb("#000405") }.Row(0).Column(6),
            new BoxView { Color = Color.FromArgb("#f3f337") }.Row(1).Column(0),
            new BoxView { Color = Color.FromArgb("#a2eb5b") }.Row(1).Column(1),
            new BoxView { Color = Color.FromArgb("#4edb80") }.Row(1).Column(2),
            new BoxView { Color = Color.FromArgb("#00c89f") }.Row(1).Column(3),
            new BoxView { Color = Color.FromArgb("#00b1b1") }.Row(1).Column(4),
            new BoxView { Color = Color.FromArgb("#0098b2") }.Row(1).Column(5),
            new BoxView { Color = Color.FromArgb("#177ea2") }.Row(1).Column(6),
            new BoxView { Color = Color.FromArgb("#bf7aef") }.Row(2).Column(0),
            new BoxView { Color = Color.FromArgb("#ea6cd4") }.Row(2).Column(1),
            new BoxView { Color = Color.FromArgb("#ff63b3") }.Row(2).Column(2),
            new BoxView { Color = Color.FromArgb("#ff6590") }.Row(2).Column(3),
            new BoxView { Color = Color.FromArgb("#ff716e") }.Row(2).Column(4),
            new BoxView { Color = Color.FromArgb("#ff844e") }.Row(2).Column(5),
            new BoxView { Color = Color.FromArgb("#f89832") }.Row(2).Column(6),
        }
    };

    #endregion

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
}

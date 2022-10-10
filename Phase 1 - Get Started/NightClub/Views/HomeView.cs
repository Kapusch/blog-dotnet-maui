using CommunityToolkit.Maui.Markup;
using NightClub.ViewModels;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;

namespace NightClub.Views;
public class HomeView : ContentPage
{
    public HomeView()
    {
        Console.WriteLine("[NightClub] HomePage - Constructor");

        BindingContext = new HomeViewModel();

        NavigationPage.SetHasNavigationBar(this, false);
        BackgroundColor = Colors.Black;

        Content = new VerticalStackLayout
        {
            Children =
            {
                NightClubImage,
                EnterButton
            }
        }.CenterVertical();
    }

    #region Controls

    Image NightClubImage => new Image
    {
        Source = "night_club"
    };

    Button EnterButton => new Button
    {
        Text = "ENTER",
        TextColor = Colors.White,
        BackgroundColor = Colors.Magenta,
        CornerRadius = 10
    } .Bold() .Paddings(50, 2, 50, 2) .CenterHorizontal()
        .BindCommand("EnterCommand");

    #endregion

    #region Events

    /// <summary>
    /// Event raised when HomePage appears on screen
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        Console.WriteLine("[NightClub] HomePage - OnAppearing");
    }

    /// <summary>
    /// Event raised when HomePage disappears from screen
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Console.WriteLine("[NightClub] HomePage - OnDisappearing");
    }

    #endregion
}
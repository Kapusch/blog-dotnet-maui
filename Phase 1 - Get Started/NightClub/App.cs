using NightClub.Views;

namespace NightClub;

public partial class App : Application
{
    public App()
    {
        MainPage = new NavigationPage(new HomePage());
    }
}
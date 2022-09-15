using NightClub.Views;

namespace NightClub;

public partial class App : Application
{
    public App()
    {
        MainPage = new NavigationPage(new HomePage());
    }

    protected override void OnStart()
    {
        base.OnStart();

        Console.WriteLine("[NightClub] START");
    }

    protected override void OnSleep()
    {
        base.OnSleep();

        Console.WriteLine("[NightClub] SLEEP");
    }

    protected override void OnResume()
    {
        base.OnResume();

        Console.WriteLine("[NightClub] RESUME");
    }
}
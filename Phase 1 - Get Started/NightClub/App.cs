using NightClub.Views;

namespace NightClub;

public class App : Application
{
    public App()
    {
        Console.WriteLine("[NightClub] App - Constructor");

        MainPage = new HomePage();
    }

    protected override void OnStart()
    {
        base.OnStart();

        Console.WriteLine("[NightClub] App - OnStart");
    }

    protected override void OnSleep()
    {
        base.OnSleep();

        Console.WriteLine("[NightClub] App - OnSleep");
    }

    protected override void OnResume()
    {
        base.OnResume();

        Console.WriteLine("[NightClub] App - OnResume");
    }
}
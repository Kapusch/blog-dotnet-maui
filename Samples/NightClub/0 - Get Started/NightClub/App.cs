using NightClub.Views;

namespace NightClub;

public class App : Application
{
    public App()
    {
        Console.WriteLine("[NightClub] App - Constructor");

        MainPage = new ContentPage();
    }

    /// <summary>
    /// Event raised when running a fresh app start (e.g. after phone is turned on)
    /// </summary>
    protected override void OnStart()
    {
        base.OnStart();

        Console.WriteLine("[NightClub] App - OnStart");

        MainPage = new HomeView();
    }

    /// <summary>
    /// Event raised when the app is not focus anymore (e.g. when switching to another app)
    /// </summary>
    protected override void OnSleep()
    {
        base.OnSleep();

        Console.WriteLine("[NightClub] App - OnSleep");
    }

    /// <summary>
    /// Event raised when the app goes to the foreground (e.g. when returning to the app)
    /// </summary>
    protected override void OnResume()
    {
        base.OnResume();

        Console.WriteLine("[NightClub] App - OnResume");
    }
}
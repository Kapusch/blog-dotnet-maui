using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui;

namespace NightClub;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
            .UseMauiApp<App>();

        return builder.Build();
    }
}
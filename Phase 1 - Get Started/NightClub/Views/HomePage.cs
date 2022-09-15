using System;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;

namespace NightClub.Views
{
    public class HomePage : ContentPage
    {
        public HomePage()
        {
            #region Page configuration

            On<iOS>().SafeAreaInsets();
            NavigationPage.SetHasNavigationBar(this, false);

            #endregion

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
            // .NET MAUI converts SVG files to PNG files.
            Source = "night_club.png"
        };

        Button EnterButton => new Button
        {
            Text = "ENTER",
            TextColor = Colors.White,
            BackgroundColor = Colors.Magenta,
            CornerRadius = 10
        } .Bold() .Paddings(50, 2, 50, 2) .CenterHorizontal();

        #endregion
    }
}
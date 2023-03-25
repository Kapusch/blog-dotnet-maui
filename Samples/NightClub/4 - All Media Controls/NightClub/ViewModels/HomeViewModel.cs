using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NightClub.Views;

namespace NightClub.ViewModels;
public partial class HomeViewModel : ObservableObject
{
    public HomeViewModel()
    {
    }

    [RelayCommand]
    async Task Enter()
    {
        await Application.Current.MainPage.Navigation.PushAsync(
            new MusicPlayerView());
    }
}


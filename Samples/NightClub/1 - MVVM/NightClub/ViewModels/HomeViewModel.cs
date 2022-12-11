using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NightClub.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public HomeViewModel()
    {
    }

    [RelayCommand]
    async Task Enter()
    {
        await Application.Current.MainPage.DisplayAlert(
            "Well Done !",
            "You have successfully reached the end of this chapter.",
            "Next !");
    }
}


using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NightClub.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        public HomeViewModel()
        {
        }

        [RelayCommand]
        void EnterCommand()
        {
            Console.WriteLine("[NightClub] HomePage - EnterCommand has been executed from the UI");
        }
    }
}


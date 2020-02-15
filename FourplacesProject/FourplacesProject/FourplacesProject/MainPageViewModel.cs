using Storm.Mvvm;
using Storm.Mvvm.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FourplacesProject
{
    class MainPageViewModel : ViewModelBase
    {
        public ICommand goToCreateUser { get; }
        public ICommand goToLogin { get; }

        public MainPageViewModel()
        {
            goToCreateUser = new Command(Button_Clicked_1);
            goToLogin = new Command(Button_Clicked);
        }

        private async void Button_Clicked_1()
        {
            await DependencyService.Get<INavigationService>().PushAsync(new CreateUser());
        }


        private void Button_Clicked()
        {

        }
    }
}

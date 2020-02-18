using Common.Api.Dtos;
using MonkeyCache.SQLite;
using Storm.Mvvm;
using Storm.Mvvm.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace FourplacesProject
{
    class MainPageViewModel : ViewModelBase
    {
        public ICommand goToCreateUser { get; }
        public ICommand goToLogin { get; }

        private string _identifiant;
        public string Identifiant
        {
            get => _identifiant;
            set => SetProperty(ref _identifiant, value);
        }

        private string _motDePasse;
        public string MotDePasse
        {
            get => _motDePasse;
            set => SetProperty(ref _motDePasse, value);
        }

        private string URI = "https://td-api.julienmialon.com/";

        public MainPageViewModel()
        {
            goToCreateUser = new Command(Button_Clicked_1);
            goToLogin = new Command(Button_Clicked);
        }

        private async void Button_Clicked_1()
        {
            await DependencyService.Get<INavigationService>().PushAsync(new CreateUser());
        }


        private async void Button_Clicked()
        {
            INavigationService nav = DependencyService.Get<INavigationService>();
            ApiClient api = new ApiClient();
            HttpResponseMessage response = await api.Execute(HttpMethod.Post, URI + "auth/login", new LoginRequest() { Email = _identifiant, Password = _motDePasse });
            Response<LoginResult> res = await api.ReadFromResponse<Response<LoginResult>>(response);
            if (res.IsSuccess)
            {
                var currentBarrel = Barrel.Current;
                Response<LoginResult> deserialized = await api.ReadFromResponse<Response<LoginResult>>(response);
                currentBarrel.Add("token", deserialized.Data.AccessToken, TimeSpan.FromSeconds(deserialized.Data.ExpiresIn));
                await nav.PushAsync<HomePage>();
            }
        }
    }
}

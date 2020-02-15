using Common.Api.Dtos;
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
    class CreateUserViewModel : ViewModelBase
    {
        private string URI = "https://td-api.julienmialon.com/";
        public ICommand goToMainPage { get; }

        private string _mail;
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }

        private string _motDePasse;
        public string MotDePasse
        {
            get => _motDePasse;
            set => SetProperty(ref _motDePasse, value);
        }

        private string _nom;
        public string Nom
        {
            get => _nom;
            set => SetProperty(ref _nom, value);
        }

        private string _prenom;
        public string Prenom
        {
            get => _prenom;
            set => SetProperty(ref _prenom, value);
        }

        public CreateUserViewModel()
        {
            goToMainPage = new Command(Button_Clicked);
        }

        private async void Button_Clicked()
        {
            if (_mail != "" || _motDePasse != "" || _nom != "" || _prenom != "")
            {
                INavigationService nav = DependencyService.Get<INavigationService>();
                ApiClient api = new ApiClient();
                HttpResponseMessage response = await api.Execute(HttpMethod.Post, URI + "auth/register", new RegisterRequest() { Email = _mail, FirstName = _prenom, LastName = _prenom, Password = _motDePasse });
                Response<LoginResult> res = await api.ReadFromResponse<Response<LoginResult>>(response);
                if (res.IsSuccess)
                {
                    await nav.PopAsync(true);
                }
            }
        }
    }
}

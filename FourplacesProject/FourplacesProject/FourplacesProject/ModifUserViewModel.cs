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
    class ModifUserViewModel : ViewModelBase
    {
        private string URI = "https://td-api.julienmialon.com/";
        public ICommand goToHome { get; }

        private string _motDePasseInit;
        private string _motDePasse;
        public string MotDePasse
        {
            get => _motDePasse;
            set => SetProperty(ref _motDePasse, value);
        }

        private string _nomInit;
        private string _nom;
        public string Nom
        {
            get => _nom;
            set => SetProperty(ref _nom, value);
        }

        private string _prenomInit;
        private string _prenom;
        public string Prenom
        {
            get => _prenom;
            set => SetProperty(ref _prenom, value);
        }

        public ModifUserViewModel()
        {
            goToHome = new Command(Button_Clicked);
            getUser();
        }

        public async void getUser()
        {
            INavigationService nav = DependencyService.Get<INavigationService>();
            ApiClient api = new ApiClient();
            HttpResponseMessage response = await api.Execute(HttpMethod.Get, URI + "me", new UserItem() { });
            Response<UserItem> res = await api.ReadFromResponse<Response<UserItem>>(response);
            Console.WriteLine(res.Data);
        }

        private async void Button_Clicked()
        {

        }
    }
}

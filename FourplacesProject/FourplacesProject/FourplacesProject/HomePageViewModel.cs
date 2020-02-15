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
    class HomePageViewModel : ViewModelBase
    {
        private string URI = "https://td-api.julienmialon.com/";
        public ICommand goToModifPage { get; }

        private string _listPLace;
        public string ListPlace
        {
            get => _listPLace;
            set => SetProperty(ref _listPLace, value);
        }

        public HomePageViewModel()
        {
            goToModifPage = new Command(Button_Clicked);
            getPlaces();
        }

        private async void getPlaces()
        {
            INavigationService nav = DependencyService.Get<INavigationService>();
            ApiClient api = new ApiClient();
            HttpResponseMessage response = await api.Execute(HttpMethod.Get, URI + "places", new PlaceItemSummary() {});
            Response<PlaceItemSummary> res = await api.ReadFromResponse<Response<PlaceItemSummary>>(response);
            if (res.IsSuccess)
            {
                Console.WriteLine(res.Data);
            }
        }

        private async void Button_Clicked()
        {
            await DependencyService.Get<INavigationService>().PushAsync(new ModifUser());
        }
    }
}

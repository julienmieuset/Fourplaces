using Common.Api.Dtos;
using MonkeyCache.SQLite;
using Storm.Mvvm;
using Storm.Mvvm.Services;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using TD.Api.Dtos;
using Xamarin.Forms;
using MonkeyCache.SQLite;

namespace FourplacesProject
{
    class HomePageViewModel : ViewModelBase
    {
        private readonly string URI = "https://td-api.julienmialon.com/";
        private Lazy<INavigationService> _navigationService;
        public ICommand goToModifPage { get; }
        public ICommand goToAddPlacePage { get; }

        private PlaceItemSummary _rowSelected;
        public PlaceItemSummary RowSelected
        {
            get => _rowSelected;
            set
            {
                if (SetProperty(ref _rowSelected, value) && value != null)
                {
                    RowIsSelected(value);
                    RowSelected = null;
                }
            }
        }

        private ObservableCollection<PlaceItemSummary> _listPLace;
        public ObservableCollection<PlaceItemSummary> ListPlace
        {
            get => _listPLace;
            set => SetProperty(ref _listPLace, value);
        }

        public HomePageViewModel()
        {
            goToModifPage = new Command(Button_Clicked);
            goToAddPlacePage = new Command(Button_Clicked_1);
            _navigationService = new Lazy<INavigationService>(() => DependencyService.Resolve<INavigationService>());
            getPlaces();
        }

        private async void getPlaces()
        {
            try
            {
                INavigationService nav = DependencyService.Get<INavigationService>();
                ApiClient api = new ApiClient();
                HttpResponseMessage response = await api.Execute(HttpMethod.Get, URI + "places");
                Response<ObservableCollection<PlaceItemSummary>> res = await api.ReadFromResponse<Response<ObservableCollection<PlaceItemSummary>>>(response);
                if (res.IsSuccess)
                {
                    ListPlace = res.Data;
                    Console.WriteLine(_listPLace);
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", e.Message, "ok");
            }
        }

        private async void RowIsSelected(PlaceItemSummary place)
        {
            await _navigationService.Value.PushAsync<Description>(new System.Collections.Generic.Dictionary<string, object>
            {
                {"ItemPlace", place }
            });
        }

        private async void Button_Clicked()
        {
            await DependencyService.Get<INavigationService>().PushAsync(new ModifUser());
        }

        private async void Button_Clicked_1()
        {
            await DependencyService.Get<INavigationService>().PushAsync(new AddPlacePage());
        }
    }
}

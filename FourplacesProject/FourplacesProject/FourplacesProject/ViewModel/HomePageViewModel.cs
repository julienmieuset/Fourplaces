﻿using Common.Api.Dtos;
using MonkeyCache.SQLite;
using Storm.Mvvm;
using Storm.Mvvm.Services;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace FourplacesProject
{
    class HomePageViewModel : ViewModelBase
    {
        private readonly string URI = "https://td-api.julienmialon.com/";
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
            goToAddPlacePage = new Command(Button_Clicked);
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
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", e.Message, "ok");
            }
        }

        private async void RowIsSelected(PlaceItemSummary place)
        {
            var currentBarrel = Barrel.Current;
            currentBarrel.Add("CurrentPlace", place, TimeSpan.FromDays(1));
            await DependencyService.Get<INavigationService>().PushAsync(new Description());
        }

        private async void Button_Clicked()
        {
            await DependencyService.Get<INavigationService>().PushAsync(new AddPlacePage());
        }
    }
}

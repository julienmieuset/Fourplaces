using Common.Api.Dtos;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using TD.Api.Dtos;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FourplacesProject
{
    class DescriptionViewModel : ViewModelBase
    {
        private readonly string URI = "https://td-api.julienmialon.com/";
        [NavigationParameter("ItemPlace")]
        public PlaceItemSummary ItemPlace { get; set; }
        public ICommand goToGoogleMap { get; set; }

        private string _titre;
        public string Titre
        {
            get => _titre;
            set => SetProperty(ref _titre, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, Convert.ToDouble(value));
        }

        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, Convert.ToDouble(value));
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        private List<CommentItem> _commentItem;
        public List<CommentItem> CommentItem
        {
            get => _commentItem;
            set => SetProperty(ref _commentItem, value);
        }

        public DescriptionViewModel()
        {
            goToGoogleMap = new Command(Button_Clicked);
        }

        public override async void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);

            if (ItemPlace != null)
            {
                Titre = ItemPlace.Title;
                Longitude = ItemPlace.Longitude;
                Latitude = ItemPlace.Latitude;
                Description = ItemPlace.Description;
                ImageSource = URI + "images/" + ItemPlace.ImageId;

                INavigationService nav = DependencyService.Get<INavigationService>();
                ApiClient api = new ApiClient();
                HttpResponseMessage response = await api.Execute(HttpMethod.Get, URI + "places/" + ItemPlace.Id);
                Response<PlaceItem> res = await api.ReadFromResponse<Response<PlaceItem>>(response);
                if (res.IsSuccess)
                {
                    CommentItem = res.Data.Comments;
                }
            }
        }

        private async void Button_Clicked()
        {
            Console.WriteLine("here");
            var location = new Xamarin.Essentials.Location(Latitude, Longitude);
            var options = new MapLaunchOptions { Name = Titre };
            await Map.OpenAsync(location, options);
        }
    }
}

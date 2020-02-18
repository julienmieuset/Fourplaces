using Storm.Mvvm;
using System;
using Storm.Mvvm.Services;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Plugin.Media.Abstractions;
using TD.Api.Dtos;
using Common.Api.Dtos;
using Newtonsoft.Json;
using MonkeyCache.SQLite;

namespace FourplacesProject.ViewModel
{
    class AddPlacePageViewModel : ViewModelBase
    {
        private readonly string URI = "https://td-api.julienmialon.com/";
        public ICommand goToSavePlace { get; }
        public ICommand goToImage { get; }

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

        private ImageSource _sourceImage;
        public ImageSource SourceImage
        {
            get => _sourceImage;
            set => SetProperty(ref _sourceImage, value);
        }

        public AddPlacePageViewModel()
        {
            goToSavePlace = new Command(SavePlace);
            goToImage = new Command(SourceFromGallery);
        }

        private async void SavePlace()
        {
            if (Titre != "" && Longitude != 0 && Latitude != 0 && Description != "")
            {
                HttpClient client = new HttpClient();
                var currentBarrel = Barrel.Current;
                StreamImageSource TmpStream = (StreamImageSource)SourceImage;
                byte[] imageData;
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    var stream = TmpStream.Stream.Invoke(new System.Threading.CancellationToken()).Result;
                    stream.CopyTo(memoryStream);
                    stream = null;
                    imageData = memoryStream.ToArray();
                }
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URI + "images"); 
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", currentBarrel.Get<string>(key: "token"));
                MultipartFormDataContent requestContent = new MultipartFormDataContent();

                var imageContent = new ByteArrayContent(imageData);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                // Le deuxième paramètre doit absolument être "file" ici sinon ça ne fonctionnera pas
                requestContent.Add(imageContent, "file", "file.jpg");
                request.Content = requestContent;
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ApiClient api = new ApiClient();
                    Response<ImageItem> deserialized = await api.ReadFromResponse<Response<ImageItem>>(response);

                    INavigationService nav = DependencyService.Get<INavigationService>();
                    HttpResponseMessage responseCreation = await api.Execute(HttpMethod.Post, URI + "places", new CreatePlaceRequest() { Title = Titre, Description = Description, ImageId = deserialized.Data.Id, Latitude = Latitude, Longitude = Longitude }, currentBarrel.Get<string>(key: "token"));
                    Response<CreatePlaceRequest> res = await api.ReadFromResponse<Response<CreatePlaceRequest>>(responseCreation);

                    if (res.IsSuccess)
                    {
                        await nav.PopAsync(true);
                    }
                }
            }
        }

        private async void SourceFromGallery()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await App.Current.MainPage.DisplayAlert("Need location", "Gunna need that location", "OK");
                    }
                }

                if (status == PermissionStatus.Granted)
                {
                    await CrossMedia.Current.Initialize();

                    var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small
                    });
                    SourceImage = ImageSource.FromStream(() => selectedImageFile.GetStream());
                }
                else if (status != PermissionStatus.Unknown)
                {
                    //location denied
                    Console.WriteLine("Access denied");
                }
            }
            catch (Exception)
            {
                //Something went wrong
            }
        }
    }
}

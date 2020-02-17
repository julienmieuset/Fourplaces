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
            //if (Titre != "" && Longitude != 0 && Latitude != 0 && Description != "")
            //{
            Console.WriteLine(SourceImage);
                /*HttpClient client = new HttpClient();
                byte[] imageData = SourceImage;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URI + "images");
                MultipartFormDataContent requestContent = new MultipartFormDataContent();

                var imageContent = new ByteArrayContent(imageData);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                // Le deuxième paramètre doit absolument être "file" ici sinon ça ne fonctionnera pas
                requestContent.Add(imageContent, "file", "file.jpg");

                request.Content = requestContent;

                HttpResponseMessage response = await client.SendAsync(request);

                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Image uploaded!");
                }*/
            //}
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

                    var selectedImageFile = await CrossMedia.Current.PickPhotoAsync();
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

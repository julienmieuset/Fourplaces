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

        private void SavePlace()
        {
            Console.WriteLine(">>>");
            Console.WriteLine(Titre);
            Console.WriteLine(Description);
            Console.WriteLine(Latitude);
            Console.WriteLine(Longitude);
            Console.WriteLine("<<<");
        }

        private async void SourceFromGallery()
        {
            /*var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
            var response = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Calendar);
            Console.WriteLine(permissionStatus);*/

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await App.Current.MainPage.DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    //status = await CrossPermissions.Current.RequestPermissionAsync(CalendarPermission);
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
            catch (Exception ex)
            {
                //Something went wrong
            }

            /*var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
            var response = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Calendar);
            Console.WriteLine(permissionStatus);*/
        }

        /*
        Console.WriteLine("TEST");
        await CrossMedia.Current.Initialize();

        var selectedImageFile = await CrossMedia.Current.PickPhotoAsync();
        SourceImage = ImageSource.FromStream(() => selectedImageFile.GetStream());*/
    }
}

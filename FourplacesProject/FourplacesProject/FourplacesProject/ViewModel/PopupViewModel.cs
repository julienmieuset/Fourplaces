using Common.Api.Dtos;
using MonkeyCache.SQLite;
using Rg.Plugins.Popup.Services;
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
    class PopupViewModel : ViewModelBase
    {
        private readonly string URI = "https://td-api.julienmialon.com/";
        public ICommand goToCancel { get; }
        public ICommand goToSave { get; }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public PopupViewModel()
        {
            goToCancel = new Command(Button_Clicked_Cancel);
            goToSave = new Command(Button_Clicked_Save);
        }

        private async void Button_Clicked_Cancel()
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void Button_Clicked_Save()
        {
            Console.WriteLine(">>>"+Comment+"<<<");

            ApiClient api = new ApiClient();
            var currentBarrel = Barrel.Current;

            HttpResponseMessage responseCreation = await api.Execute(HttpMethod.Post, URI + "places/" + currentBarrel.Get<PlaceItemSummary>(key: "CurrentPlace").Id + "/comments", new CreateCommentRequest() { Text = Comment}, currentBarrel.Get<string>(key: "token"));
            Response<CreateCommentRequest> res = await api.ReadFromResponse<Response<CreateCommentRequest>>(responseCreation);

            if (res.IsSuccess)
            {
                await PopupNavigation.Instance.PopAsync();
            }

        }
    }
}

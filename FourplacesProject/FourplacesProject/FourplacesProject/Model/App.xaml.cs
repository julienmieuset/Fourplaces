using Storm.Mvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using MonkeyCache.SQLite;

namespace FourplacesProject
{
    public partial class App : MvvmApplication
    {
        public App() : base(() => new MainPage())
        {
            Barrel.ApplicationId = "MonkeyApp";
            InitializeComponent();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
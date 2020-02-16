using Storm.Mvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace FourplacesProject
{
    public partial class App : MvvmApplication
    {
        public App() : base(() => new MainPage())
        {
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
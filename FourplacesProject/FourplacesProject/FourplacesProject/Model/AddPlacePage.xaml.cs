using FourplacesProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourplacesProject
{
    public partial class AddPlacePage : ContentPage
    {
        public AddPlacePage()
        {
            BindingContext = new AddPlacePageViewModel();
            InitializeComponent();
        }
    }
}
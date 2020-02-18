using Rg.Plugins.Popup.Pages;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourplacesProject
{
    public partial class PopupView : PopupPage
    {
        public PopupView()
        {
            BindingContext = new PopupViewModel();
            InitializeComponent();
        }
    }
}
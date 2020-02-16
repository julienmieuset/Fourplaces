using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourplacesProject
{
    public partial class ModifUser : ContentPage
    {
        public ModifUser()
        {
            BindingContext = new ModifUserViewModel();
            InitializeComponent();
        }
    }
}
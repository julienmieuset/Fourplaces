using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FourplacesProject
{
    public partial class Description : ContentPage
    {
        public Description()
        {
            BindingContext = new DescriptionViewModel();
            InitializeComponent();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace FourplacesProject
{
    class HomePageViewModel
    {
        private string _mail;
        public string Mail
        {
            get => _mail;
            set => SetProperty(ref _mail, value);
        }
    }
}

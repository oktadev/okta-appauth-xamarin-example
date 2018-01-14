using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OktaDemo.XF.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthInfoPage : ContentPage
    {
        public AuthInfoPage(string name, string email, string preferredUsername)
        {
            InitializeComponent();
            _nameLabel.Text = name;
            _emailLabel.Text = email;
            _preferredUsernameLabel.Text = preferredUsername;
        }
    }
}
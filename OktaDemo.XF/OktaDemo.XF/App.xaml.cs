using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OktaDemo.XF.Interfaces;
using OktaDemo.XF.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OktaDemo.XF
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
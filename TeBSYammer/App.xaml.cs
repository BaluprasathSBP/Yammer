﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeBSYammer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppSettings appsettings = new AppSettings();
            MainPage = new NavigationPage(new MainPage());
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
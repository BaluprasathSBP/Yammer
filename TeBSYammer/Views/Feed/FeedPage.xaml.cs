using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TeBSYammer.Views.Feed
{
    public partial class FeedPage : ContentPage
    {
        public FeedPage()
        {
            InitializeComponent();
            this.BindingContext = new FeedPageModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}

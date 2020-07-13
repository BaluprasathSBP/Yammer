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
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = new FeedPageModel();
            
        }
    }
}

using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TeBSYammer.Views.Feed
{
    public partial class FeedCell : ViewCell
    {
        public FeedCell()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}

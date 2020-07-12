using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeBSYammer.DependencyServices;
using TeBSYammer.Views.Feed;
using Xamarin.Forms;

namespace TeBSYammer
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(Application.Current.Properties.ContainsKey("token"))
            {
                AppSettings.Token = Application.Current.Properties["token"].ToString();
                Navigation.PushAsync(new NavigationPage(new FeedPage()));
            }            
        }

        public Command LoginCommand
        {
            get => new Command(() =>
            {                 
               
                DependencyService.Get<IYammerAuth>().Authenticate((token, secret) => LoginWithAuthProvider("Yammer", token, null), HandleException);
            });
        }

        void LoginWithAuthProvider(string provider, string token, string tokenSecret, string name = null)
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {
                Navigation.PushAsync(new NavigationPage(new FeedPage()));
            }
        }

        protected virtual void HandleException(Exception e)
        {            
            Device.BeginInvokeOnMainThread(() => IsBusy = false);
        }
    }
}

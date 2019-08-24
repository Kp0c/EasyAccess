using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EAMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //if (Application.Current.Properties["UserObject"] != null)
            //{
               // MainPage = new NavigationPage(new AuthenticatedPage());
            //}
            //else
            //{
                MainPage = new NavigationPage(new MainPage());
           // }
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

using EAMobile.Extensions;
using EAMobile.PlatrformDependentInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EAMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void SignIn_Clicked(object sender, EventArgs e)
        {
            popupLoginView.IsVisible = true;
        }

        private void SignUp_Clicked(object sender, EventArgs e)
        {
            popupRegisterView.IsVisible = true;
        }

        private void Login_Clicked(object sender, EventArgs e)
        {
            string username = loginUsername.Text;

            if (String.IsNullOrEmpty(username))
            {
                DependencyService.Get<IMessage>().LongAlert("Invalid username!");
                return;
            }

            var token = EasyAccessApi.Login(username);
            if (!String.IsNullOrEmpty(token))
            {
                popupLoginView.IsVisible = false;
                Application.Current.Properties["AuthToken"] = token;
                Application.Current.Properties["Username"] = username;

                AuthenticatedPage authenticatedPage = new AuthenticatedPage();

                Navigation.PushModalAsync(authenticatedPage, true);
            }
            else
            {
                DependencyService.Get<IMessage>().LongAlert("Access is not authorized.");
            }
        }

        private void Registration_Clicked(object sender, EventArgs e)
        {
            string username = registrationUsername.Text;
            string email = registrationEmail.Text;

            if(!email.IsValidEmail())
            {
                DependencyService.Get<IMessage>().LongAlert("Invalid email!");
                return;
            }

            if(String.IsNullOrEmpty(username))
            {
                DependencyService.Get<IMessage>().LongAlert("Invalid username!");
                return;
            }

            EasyAccessApi.Register(email, username);
            popupRegisterView.IsVisible = false;

            DependencyService.Get<IMessage>().LongAlert("Click on the link in the email.");
        }
    }
}

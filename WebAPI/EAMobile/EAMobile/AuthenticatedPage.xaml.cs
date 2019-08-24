using EAMobile.Models;
using EAMobile.PlatrformDependentInterfaces;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

namespace EAMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AuthenticatedPage : ContentPage
	{
		public AuthenticatedPage ()
		{
			InitializeComponent ();

            RefreshState();
        }

        void RefreshState()
        {
          /*  var token = Application.Current.Properties["AuthToken"].ToString();
            var username = Application.Current.Properties["Username"].ToString();

            var user = EasyAccessApi.GetUser(username, token);

            Application.Current.Properties["UserObject"] = user;

            var deviceId = DependencyService.Get<IFirebaseToken>().DeviceId;

            if (user.Applications.Any(app => app.Action == deviceId))
            {
                lblAttached.Text = "Your device is attached.";
            }
            else
            {
                lblAttached.Text = "Your device is NOT attached.";
            }*/
        }

        private void Attach_Clicked(object sender, EventArgs e)
        {
            var user = Application.Current.Properties["UserObject"] as User;
            var deviceId = DependencyService.Get<IFirebaseToken>().DeviceId;
            var token = Application.Current.Properties["AuthToken"].ToString();

            Models.Application applicationToAdd = new Models.Application()
            {
                Name = "Mobile Device",
                ApplicationType = ApplicationType.Android,
                Action = deviceId
            };

            EasyAccessApi.AddApplication(user.Id, applicationToAdd, token);

            RefreshState();
        }

        private void Detach_Clicked(object sender, EventArgs e)
        {
            var user = Application.Current.Properties["UserObject"] as User;
            var deviceId = DependencyService.Get<IFirebaseToken>().DeviceId;
            var token = Application.Current.Properties["AuthToken"].ToString();

            var appToRemove = user.Applications.FirstOrDefault(app => app.Action == deviceId);

            if (appToRemove != null)
            {
                EasyAccessApi.RemoveApplication(user.Id, appToRemove.Id, token);
            }

            RefreshState();
        }

        private void Allow_Clicked(object sender, EventArgs e)
        {

        }

        private void Deny_Clicked(object sender, EventArgs e)
        {

        }
    }
}
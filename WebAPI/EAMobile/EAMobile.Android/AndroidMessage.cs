using Android.App;
using Android.Widget;
using EAMobile.Droid;
using EAMobile.PlatrformDependentInterfaces;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace EAMobile.Droid
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}
using Android.App;
using Android.Widget;
using EAMobile.Droid;
using EAMobile.PlatrformDependentInterfaces;
using Firebase.Iid;
using static Android.Provider.Settings;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseTokenAndroid))]
namespace EAMobile.Droid
{
    public class FirebaseTokenAndroid : IFirebaseToken
    {
        public string Token => FirebaseInstanceId.Instance.Token; 

        public string DeviceId => Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
    }
}
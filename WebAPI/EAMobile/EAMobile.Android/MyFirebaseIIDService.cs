using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using EAMobile.PlatrformDependentInterfaces;

namespace EAMobile.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseService : FirebaseInstanceIdService
    {
        const string TAG = "FirebaseService";
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            SendRegistrationToServer(refreshedToken);
        }

        void SendRegistrationToServer(string token)
        {
            EasyAccessApi.InsertNewToken(new Models.FirebaseToken()
            {
                DeviceId = new FirebaseTokenAndroid().DeviceId,
                Token = token
            });
        }
    }
}
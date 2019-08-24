using System;
using System.Net;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;

namespace EasyAccessMobile
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        static readonly string TAG = "EasyAccess";

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        TextView msgText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Application.Current.Properties["id"] = 5;

            ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, cert, chain, sslPolicyErrors) => true;

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }

           // msgText = FindViewById<TextView>(Resource.Id.msgText);

            IsPlayServicesAvailable();

            CreateNotificationChannel();

          /*  Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);*/

           /* Button registerButton = FindViewById<Button>(Resource.Id.Register);
            registerButton.Click += RegisterButton_Click;

            Button loginButton = FindViewById<Button>(Resource.Id.Login);
            loginButton.Click += LoginButton_Click;*/
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                msgText.Text = "Google Play Services is available.";
                return true;
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "login", ToastLength.Long).Show();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
           // EasyAccessApi.Register("1111andrey1111@gmail.com", "Kp0c");
            Toast.MakeText(this, "register", ToastLength.Long).Show();
            Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
        //    MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
        /*    if (id == Resource.Id.action_settings)
            {
                return true;
            }*/

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }
	}
}


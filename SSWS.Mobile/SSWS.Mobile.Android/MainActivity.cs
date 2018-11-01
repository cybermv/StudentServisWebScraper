using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Matcha.BackgroundService.Droid;
using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using Matcha.BackgroundService;
using SSWS.Mobile.Droid.Notifications;

namespace SSWS.Mobile.Droid
{
	[Activity (Label = "Student Servis Poslovi", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
            BackgroundAggregator.Init(this);

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
            CreateNotificationChannel();
			LoadApplication (new SSWS.Mobile.App ());
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

            NotificationChannel channel = new NotificationChannel(AndroidJobNotificator.CHANNEL_ID, AndroidJobNotificator.CHANNEL_NAME, NotificationImportance.Default)
            {
                Description = AndroidJobNotificator.CHANNEL_DESCRIPTION
            };

            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}


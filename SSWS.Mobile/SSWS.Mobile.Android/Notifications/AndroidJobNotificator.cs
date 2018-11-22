using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SSWS.Mobile.Notifications;
using SSWS.Mobile.Droid.Notifications;
using Java.Lang;
using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidJobNotificator))]
namespace SSWS.Mobile.Droid.Notifications
{
    public class AndroidJobNotificator : IJobNotificator
    {
        internal static readonly int NOTIFICATION_ID = 67993;
        internal static readonly string CHANNEL_ID = "sswsNotification";
        internal static readonly string CHANNEL_NAME = "Student Servis Poslovi notifikacija";
        internal static readonly string CHANNEL_DESCRIPTION = "Notifikacija o pronađenim novim poslovima";
        internal static readonly string NUMBER_OF_JOBS_KEY = "numberOfJobs";

        public void Notify(int numberOfJobs)
        {
            Context context = Application.Context;

            Bundle valuesForActivity = new Bundle();
            valuesForActivity.PutInt(NUMBER_OF_JOBS_KEY, numberOfJobs);

            Intent appIntent = new Intent(context, typeof(MainActivity));
            appIntent.PutExtras(valuesForActivity);

            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(context);
            stackBuilder.AddParentStack(Class.FromType(typeof(MainActivity)));
            stackBuilder.AddNextIntent(appIntent);
            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);
            
            string messageText;
            switch (numberOfJobs)
            {
                case 1:
                    messageText = $"Pronađen je {numberOfJobs} novi posao!"; break;
                case 2:
                    messageText = $"Pronađeno je {numberOfJobs} nova posla!"; break;
                default:
                    messageText = $"Pronađeno je {numberOfJobs} novih poslova!"; break;
            }
            
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                          .SetVibrate(new long[] { 1000, 75, 75, 150 }) // bzz bzzzz
                          .SetOnlyAlertOnce(true)
                          .SetLights(5669786, 1000, 1000) // SC blue color
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentIntent(resultPendingIntent)
                          .SetContentTitle("Student Servis Poslovi")
                          .SetSmallIcon(Resource.Drawable.ssws_icon)
                          //.SetLargeIcon(Android.Graphics.Bitmap.)
                          .SetContentText(messageText);

            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(NOTIFICATION_ID, builder.Build());
        }
    }
}
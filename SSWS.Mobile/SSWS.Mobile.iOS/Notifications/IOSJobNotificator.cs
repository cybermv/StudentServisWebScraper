using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SSWS.Mobile.Notifications;
using UIKit;

namespace SSWS.Mobile.iOS.Notifications
{
    public class IOSJobNotificator : IJobNotificator
    {
        public void Notify(int numberOfJobs)
        {
            // TODO implement fully
            UILocalNotification notification = new UILocalNotification
            {
                AlertAction = "View Alert",
                AlertBody = "Your one minute alert has fired!",
                ApplicationIconBadgeNumber = 1,
                SoundName = UILocalNotification.DefaultSoundName
            };

            UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
        }
    }
}
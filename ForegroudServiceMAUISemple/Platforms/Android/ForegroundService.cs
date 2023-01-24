using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using Plugin.Maui.Audio;

namespace ForegroudServiceMAUISemple.Platforms.Android
{
    [Service]
    public class ForegroundService : Service
    {
        private string NOTIFICATION_CHANNEL_ID = "1000";
        private int NOTIFICATION_ID = 1;
        private string NOTIFICATION_CHANNEL_NAME = "notification";

        private void StartForegroundService()
        {
            DateTime interval = DateTime.Now
                .AddHours(App.timer.Hours)
                .AddMinutes(App.timer.Minutes)
                .AddSeconds(App.timer.Seconds);

            var now_date = DateTime.Now;
            TimeSpan time = interval.Subtract(now_date);

            var notifcationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                CreateNotificationChannel(notifcationManager);
            }

            Intent notificationIntent = Platform.CurrentActivity?.PackageManager?.GetLaunchIntentForPackage("com.hi.OraOra.semple");
            PendingIntent contentIntent = PendingIntent.GetActivity(Platform.CurrentActivity.ApplicationContext, 0, notificationIntent, 0);

            var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
            notification.SetOngoing(true);
            notification.SetContentIntent(contentIntent);
            notification.SetSmallIcon(Resource.Drawable.ic_clock_black_24dp);
            notification.SetContentTitle("Timer");
            notification.SetContentText($"Time: {time}");
            StartForeground(NOTIFICATION_ID, notification.Build());

            UpdataNotification(contentIntent, time);
        }

        async void UpdataNotification(PendingIntent contentIntent, TimeSpan time)
        {
            TimeSpan interval = time;
            PendingIntent intent = contentIntent;

            while (true)
            {
                var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
                notification.SetOngoing(true);
                notification.SetContentIntent(contentIntent);
                notification.SetSmallIcon(Resource.Drawable.ic_clock_black_24dp);
                notification.SetContentTitle("Timer");
                notification.SetContentText($"Time: {interval}");
                StartForeground(NOTIFICATION_ID, notification.Build());

                TimeSpan tmp = new TimeSpan(interval.Hours, interval.Minutes, interval.Seconds - 1);
                interval = tmp;

                if (tmp.ToString() == "-00:00:01")
                {
                    AlarmManager manager = (AlarmManager)GetSystemService(Context.AlarmService);
                    manager.SetExact(AlarmType.ElapsedRealtime, (long)(SystemClock.ElapsedRealtime() + 1000), intent);

                    break;
                }

                await Task.Delay(1000);
            }

            App.StopForegroundService();
        }

        private void CreateNotificationChannel(NotificationManager notificationMnaManager)
        {
            var channel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME,
            NotificationImportance.Low);
            notificationMnaManager.CreateNotificationChannel(channel);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            StartForegroundService();
            return StartCommandResult.NotSticky;
        }
    }
}

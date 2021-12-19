using System;
using System.Collections.Generic;
using System.Text;

namespace Luna
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void SendNotification(string title, string message, DateTime notifyTimeUTC);
        void ReceiveNotification(string title, string message);
        void CancelAll();
    }
}

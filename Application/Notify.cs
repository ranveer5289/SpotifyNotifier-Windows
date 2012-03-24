/* Author: Ranveer Raghuwanshi
 * Email: ranveer.raghu@gmail.com
 * Stackoverflow: http://stackoverflow.com/users/776084/ranrag */

using System;
using System.Diagnostics;
using System.Text;
using Metadata;

namespace Notification
{
        class Notify
        {
                public void sendNotification(string track, string artist)
                {
                        string command = string.Format("/t:\"Song: {0}\" /i:\"C:\\spotify.jpg\" /n:\"Spotify\" /a:\"Spotify\" \"Artist: {1}\"",track,artist);
                        ProcessStartInfo cmdsi = new ProcessStartInfo("growlnotify.exe");
                        cmdsi.Arguments = command;
                        Process cmd = Process.Start(cmdsi);
                }



        }
}

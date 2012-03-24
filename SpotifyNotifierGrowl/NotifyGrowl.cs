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


                        string ApplicationName = "growlnotify.exe"; //application name

                        string ApplicationArguments = string.Format("/t:\"Song: {0}\" /n:\"Spotify\" /a:\"Spotify\" \"Artist: {1}\"",track,artist); //arguments for growlnotify
                        

                        //Creating process object 
                        Process ProcessObj = new Process();
                        ProcessObj.StartInfo.FileName = ApplicationName;
                        ProcessObj.StartInfo.Arguments = ApplicationArguments;

                        ProcessObj.StartInfo.UseShellExecute = false;
                        ProcessObj.StartInfo.CreateNoWindow = true;


                        ProcessObj.Start();


                }



        }
}


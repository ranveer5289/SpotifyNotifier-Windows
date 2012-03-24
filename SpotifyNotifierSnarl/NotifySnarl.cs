using System;
using System.Diagnostics;
using System.Text;
using Metadata;

namespace Notification
{
        class Notify
        {
                public static Process Obj = null;

                public static void Notification(Process Obj)
                {
                        Obj.StartInfo.UseShellExecute = false;
                        Obj.StartInfo.CreateNoWindow = true;
                        Obj.Start();

                }
                public void sendNotification(string track, string artist)
                {

                        //Application name
                        string ApplicationName = "heysnarl.exe";

                        //Argument to snarl
                        string ApplicationArguments_Register = "\"register?app-sig=app/Spotify&title=SpotifyNotifierSnarl\"";
                        string ApplicationArguments = string.Format("notify?app-sig=app/Spotify&title=Song:{0}&text=Artist:{1}",track,artist);

                        //Creating Process
                        Process ProcessObj = new Process();
                        ProcessObj.StartInfo.FileName = ApplicationName;
                        ProcessObj.StartInfo.Arguments = ApplicationArguments_Register;
                        Notification(ProcessObj);
                        ProcessObj.StartInfo.Arguments = ApplicationArguments;
                        Notification(ProcessObj);

                }



        }
}


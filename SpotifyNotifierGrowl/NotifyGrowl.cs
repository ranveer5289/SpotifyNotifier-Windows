/* Author: Ranveer Raghuwanshi
 * Email: ranveer.raghu@gmail.com
 * Stackoverflow: http://stackoverflow.com/users/776084/ranrag */

using System;
using System.Diagnostics;
using System.Text;
using Metadata;
using System.Xml.XPath;
using System.Xml;

namespace Notification
{
        class Notify
        {
               //private string ApplicationName = "C:\\Program Files (x86)\\Growl for Windows\\growlnotify.exe"; //application name
                private string ApplicationName = "C:\\Program Files\\Growl for Windows\\growlnotify.exe"; //application name

                public void sendNotification(string track, string artist)
                {
                        
                        string CoverUrl = getAlbumArt(track, artist);
                        string ApplicationArguments = string.Format("/t:\"Song: {0}\" {2} /n:\"Spotify\" /a:\"Spotify\" \"Artist: {1}\"",track,artist, !String.IsNullOrEmpty(CoverUrl) ? " /i:" + CoverUrl : ""); //arguments for growlnotify

                        //Creating process object 
                        Process ProcessObj = new Process();
                        ProcessObj.StartInfo.FileName = ApplicationName;
                        ProcessObj.StartInfo.Arguments = ApplicationArguments;

                        ProcessObj.StartInfo.UseShellExecute = false;
                        ProcessObj.StartInfo.CreateNoWindow = true;


                        ProcessObj.Start();

                }

                //Thanks to Matthew Javellana @ mmjavellana@gmail.com
                public void register()
                {
                        string ApplicationArguments = string.Format("/r:Spotify /a:Spotify /ai:\"{0}\\logo.png\" \"Spotify has registered with Growl\"", System.IO.Directory.GetCurrentDirectory()); //arguments for growlnotify


                        //Creating process object 
                        Process ProcessObj = new Process();
                        ProcessObj.StartInfo.FileName = ApplicationName;
                        ProcessObj.StartInfo.Arguments = ApplicationArguments;

                        ProcessObj.StartInfo.UseShellExecute = false;
                        ProcessObj.StartInfo.CreateNoWindow = true;


                        ProcessObj.Start();
                }

                //Thanks to Matthew Javellana @ mmjavellana@gmail.com
                public string getAlbumArt(string track, string artist)
                {
                        string coverUrl = null;

                        try
                        {
                            string apiKey = "b25b959554ed76058ac220b7b2e0a026";
                            string path = "http://ws.audioscrobbler.com/2.0/?method=track.getinfo&api_key=" + apiKey + "&artist=" + artist + "&track=" + track;
                            Console.WriteLine(path);
                            XPathDocument doc = new XPathDocument(path);

                            XPathNavigator navigator = doc.CreateNavigator();
                            XPathNodeIterator nodeImage = navigator.Select("/lfm/track/album/image");

                            while (nodeImage.MoveNext())
                            {
                                XPathNavigator node = nodeImage.Current;
                                coverUrl = node.InnerXml;
                            }                        
                        }
                        catch (Exception)
                        {                       
                        }


                        return coverUrl;
               }
        }
}


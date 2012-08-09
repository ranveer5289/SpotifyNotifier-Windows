/* Author: Ranveer Raghuwanshi
 * Email: ranveer.raghu@gmail.com
 * Stackoverflow: http://stackoverflow.com/users/776084/ranrag */

using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using Metadata;
using System.Xml.XPath;
using System.Xml;
using Growl.Connector;

namespace SpotifyNotification
{
    class Notify
    {


        private string notification_icon_path = null;
        private GrowlConnector growl = null;

        //Create growl object to register application and send notification.
        public void createGrowlObject()
        {
            //Growl object 
            growl = new GrowlConnector();

            //Check growl running or not.
            if (GrowlConnector.IsGrowlRunningLocally())
            {

                //Application name(Spotify) as seen in growl GUI.
                Growl.Connector.Application application = new Growl.Connector.Application("Spotify");


                //Application Icon.
                application.Icon = getCurrentWorkingDirectory();

                NotificationType Spotify_notification_type = new NotificationType("SPOTIFYNOTIFICATION", "Display Notification");
                //Register "Spotify" application
                growl.Register(application, new NotificationType[] { Spotify_notification_type });

                //return growl object to show notification.
                // return growl;

            }
            else
                MessageBox.Show("check Growl running or not","Growl Not Found",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);


        }


        public void sendNotification(string track, string artist)
        {

            string CoverUrl = getAlbumArt(track, artist);

            //Notification specific icon.
            Growl.CoreLibrary.Resource Icon = !String.IsNullOrEmpty(CoverUrl) ? CoverUrl : getCurrentWorkingDirectory();
            Priority priority = 0;

            //If no artist metadata is found set Artist=UNKNOWN
            if(String.IsNullOrEmpty(artist))
                artist = "UNKNOWN";

            Notification notification = new Notification("Spotify", "SPOTIFYNOTIFICATION", null, artist, track,Icon,false,priority,null);


            //Notify growl.
            growl.Notify(notification);

        }


        private string getCurrentWorkingDirectory()
        {
            //Get current working directory.
            String path = Directory.GetCurrentDirectory();
            notification_icon_path = String.Concat(path , @"\logo.png");
            return notification_icon_path;

        }


        //Thanks to Matthew Javellana @ mmjavellana@gmail.com
        private string getAlbumArt(string track, string artist)
        {
            string coverUrl = null;

            try
            {
                string apiKey = "2a2d43dd092fd20165cda5deeb7263e5";
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


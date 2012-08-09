/* Author: Ranveer Raghuwanshi
 * Email: ranveer.raghu@gmail.com
 * Stackoverflow: http://stackoverflow.com/users/776084/ranrag */

using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using Metadata;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Xml;
using SnarlNetworkProtocol;

namespace Notification
{
    class Notify
    {
        private static string hostname = "127.0.0.1";
        private static int hostport = 9887;
        private string appName = "Spotify";
        private string notification_icon_path = null;
        private string timeout = "5";

        private SNP snarl_object = new SNP(hostname, hostport);

        public void sendNotification(string track, string artist)
        {


            //Check Snarl running or not.
            if (snarl_object.isSnarlRunningLocally())
                //Register app with Snarl.
                snarl_object.register(appName);

            else
                MessageBox.Show("check Snarl running or not","Snarl Not Found",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);

            if(String.IsNullOrEmpty(artist))
                artist = "UNKNOWN";

            string CoverUrl = getAlbumArt(track, artist);

            string icon = !String.IsNullOrEmpty(CoverUrl) ? CoverUrl: getCurrentWorkingDirectory();

            //Send notification to Snarl.
            snarl_object.notify(appName, null, track, artist, timeout, icon);

        }

        //Get Album Art from Last.fm
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

        private string encodeForLastFm(string stringToEncode)
        {
            stringToEncode = stringToEncode.Replace(" ", "%20");

            stringToEncode = stringToEncode.Replace("&", "%26");

            return stringToEncode;
        }

        //Get path of default icon(logo.png).
        private string getCurrentWorkingDirectory()
        {
            //Get current working directory.
            String path = Directory.GetCurrentDirectory();
            notification_icon_path = String.Concat(path , @"\logo.png");
            return notification_icon_path;

        }


    }
}

